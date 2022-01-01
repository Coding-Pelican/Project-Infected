using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public GameObject Enemy;
    [Range(0, 100)]
    public float startSpawnRadius = 25f;
    [Range(0, 500)]
    public int maxEnemy;
    [Range(0, 5)]
    public float time = 0.5f;

    Vector2 curPos;

    void Start() {
        StartCoroutine(Spawn());
    }

    private void Update() {
        curPos.x = gameObject.transform.position.x;
        curPos.y = gameObject.transform.position.y;
    }

    void SpawnEnemy() {
        if (GameManager.instance.curEnemy >= maxEnemy) {
            return;
        }
        GameManager.instance.curEnemy++;
        Vector2 spawnPos = Random.insideUnitCircle.normalized * startSpawnRadius + curPos;
        Instantiate(Enemy, spawnPos, Quaternion.identity);
    }

    IEnumerator Spawn() {
        yield return new WaitForSeconds(time);
        SpawnEnemy();
        StartCoroutine(Spawn());
    }
}
