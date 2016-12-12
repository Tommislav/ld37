using UnityEngine;
using System.Collections;

public class EndBoss : MonoBehaviour {

	private ParticleSystem ps;
	private Transform shoot;
	private Animator anim;

	private bool isShooting;
	private bool isDead;
	private float shootTimer;

	private bool isBlinking;
	private float blinkTimer;
	private GameObject gfx;

	void Start () {
		ps = Find.ComponentOnChild<ParticleSystem>(this, "ps");
		shoot = Find.ComponentOnChild<Transform>(this, "shoot");
		anim = Find.ComponentOnGameObject<Animator>(this);
		gfx = Find.ChildByName(this, "atlas_88");
	}
	
	private void EnemyStart() {
		Idle();
	}

	private void Shoot() {
		if (isDead) {
			return;
		}
		shootTimer = 0.3f;
		isShooting = true;
		anim.Play("Boss_Shooting");
		LeanTween.delayedCall(gameObject, Random.Range(0.5f,2f), Idle);
	}
	private void Idle() {
		if (isDead) {
			return;
		}
		isShooting = false;
		anim.Play("Boss_Idle");
		LeanTween.delayedCall(gameObject, Random.Range(1, 3), Shoot);
	}
	private void Fire() {
		GameObject bullet = Instantiate(PrefabRegistry.Instance.bullet) as GameObject;
		bullet.transform.position = shoot.position;

	}

	void Update () {
		if (isBlinking) {
			gfx.SetActive(!gfx.activeSelf);
			if (Time.time > blinkTimer) {
				isBlinking = false;
			}
		} else {
			gfx.SetActive(true);
		}


		if (isShooting) {
			shootTimer -= Time.deltaTime;
			if (shootTimer <= 0) {
				Fire();
				shootTimer = 0.25f;
			}
		}
	}


	private void OnEnemyHit() {
		isBlinking = true;
		blinkTimer = Time.time + 1f;
	}
	private void OnEnemyDie() {
		Idle();
		LeanTween.cancel(gameObject);
		isBlinking = false;
		ps.Play();

		LeanTween.delayedCall(gameObject, 4, () => {
			
			ps.Stop();
			Game.Instace.isEndOfGame = true;
			Game.Instace.EnableEndGameTrigger();
			anim.Play("Boss_Dead");

		});
	}


	private void OnDestroy() {
		LeanTween.cancel(gameObject);
	}
}
