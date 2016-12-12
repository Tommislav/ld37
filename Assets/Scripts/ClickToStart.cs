using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonUp("Fire1")) {
			SceneManager.LoadScene("MainScene");
		}
	}
}
