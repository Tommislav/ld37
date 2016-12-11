using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public static PlayerControls Instance;

    private Rigidbody2D rb;
	private float moveForce = 200f;

	private GameObject swordU;
	private GameObject swordD;
	private GameObject swordL;
	private GameObject swordR;

	private GameObject[] swords;
	private bool isAttacking;

    void Awake() {
		Instance = this;
		rb = Find.ComponentOnGameObject<Rigidbody2D>(this);
		swords = new GameObject[] { Find.ChildByName(this, "SwordU"), Find.ChildByName(this, "SwordD"), Find.ChildByName(this, "SwordL"), Find.ChildByName(this, "SwordR") };
		HideAndGet(-1);
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

		int atk = -1;
		atk = Input.GetKey(KeyCode.W) ? 0:atk;
		atk = Input.GetKey(KeyCode.S) ? 1 : atk;
		atk = Input.GetKey(KeyCode.A) ? 2 : atk;
		atk = Input.GetKey(KeyCode.D) ? 3 : atk;
	
		if (atk != -1 && !isAttacking) {
			LeanTween.cancel(gameObject);
			isAttacking = true;
			GameObject sword = HideAndGet(atk);
			sword.SetActive(true);
			LeanTween.delayedCall(gameObject, 0.18f, () => sword.SetActive(false));
		}
		if (atk == -1) {
			isAttacking = false;
		}
	}

	private GameObject HideAndGet(int dir) {
		for (int i=0; i<swords.Length; i++) {
			swords[i].SetActive(false);
		}
		if (dir >= 0 && dir < swords.Length) {
			return swords[dir];
		}
		return null;
	}
}