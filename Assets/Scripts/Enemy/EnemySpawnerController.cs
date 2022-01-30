using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public GameObject Enemy;
    [Range(0, 100)]
    public float startingSpawnRadius = 25f;
    [Range(0, 500)]
    public int maxEnemy;
    [Range(0, 5)]
    public float spawnDelayTime = 0.5f;

    Vector2 currentPosition;

    IEnumerator spawnCoroutine;

    void Start() {
        spawnCoroutine = Spawn();
        StartCoroutine(spawnCoroutine);
    }

    private void Update() {
        currentPosition.x = gameObject.transform.position.x;
        currentPosition.y = gameObject.transform.position.y;
    }

    void SpawnEnemy() {
        if (GameManager.instance.currentEnemy >= maxEnemy) {
            return;
        }
        GameManager.instance.currentEnemy++;
        Vector2 spawnPosition = Random.insideUnitCircle.normalized * startingSpawnRadius + currentPosition;
        Instantiate(Enemy, spawnPosition, Quaternion.identity);
    }

    IEnumerator Spawn() {
        yield return new WaitForSeconds(spawnDelayTime);
        SpawnEnemy();
        StartCoroutine(spawnCoroutine);
    }
}
