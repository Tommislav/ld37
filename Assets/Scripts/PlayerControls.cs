using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public static PlayerControls Instance;

    private Rigidbody2D rb;
	private float moveForce = 200f;
	private bool isTransition;

    void Awake() {
		Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }
	void OnDestroy() {
		Instance = null;
	}
	public void OnRoomTransitionStarted() {
		rb.velocity = Vector3.zero;
	}
	public void OnRoomTranisiontCompleted() {
	}

	void Update() {
		if (Game.isRoomTransition) {
			return;
		}

		bool moveLeft = Input.GetKey(KeyCode.LeftArrow);
		bool moveRight = Input.GetKey(KeyCode.RightArrow);
		bool moveUp = Input.GetKey(KeyCode.UpArrow);
		bool moveDown = Input.GetKey(KeyCode.DownArrow);
		bool move = moveLeft || moveRight || moveUp || moveDown;

		if(moveLeft) {
			rb.AddForce(new Vector2(-moveForce, 0), ForceMode2D.Force);
		}
		if(moveRight) {
			rb.AddForce(new Vector2(moveForce, 0), ForceMode2D.Force);
		}
		if (moveUp) {
			rb.AddForce(new Vector2(0, moveForce));
		}
		if (moveDown) {
			rb.AddForce(new Vector2(0, -moveForce));
		}

		rb.drag = move ? 5 : 15;
	}
}