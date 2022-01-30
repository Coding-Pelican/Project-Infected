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
        Vector3 mPosition = Input.mousePosition; 
        Vector3 oPosition = transform.position; 

        
        Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);

        
        float dy = target.y - oPosition.y;
        float dx = target.x - oPosition.x;

        
        float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg - 90; 

        
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
