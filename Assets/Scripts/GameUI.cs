using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameUI: MonoBehaviour {
    public void OnClickNewGameBtn() {
        PrintOnClickButtonName();
    }

    public void OnClickLoadGameBtn() {
        PrintOnClickButtonName();
    }

    public void OnClickOptionsBtn() {
        PrintOnClickButtonName();
    }

    public void OnClickExitBtn() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void PrintOnClickButtonName() {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name + " Å¬¸¯");
    }
}
