using UnityEngine;
using System.Collections;
using System;

public class Smoke : MonoBehaviour {

    private Animator animator;

    private Vector3 centerPos;
    private float rad;
    private int numTimes;
    private SmokeSpawner.SmokeCallback callback;

    public void StartSmoke(Vector3 centerPos, float rad, int minTimes, int maxTimes, SmokeSpawner.SmokeCallback callback) {
        this.centerPos = centerPos;
        this.rad = rad;
        numTimes = UnityEngine.Random.Range(minTimes, maxTimes);
        this.callback = callback;

        StartNewSmoke();
    }

    private void StartNewSmoke() {
        if (--numTimes <= 0) {
            // We are done!

            if (callback != null) {
                callback();
                callback = null;
            }

            Destroy(gameObject);
            return;
        }
        animator = Find.ComponentOnGameObject<Animator>(this);

        float r = UnityEngine.Random.value * Mathf.PI * 2f;
        float dist = UnityEngine.Random.Range(0f, rad);
        Vector3 p = centerPos + new Vector3(Mathf.Cos( r ) * dist, Mathf.Sin(r) * dist);
        transform.position = p;

        if (UnityEngine.Random.value < 0.5f) {
            animator.Play("Smoke1");
        } else {
            animator.Play("Smoke2");
        }

    }

    public void OnSmokeAnimationDone_callback() {
        StartNewSmoke();
    }

    void Start() {
        animator = Find.ComponentOnGameObject<Animator>(this);
    }
}
