using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour {
    public Transform bpoint; //�Ѿ��� ������ ��
    public GameObject bulletA; //������ �Ѿ�
    [Range(0, 10)]
    public float fireRate; //�߻�ӵ�
    private float curShotDelay = 0f; //�߻��Ľð�
    public int maxMag; //�ִ� ź���
    [HideInInspector]
    public int curMag; //���� ź���
    [Range(0, 5)]
    public float reloadTime;
    public bool isReloading = false;
    public bool isBroken;
    [Range(0, 100)]
    public int breakingProbability;
    [Range(0, 100)]
    public int damage = 40; // 20~60
    [Range(0, 100)]
    public float bspeed = 50f;
    [Range(0, 50)]
    public float glidingTime = 10f;

    public void Start() {
        curMag = maxMag;
        isBroken = false;
    }
    void Update() {
        if (gameObject.GetComponent<PlayerController>().isDied) {
            return;
        }
        if (isReloading) {
            return;
        }
        if (Input.GetKeyDown("r")) {
            StartCoroutine(Reload());
        }
        if (isBroken) {
            return;
        }
        if (curMag <= 0) {
            return; //���� �Ѿ� 0���̸� ����
        }
        if (Input.GetMouseButton(0) && Time.time >= curShotDelay) { //1�� ������ �ּ� �ð����� ���� ���� �ð��� ª���� ����
            curShotDelay = Time.time + 1f / fireRate;
            fire();
        }
    }  //�����Ӵ� �ð��� �󸶳� �������� ����

    void fire() {
        curMag--;
        GameObject bullet = Instantiate(bulletA, bpoint.position, transform.rotation); //�Ѿ� ����{������ ������Ʈ�� ����, ������ ��ġ(���� �������� �����ص���), ����(�÷��̾����� ������)}
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bpoint.up * bspeed, ForceMode2D.Impulse);
        Debug.Log("curMag = " + curMag);
        int randBreak = Random.Range(1, 101);
        Debug.Log("randBreak = " + randBreak);

        if (isBroken == false) {
            if (breakingProbability >= randBreak) { // 5 >= 0~10
                isBroken = true;
                Debug.Log("Gun is Broken!");
            }
        }
    }

    public IEnumerator Reload() {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(isBroken ? reloadTime : reloadTime * 3);
        Debug.Log("Reloading Complete!");

        curMag = maxMag;
        isReloading = false;
        isBroken = false;
    }
}
