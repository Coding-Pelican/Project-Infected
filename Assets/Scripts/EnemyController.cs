using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float health = 100f;
    public float armor = 100f;
    public float moveSpeed = 5.2f;
    public int damage = 20;
    private float distance;

    private GameObject player;
    private Rigidbody2D rb;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;
    public bool isAttacking = false;
    public bool isFollowing = false;

    void Start() {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = Random.Range(moveSpeed / 2, moveSpeed * 1.25f);
        StartCoroutine(Detect(1f));
    }

    void Update() {
        if (health <= 0) {
            Die();
        }
    }

    void FixedUpdate() {
        if (GameManager.instance.isPlayerDied) return;
        //isPlayerDeid 추가하기
        Following(distance);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag.Equals("Player")) {
            if (GameManager.instance.isPlayerDied) return;
            isAttacking = true;
            StartCoroutine(Attack(collision));
            //Debug.Log("Entered");
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (isAttacking) isAttacking = false;
    }

    public void Die() {
        Destroy(gameObject);
    }

    public void TakeDamage(float damageAmount) {
        health -= damageAmount;
    }

    public void Following(float _distance) {
        if (_distance < 20f) {
            isFollowing = true;
            Vector3 lookDir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator Detect(float time) {
        while (true) {
            distance = Vector2.Distance(transform.position, player.transform.position);
            yield return new WaitForSeconds(time);
            //Debug.Log("Distance = " + distance);
        }
    }

    IEnumerator Attack(Collision2D _collision) {
        while (isAttacking) {
            int randomDamage = Random.Range(-damage / 2, damage / 2 + 1);
            Debug.Log("Deals damage to Player");
            _collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage + randomDamage);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Wander() {
        //랜덤 대기 상태 구현하기
        while (!isFollowing) {
            Vector3 targetPos;
            targetPos.x = Random.Range(-5, 5);
            targetPos.y = Random.Range(-5, 5);
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(transform.position.x + targetPos.x, transform.position.y + targetPos.y, transform.position.z), moveSpeed * Time.deltaTime);
            if (isFollowing) break;
            yield return new WaitForSeconds(1);
        }
    }
}
