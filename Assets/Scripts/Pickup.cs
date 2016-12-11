using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	void Start () {
		gameObject.SetActive(false);
	}
	
	private void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject.CompareTag("Player")) {
			Game.Instace.OnHealthPickup();
			gameObject.SetActive(false);
		}
	}
}
