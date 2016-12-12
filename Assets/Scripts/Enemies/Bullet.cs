using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private float speed = 0.05f;
	private Vector2 vec;
	private Animator anim;
	
	void Update () {
		transform.Translate(vec);

		float x = transform.position.x;
		float y = transform.position.y;
		if (x > 7f || x < -7f || y > 5.5f || y < -5.5f) {
			Destroy(gameObject);
		}
	}

	public void Start() {
		anim = Find.ComponentOnGameObject<Animator>(this);
		Find.ComponentOnGameObject<Enemy>(this).SpecialForBullets();
		Vector2 m = transform.position;
		Vector2 p = PlayerControls.Instance.gameObject.transform.position;
		vec = ((p - m).normalized) * speed;
	}

	public void OnHitAnimComplete() {
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.CompareTag("Player")) {
			vec = Vector2.zero;
			anim.Play("Bullet_Hit");

			transform.parent = c.transform;
			//Game.Instace.OnPlayerDamage();
		}
	}
}
