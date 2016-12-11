using UnityEngine;
using System.Collections;

public class Bouncer : MonoBehaviour {

	private Rigidbody2D rb;

	public float f = 100f;

	float vx;
	float vy;

	private int cnt;

	void Start () {
		rb = Find.ComponentOnGameObject<Rigidbody2D>(this);
		vx = Random.value < 0.5f ? -1f : 1f;
		vy = Random.value < 0.5f ? -1f : 1f;
		rb.AddForce((new Vector2(vx, vy) * f), ForceMode2D.Impulse);
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.gameObject.layer != LayerMask.NameToLayer("World")) {
			return;
		}

		Vector2 cPos = coll.contacts[0].point;
		Vector2 me = new Vector2(transform.position.x, transform.position.y);
		Vector2 rel = cPos - me;

		if (Mathf.Abs(rel.x) >= Mathf.Abs(rel.y)) {
			vx = -vx;
		}
		if (Mathf.Abs(rel.x) <= Mathf.Abs(rel.y)) {
			vy = -vy;
		}

		rb.velocity = Vector2.zero;
		rb.AddForce((new Vector2(vx, vy) * f), ForceMode2D.Impulse);
	}

	void Update() {
		if (--cnt <= 0) {
			cnt = 10;

			if (rb.velocity.magnitude > 2f) {
				rb.velocity = rb.velocity.normalized * 2f;
			}
		}
	}
}
