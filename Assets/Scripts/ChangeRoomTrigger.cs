using UnityEngine;
using System.Collections;

public class ChangeRoomTrigger : MonoBehaviour {


	public ChangeRooms.Dir direction;
	
	private void OnTriggerEnter2D(Collider2D coll) {
		if (Game.isRoomTransition) {
			return;
		}

		// if player
		ChangeRooms.Instance.ChangeRoom(direction);
	}
}
