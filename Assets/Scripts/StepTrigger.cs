using UnityEngine;
using System.Collections;

public class StepTrigger : MonoBehaviour {

	public bool isEndGameTrigger;


    void Start() {
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Player")) {


            // play a sound
			if (isEndGameTrigger) {
				Game.Instace.OnEndGameTriggerInvoked();
			} else {
				Game.Instace.OnTriggerInvoked();
			}


			gameObject.SetActive(false);
        }
    }

	
}
