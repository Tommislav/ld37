using UnityEngine;
using System.Collections;

public class StepTrigger : MonoBehaviour {

    void Start() {
        //gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Player")) {

            // play a sound

            LeanTween.delayedCall(gameObject, 1f, OnTriggerInvoked).setIgnoreTimeScale(true);
            Time.timeScale = 0;

        }
    }

    private void OnTriggerInvoked() {
        Time.timeScale = 1;
        Game.Instace.OnTriggerInvoked();
        gameObject.SetActive(false);
    }
}
