using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour {
    public Transform bpoint; //총알이 나가는 곳
    public GameObject bulletA; //나가는 총알
    [Range(0, 10)]
    public float fireRate; //발사속도
    private float curShotDelay = 0f; //발사후시간
    public int maxMag; //최대 탄약수
    [HideInInspector]
    public int curMag; //현재 탄약수
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
            return; //남은 총알 0발이면 제외
        }
        if (Input.GetMouseButton(0) && Time.time >= curShotDelay) { //1발 나가는 최소 시간보다 전에 쐈던 시간이 짧으면 제외
            curShotDelay = Time.time + 1f / fireRate;
            fire();
        }
    }  //프래임당 시간이 얼마나 지났는지 갱신

    void fire() {
        curMag--;
        GameObject bullet = Instantiate(bulletA, bpoint.position, transform.rotation); //총알 생성{생성할 오브젝트의 원본, 생성될 위치(따로 한점으로 지정해뒀음), 방향(플래이어한테 가져옴)}
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
