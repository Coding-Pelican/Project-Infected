using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [HideInInspector]
    public static readonly WaitForSeconds detectWaitForSecond1s = new WaitForSeconds(1f);

    private GameObject player;
    private Rigidbody2D rb;

    public float health = 100f;
    //public float armor = 100f;
    public float movementSpeed = 5.2f;
    public int damage = 20;
    public float detectionDistance;
    private float distanceBetweenPlayer;
    [Range(0, 5)]
    public float attackDelay;
    private float targetAngle = 0;
    public float deltaTimeToTarget = 0;
    private float wanderTime = 0;

    public bool isReadyToAttack = false;
    public bool isFollowing = false;
    public bool isWandering = false;
    public bool isIdle = false;

    IEnumerator detectPlayerByDistanceCoroutine;
    IEnumerator checkAttackCoroutine;

    //Vector2 testVector2;

    private void Awake() {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        movementSpeed = Random.Range(movementSpeed / 2, movementSpeed * 1.25f);
    }

    private void Start() {
        detectPlayerByDistanceCoroutine = DetectPlayerByDistance(detectionDistance);
        checkAttackCoroutine = CheckAttack();
        StartCoroutine(detectPlayerByDistanceCoroutine);
        StartCoroutine(checkAttackCoroutine);
    }

    private void Update() {
        if (GameManager.instance.isPlayerDied) {
            return;
        }
        if (health <= 0) {
            Die();
        }
        if (isFollowing) {
            FollowPlayer();
        } else if (isWandering){
            Wander();
            //TestWonder();
        } else {
            if (!isIdle) {
                StartCoroutine(Idle());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.tag.Equals("Player")) {
            if (GameManager.instance.isPlayerDied || !isReadyToAttack) {
                return;
            }
            Debug.Log("Deals damage to Player");
            int randomDamage = Random.Range(-damage / 2, damage / 2 + 1);
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage + randomDamage);
            isReadyToAttack = false;
            //Debug.Log("Entered");
        }
    }

    public void Die() {
        GameManager.instance.currentEnemy--;
        Destroy(gameObject);
    }

    public void TakeDamage(float damageAmount) {
        health -= damageAmount;
        Debug.Log("Enemy health : " + health);
    }

    public void FollowPlayer() {
        isWandering = false;
        Vector3 lookDir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
    }

    private IEnumerator Idle() {
        isIdle = true;
        Debug.Log("Enemy State is idle");
        float time = Random.Range(3f, 15f);
        yield return new WaitForSeconds(time);
        if (isFollowing) {
            isIdle = false;
            yield break;
        } else {
            SetWandering();
        }
        isIdle = false;
    }

    private void SetWandering() {
        Debug.Log("Enemy State is Setting Wandering");
        isWandering = true;
        targetAngle = Random.Range(0, 360);
        deltaTimeToTarget = Random.Range(1f, 3f);
        wanderTime = deltaTimeToTarget;
        //testVector2 = transform.position;
    }

    //private void TestWonder() {
    //    if (isFollowing) {
    //        isWandering = false;
    //        return;
    //    }
    //    Debug.Log("Enemy State is Wandering");
    //    float speed = movementSpeed * deltaTimeToTarget;
    //    testVector2.x = speed * Mathf.Cos(targetAngle);
    //    testVector2.y = speed * Mathf.Sin(targetAngle);
    //    transform.eulerAngles = new Vector3(0, 0, targetAngle);
    //    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + testVector2.x, transform.position.y + testVector2.y), movementSpeed * Time.deltaTime);
    //    wanderTime -= Time.deltaTime;
    //    if (wanderTime <= 0) {
    //        isWandering = false;
    //        return;
    //    }
    //}

    private void Wander() {
        if (isFollowing) {
            isWandering = false;
            return;
        }
        Debug.Log("Enemy State is Wandering");
        transform.eulerAngles = new Vector3(0, 0, targetAngle);
        transform.Translate(Vector3.up.normalized * (movementSpeed * 0.5f) * Time.deltaTime);
        wanderTime -= Time.deltaTime;
        if (wanderTime <= 0) {
            isWandering = false;
            return;
        }
    }

    private IEnumerator CheckAttack() {
        if (!isReadyToAttack) {
            isReadyToAttack = true;
        }
        yield return new WaitForSeconds(attackDelay);
        StartCoroutine(checkAttackCoroutine);
    }

    private IEnumerator DetectPlayerByDistance(float _detectionDistance) {
        while (true) {
            yield return detectWaitForSecond1s;
            distanceBetweenPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceBetweenPlayer < _detectionDistance) {
                isFollowing = true;
                Debug.Log("Detected Player!");
            }
            //Debug.Log("Distance = " + distance);
        }
    }
}