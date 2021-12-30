using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    //[Range(0, 100)]
    //public int damage = 40; // 20~60
    //[Range(0, 100)]
    //public float bspeed = 50f;
    //[Range(0, 50)]
    //public float glidingTime = 10f;

    PlayerShot playerShot;
    [SerializeField] int damage;
    [SerializeField] float glidingTime;
    [SerializeField] float bspeed;

    Vector3 startPos;
    Vector3 currentPos;

    private void Start() {
        startPos = transform.position;
        playerShot = GameObject.Find("Player").GetComponent<PlayerShot>();
        damage = playerShot.damage;
        glidingTime = playerShot.glidingTime;

        StartCoroutine(DestroySelfAfterSeconds(glidingTime));
    }
    void Update() {
        currentPos = transform.position;
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag.Equals("Bullet")) return;

        int randomDamage = Random.Range(-damage / 2, damage / 2 + 1);
        if (collision.gameObject.tag == "border") {
            Debug.Log("Hit wall");
        } else if (collision.transform.tag == "Enemy") {
            Debug.Log("Deals damage to Enemy");
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage + randomDamage);
        }
        Destroy(gameObject);
    }
    IEnumerator DestroySelfAfterSeconds(float destroyTime) {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
