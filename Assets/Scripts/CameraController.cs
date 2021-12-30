using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public GameObject player;  //cam��� GameObject���� ����
    Transform cam;
    void Start() {
        cam = player.transform;
    }
    void LateUpdate() {
        transform.position = new Vector3(cam.position.x, cam.position.y, transform.position.z);
    }
}