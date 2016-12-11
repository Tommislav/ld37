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

	private int numberOfRooms = 0;
	private int numberOfMonstersInRoom = 0;
	private int healthAtStartOfRoom = 0;
	private string room0 = "";
	private string room1 = "";
	private string rooms = "";
	private bool checkForHealthPickupAtEnd;

    void Awake() {
        Instace = this;
        lockDoors = Find.ChildByName(this, "/World/Doors");
        SetLockExits(false);
    }
    private void OnDestroy() {
        Instace = null;
    }


    public void OnEnterNewRoom(string room) {
		numberOfRooms++;
		room1 = room0;
		room0 = room;
		rooms = room0 + room1;
		Debug.Log(rooms + ", " + numberOfRooms);

		StartCoroutine(SpawnMonsters());

		
		numberOfMonstersInRoom = numberOfActiveMonsters;
		healthAtStartOfRoom = health;

        if (numberOfActiveMonsters >= 3) {
            SetLockExits(true);
        }
    }

	private IEnumerator SpawnMonsters() {
		checkForHealthPickupAtEnd = false;
		if (numberOfRooms < 2) { // 0, 1
			numberOfActiveMonsters += 1;
			yield return new WaitForSeconds(1);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncer);
		}
		else if (numberOfRooms == 2) {
			numberOfActiveMonsters += 3;
			checkForHealthPickupAtEnd = true;
			SetLockExits(true);
			yield return new WaitForSeconds(1);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncer);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncer);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncer);
		}






		yield return null;
	}

    private void SpawnMonster(GameObject enemyPrefab) {
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

		if (numberOfRooms < 2) { // 0 or 1, first two rooms
			return "Move with arrows, attack with WASD";
		}
		if (numberOfRooms == 2) {
			return "It seems to be the same room over and over...";
		}
		if (numberOfRooms == 3) {
			return "Keep going left for a nice surprise!";
		}

        return "Welcome to the dungeon!";
    }

    public void OnNewMonster() {
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
