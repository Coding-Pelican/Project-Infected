using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public bool isPlayerDied;

    private void Awake() {
        instance = this;
        isPlayerDied = false;
    }
}
