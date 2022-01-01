using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Range(1, 200)]
    public float maxHealth = 100f;
    public float currentHealth;
    [Range(0, 100)]
    //public float armor = 100f;
    //public float thirsty;
    //public float hungry;
    [HideInInspector]
    public float speed;
    [Range(0, 10)]
    public float sprintSpeed = 7.7f;
    [Range(0, 10)]
    public float crouchSpeed = 2.3f;
    [Range(0, 10)]
    public float walkingSpeed = 3.7f;

    [Range(0, 30)]
    public float respawnTime = 5f;
    public bool isDied = false;

    public Sprite defaultSprite;
    public Sprite deadSprite;

    public GameObject spawnPoint;
    private Transform spawn;
    private PlayerShot playerShot;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    Vector2 movement;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerShot = GetComponent<PlayerShot>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start() {
        currentHealth = maxHealth;
        spawn = spawnPoint.transform;
    }

    private void Update() {
        if (isDied) {
            return;
        }
        if (currentHealth <= 0) {
            Die();
        }
        Move();
    }

    private void FixedUpdate() {
        if (isDied) {
            return;
        }
        LookingAtTheMouse();
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    private void LookingAtTheMouse() {
        Vector3 mPosition = Input.mousePosition; //마우스 좌표 저장
        Vector3 oPosition = transform.position; //오브젝트 좌표 저장

        //화면의 마우스의 좌표를 유니티의 좌표로 변화.
        Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);

        //아크탄젠트(두 변 길이 비 -> 각도,탄젠트 함수의 역함수)로 게임 오브젝트의 좌표와 마우스 포인트의 좌표를
        //이용하여 각도를 구한 후, 오일러(Euler)회전 함수를 사용하여 게임 오브젝트를 회전시키기
        //위해, 각 축의 거리차를 구한 후 오일러 회전함수에 적용시킵니다.

        //각 축의 거리를 계산하여, dy, dx에 저장
        float dy = target.y - oPosition.y;
        float dx = target.x - oPosition.x;

        //오릴러 회전 함수는 360도 기준으로 각도를 입력 받는데 반해서
        //아크탄젠트 Atan2()함수의 결과 값은 라디안 값으로 출력
        //라디안 값을 각도로 변화하기 위해 Rad2Deg(=radian to degrees=180/pi)를 곱한다.
        float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg - 90;

        //구해진 각도를 오일러 회전 함수에 적용하여 z축을 기준으로 게임 오브젝트를 회전시킨다.
        transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree);
    }

    private void Move() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (!playerShot.isReloading) {
            if (Input.GetAxisRaw("speed_control") > 0) {
                speed = sprintSpeed;
            } else if (Input.GetAxisRaw("speed_control") < 0) {
                speed = crouchSpeed;
            } else {
                speed = walkingSpeed;
            }
        } else {
            speed = crouchSpeed;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) == 2) {
            speed /= Mathf.Sqrt(2);
        }
    }

    public void Die() {
        isDied = true;
        GameManager.instance.isPlayerDied = true;
        spriteRenderer.sprite = deadSprite;
        Invoke("Respawn", respawnTime);
        Debug.Log("Player is Dead");
    }

    public void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        Debug.Log("Health : " + currentHealth);
    }

    public void Respawn() {
        transform.position = spawn.position;
        currentHealth = maxHealth;
        isDied = false;
        GameManager.instance.isPlayerDied = false;
        spriteRenderer.sprite = defaultSprite;
        GetComponent<PlayerShot>().Start();
    }
}
