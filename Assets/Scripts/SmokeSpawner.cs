using UnityEngine;
using System.Collections;

public class SmokeSpawner : MonoBehaviour {


    private int activeSmokes;
    private SmokeCallback completeCallback;
    public delegate void SmokeCallback();

    public void StartSmoke(float rad, int minTimes, int maxTimes, int numSmokes, SmokeCallback callback) {
        for (int i=0; i<numSmokes; i++) {
            GameObject go = Instantiate(PrefabRegistry.Instance.smoke) as GameObject;
            go.transform.parent = transform;
            activeSmokes++;
            Find.ComponentOnGameObject<Smoke>(this, go).StartSmoke(rad, minTimes, maxTimes, OnSmokeCompleted);
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
