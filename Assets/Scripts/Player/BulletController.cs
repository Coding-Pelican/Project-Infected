using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    [SerializeField] int damage;
    [SerializeField] float glidingTime;

    private PlayerShot playerShot;

    IEnumerator destroySelfAfterSecondsCoroutine;
    private void Awake() {
        playerShot = GameObject.Find("Player").GetComponent<PlayerShot>();
        damage = playerShot.damage;
        glidingTime = playerShot.glidingTime;
    }

    private void Start() {
        destroySelfAfterSecondsCoroutine = DestroySelfAfterSeconds(glidingTime);
        StartCoroutine(destroySelfAfterSecondsCoroutine);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag.Equals("Bullet")) {
            return;
        }

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
