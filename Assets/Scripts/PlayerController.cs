using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Range(1, 200)]
    public float health = 100f;
    [Range(0, 100)]
    public float armor = 100f;
    public float thirsty;
    public float hungry;
    [HideInInspector]
    public float speed;
    [Range(0, 10)]
    public float sprintSpeed = 7.7f;
    [Range(0, 10)]
    public float crouchSpeed = 2.3f;
    [Range(0, 10)]
    public float walkingSpeed = 3.7f;
    PlayerShot playerShot;
    SpriteRenderer spriteRenderer;
    public Sprite deadSprite;
    public bool isDied = false;

    Rigidbody2D rb;

    Vector2 movement;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerShot = gameObject.GetComponent<PlayerShot>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (isDied) return;
        if (health <= 0) Die();
        Movement();
    }

    void FixedUpdate() {
        if (isDied) return;
        LookingAtTheMouse();
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }
    void LookingAtTheMouse() {
        Vector3 mPosition = Input.mousePosition; //���콺 ��ǥ ����
        Vector3 oPosition = transform.position; //������Ʈ ��ǥ ����

        //ȭ���� ���콺�� ��ǥ�� ����Ƽ�� ��ǥ�� ��ȭ.
        Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);

        //��ũź��Ʈ(�� �� ���� �� -> ����,ź��Ʈ �Լ��� ���Լ�)�� ���� ������Ʈ�� ��ǥ�� ���콺 ����Ʈ�� ��ǥ��
        //�̿��Ͽ� ������ ���� ��, ���Ϸ�(Euler)ȸ�� �Լ��� ����Ͽ� ���� ������Ʈ�� ȸ����Ű��
        //����, �� ���� �Ÿ����� ���� �� ���Ϸ� ȸ���Լ��� �����ŵ�ϴ�.

        //�� ���� �Ÿ��� ����Ͽ�, dy, dx�� ����
        float dy = target.y - oPosition.y;
        float dx = target.x - oPosition.x;

        //������ ȸ�� �Լ��� 360�� �������� ������ �Է� �޴µ� ���ؼ�
        //��ũź��Ʈ Atan2()�Լ��� ��� ���� ���� ������ ���
        //���� ���� ������ ��ȭ�ϱ� ���� Rad2Deg(=radian to degrees=180/pi)�� ���Ѵ�.
        float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg - 90;

        //������ ������ ���Ϸ� ȸ�� �Լ��� �����Ͽ� z���� �������� ���� ������Ʈ�� ȸ����Ų��.
        transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree);
    }

    void Movement() {
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
    }

    public void TakeDamage(float damageAmount) {
        health -= damageAmount;
        Debug.Log("Health : " + health);
    }
}
