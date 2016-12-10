using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public static Game Instace;

	public static bool isRoomTransition;

	void Awake() {
		Instace = this;
	}
	private void OnDestroy() {
		Instace = null;
	}

	void Update () {
	
	}
}
