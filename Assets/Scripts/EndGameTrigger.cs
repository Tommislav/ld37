using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject.CompareTag("Player")) {
			SceneManager.LoadScene("EndScene");
		}
	}
}
