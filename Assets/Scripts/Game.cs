using UnityEngine;
using System.Collections;
using System;

public class Game : MonoBehaviour {

    public static Game Instace;

    public static bool isRoomTransition;

    public ParticleSystem hitMonsterParticles;

    private int numberOfActiveMonsters;
    private GameObject lockDoors;
    private bool doorsAreLocked;
	private int health = 5;

    void Awake() {
        Instace = this;
        lockDoors = Find.ChildByName(this, "/World/Doors");
        SetLockExits(false);
    }
    private void OnDestroy() {
        Instace = null;
    }


    public void OnEnterNewRoom(string room) {
        LeanTween.delayedCall(gameObject, 1f, SpawnMonster);

        if (numberOfActiveMonsters >= 3) {
            SetLockExits(true);
        }
    }

    private void SpawnMonster() {
        float minX = -6f;
        float maxX = 6f;
        float minY = -4f;
        float maxY = 4f;
        Vector3 monsterPos = new Vector3(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY), -0.7f);
        Vector3 smokePos = new Vector3(0, 0, -0.1f);

        GameObject monster = Instantiate(PrefabRegistry.Instance.enemyBouncer) as GameObject;
        monster.transform.position = monsterPos;

        GameObject smoke = Instantiate(PrefabRegistry.Instance.smokeSpawner) as GameObject;
        smoke.transform.parent = monster.transform;
        smoke.transform.localPosition = smokePos;

        Find.ComponentOnGameObject<SmokeSpawner>(this, smoke).StartSmoke(0.6f, 3, 6, 10, ()=> Find.ComponentOnGameObject<Enemy>(monster).EnemySpawned());
    }

    public void OnLeaveRoom() {

    }

    public void OnPlayerHitMonster(Vector2 position) {
        Vector3 p = new Vector3(position.x, position.y, hitMonsterParticles.transform.position.z);
        hitMonsterParticles.transform.position = p;
        hitMonsterParticles.Play();
    }

    public string GetTextForSign() {
        return "Welcome to the dungeon!";
    }

    public void OnNewMonster() {
        numberOfActiveMonsters++;
    }

    public void OnMonsterKilled() {
        numberOfActiveMonsters--;

        if (doorsAreLocked && numberOfActiveMonsters == 0) {
            SetLockExits(false);
        }
    }

	public void OnPlayerDamage() {
		health--;
		Debug.Log("OnPlayerDamage: " + health);
		Hud.Instance.SetHealth(health);
		if (health <= 0) {
			Debug.Log("Game Over");
		}
	}

	public void OnHealthPickup() {
		health++;
		if (health > 5) {
			health = 5;
		}
		Hud.Instance.SetHealth(health);
	}

    public void OnTriggerInvoked() {
        Find.ChildByName(this, "/World/InteriorCombos/One").SetActive(true);
    }

    public void SetLockExits(bool f) {
        doorsAreLocked = f;
        lockDoors.SetActive(f);
    }
}
