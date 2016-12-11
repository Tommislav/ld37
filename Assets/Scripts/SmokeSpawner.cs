using UnityEngine;
using System.Collections;

public class SmokeSpawner : MonoBehaviour {


    private int activeSmokes;
    private SmokeCallback completeCallback;
    public delegate void SmokeCallback();

    public void StartSmoke(float rad, int numTimes, int numSmokes, SmokeCallback callback) {
        for (int i=0; i<numSmokes; i++) {
            GameObject go = Instantiate(PrefabRegistry.Instance.smoke) as GameObject;
            activeSmokes++;
            Find.ComponentOnGameObject<Smoke>(this, go).StartSmoke(transform.position, rad, numTimes - 3, numTimes + 3, OnSmokeCompleted);
        }
        completeCallback = callback;
    }

    private void OnSmokeCompleted() {
        activeSmokes--;
        if (activeSmokes <= 0) {
            // DONE
            if (completeCallback != null) {
                completeCallback();
            }
            Destroy(gameObject);
        }
    }
}
