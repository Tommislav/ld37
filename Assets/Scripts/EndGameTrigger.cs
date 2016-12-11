using UnityEngine;
using System.Collections;

public class EndGameTrigger : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject.CompareTag("Player")) {
			Debug.Log("END GAME");
		}
	}
}
