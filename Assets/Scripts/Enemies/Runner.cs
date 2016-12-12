using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	private Animator animator;
	private Rigidbody2D rb;

	private int IDLE = 1;
	private int RUNNING = 2;
	private int SHOOTING = 3;
	private int DEAD = 4;
	private int state;

	private float countDown = 0f;
	private bool isStarted;
	private int speedCheck;

	private Vector2 runDir;
	private float runSpeed = 100;
	private float maxSpeed = 1.5f;

	private int collCountdown = 60;
	private float hitCooldown = 0f;
	private float nextShootTime = 0f;

	private Transform childTrans;
	private GameObject shootFrom;

	void Start () {
		animator = Find.ComponentOnGameObject<Animator>(this);
		rb = Find.ComponentOnGameObject<Rigidbody2D>(this);
		childTrans = Find.ComponentOnChild<Transform>(this, "atlas_64");
		shootFrom = Find.ChildByName(this,"shoot");
	}
	

	private void EnemyStart() {
		isStarted = true;
		SetIdle();
	}


	private void SetIdle() {
		state = IDLE;
		countDown = UnityEngine.Random.Range(1, 4); // depending on difficultylevel?
		rb.velocity = Vector2.zero;
		rb.isKinematic = true;
		animator.Play("Runner_Idle");
	}
	private void SetRunning(bool wasAttacked) {
		state = RUNNING;
		countDown = Random.Range(1, 4);
		rb.isKinematic = false;
		animator.Play("Runner_Running");

		int r = Random.Range(0, 4);
		if (r == 0) {
			runDir = Vector2.up * runSpeed;
		} else if (r == 1) {
			runDir = Vector2.down * runSpeed;
		} else if (r == 2) {
			runDir = Vector2.left * runSpeed;
		} else {
			runDir = Vector2.right * runSpeed;
		}
		childTrans.localScale = new Vector3(r == 2 ? -1 : 1, 1);
	}
	private void SetShoot() {
		state = SHOOTING;
		countDown = Random.Range(0.5f, 1.5f);
		rb.isKinematic = true;
		animator.Play("Runner_Shooting");
	}
	private void SetDie() {
		state = DEAD;
		rb.isKinematic = true;
		animator.Play("Runner_Death");
	}

	void Update () {
		if (isStarted) {

			if (state == DEAD) {
				return;
			}

			if (hitCooldown > 0) {
				childTrans.gameObject.SetActive(!childTrans.gameObject.activeSelf);
				hitCooldown -= Time.deltaTime;
				if (hitCooldown <= 0) {
					childTrans.gameObject.SetActive(true);
					SetShoot();
				}
			}

			if (--speedCheck < 0) {
				if (rb.velocity.magnitude > maxSpeed) {
					rb.velocity = rb.velocity.normalized * maxSpeed;
					speedCheck = 10;
				}
			}

			countDown -= Time.deltaTime;

			if (state == RUNNING) {
				rb.AddForce(runDir, ForceMode2D.Force);

				if (countDown < 0) {
					SetIdle();
				}
			}
			else if (state == IDLE) {
				if (countDown < 0) {

					
					if(Random.value < 0.5f && Game.Instace.GetDifficultyLevel() > 2) {
						SetShoot();
					} else {
						SetRunning(false);
					}
				}
			}

			if (state == SHOOTING) {
				if (countDown < 0) {
					if (Random.value < 0.4f) {
						SetIdle();
					} else {
						SetRunning(false);
					}
				}
			}

		}
	}

	public void OnDeathAnimationCompleted() {
		animator.Play("Runner_Dead");
	}


	private void OnEnemyDie() {
		SetDie();
	}

	private void OnEnemyHit() {
		if (state == DEAD) {
			return;
		}
		SetIdle();
		hitCooldown = 1f;
	}

	public void Shoot() {
		
		if (Time.time > nextShootTime) {
			nextShootTime = Time.time + 1f;
			GameObject bullet = Instantiate(PrefabRegistry.Instance.bullet) as GameObject;
			bullet.transform.position = shootFrom.transform.position;
		}
	}
}
