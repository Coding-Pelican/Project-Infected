using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timerText;
    public bool is2022Year = false;
    public GameObject EnemySpawner;
    IEnumerator enableEnemySpawnerCoroutine;

    public static string GetCurrentDate() {
        return DateTime.Now.ToString(("yyyy.MM.dd. HH:mm:ss")); //yyyy-MM-dd HH:mm:ss tt
    }

    public static string GetYear() {
        return DateTime.Now.ToString(("yyyy"));
    }

    public static string GetTimeLeftUntilNextYear() {
        string hour = Convert.ToString(23 - DateTime.Now.Hour);
        string minute = Convert.ToString(59 - DateTime.Now.Minute);
        string second = Convert.ToString(60 - DateTime.Now.Second);
        return (hour.Length > 1 ? hour : "0" + hour) + ":" + (minute.Length > 1 ? minute : "0" + minute) + ":" + (second.Length > 1 ? second : "0" + second);
    }

    public void Start() {
        enableEnemySpawnerCoroutine = EnableEnemySpawner();
        StartCoroutine(enableEnemySpawnerCoroutine);
    }

    public void Update() {
        dateText.text = GetCurrentDate();
        if (DateTime.Now.ToString(("yyyy.MM.dd. HH:mm:ss")) == "2022.01.01. 00:00:00") is2022Year = true;
        if (is2022Year) return;
        timerText.text = GetTimeLeftUntilNextYear();
    }

    IEnumerator EnableEnemySpawner() {
        while (!is2022Year) {
            yield return new WaitForSeconds(1f);
        }
        EnemySpawner.SetActive(true);
        Debug.Log("isActive");
    }
}
    
