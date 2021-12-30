using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public GameObject player;  //cam라는 GameObject변수 선언
    Transform cam;
    void Start() {
        cam = player.transform;
    }
    void LateUpdate() {
        transform.position = new Vector3(cam.position.x, cam.position.y, transform.position.z);
    }
}