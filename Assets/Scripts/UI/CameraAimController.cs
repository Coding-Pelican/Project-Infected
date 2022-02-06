using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAimController : MonoBehaviour {
    public Transform player;
    Vector3 target, mousePosition, refVel, shakeOffset;

    [Range(0f, 10f)]
    public float cameraDistance = 3.5f;
    [Range(0f, 0.5f)]
    public float smoothTime = 0.2f;
    private float zStart;
    [Range(0f, 1f)]
    public float shakeMagnitude, shakeTimeEnd;
    public bool isShaking = false;
    Vector3 shakeVector;

    private void Start() {
        target = player.position;
        zStart = transform.position.z;
    }
    void Update() {
        mousePosition = CaptureMousePosition();
        shakeOffset = UpdateShake();
        target = UpdateTargetPosition();
        UpdateCameraPosition();
    }

    private Vector3 UpdateShake() {
        if (!isShaking || Time.time > shakeTimeEnd) {
            isShaking = false;
            return Vector3.zero;
        }
        Vector3 tempOffset = shakeVector;
        tempOffset *= shakeMagnitude;
        return tempOffset;
    }

    public void Shake(Vector3 direction, float magnitude, float length) {
        isShaking = true;
        shakeVector = direction;
        shakeMagnitude = magnitude;
        shakeTimeEnd = Time.time + length;
    }

    private void UpdateCameraPosition() {
        Vector3 tempPositon;
        tempPositon = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPositon;
    }

    private Vector3 UpdateTargetPosition() {
        Vector3 mouseOffset = mousePosition * cameraDistance;
        Vector3 ret = player.position + mouseOffset;
        ret += shakeOffset;
        ret.z = zStart;
        return ret;
    }

    private Vector3 CaptureMousePosition() {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max) {
            ret = ret.normalized;
        }
        return ret;
    }
}