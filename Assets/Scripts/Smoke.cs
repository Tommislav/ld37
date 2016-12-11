using UnityEngine;
using System.Collections;
using System;

public class Smoke : MonoBehaviour {

    private Animator animator;

    private float rad;
    private int numTimes;
    private SmokeSpawner.SmokeCallback callback;

    public void StartSmoke(float rad, int minTimes, int maxTimes, SmokeSpawner.SmokeCallback callback) {
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
        Vector3 p = new Vector3(Mathf.Cos( r ) * dist, Mathf.Sin(r) * dist);
        transform.localPosition = p;

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
