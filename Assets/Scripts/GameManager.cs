using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public bool isPlayerDied;
    public int currentEnemy;

    private void Awake() {
        instance = this;
        isPlayerDied = false;
        currentEnemy = 0;
    }
}
