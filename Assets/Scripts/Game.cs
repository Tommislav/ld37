using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public static Game Instace;

	public static bool isRoomTransition;

	public bool isEndOfGame;

	public ParticleSystem hitMonsterParticles;

	private int numberOfActiveMonsters;
	private GameObject lockDoors;
	private GameObject endgameDoor;
	private bool doorsAreLocked;
	private int health = 5;

	private bool isRandomInterior;



	private GameObject healthPickup;
	private GameObject positions;
	private GameObject randomInteriors;
	private GameObject trigger1;
	private GameObject endGameTrigger;

	private int numberOfRooms = 5;
	private int numberOfMonstersInRoom = 0;
	private int healthAtStartOfRoom = 0;
	private string room0 = "";
	private string room1 = "";
	private string rooms = "";
	private bool checkForHealthPickupAtEnd;
	private int roomEndCondition;
	private int difficultyLevel = 0;

	public int GetDifficultyLevel() {
		return difficultyLevel;
	}

	void Awake() {
		Instace = this;
		lockDoors = Find.ChildByName(this, "/World/Doors");
		healthPickup = Find.ChildByName(this, "/World/Pickups/HeartPickup");
		positions = Find.ChildByName(this, "/World/Pos");
		randomInteriors = Find.ChildByName(this, "/World/InteriorCombos/Rnd");
		trigger1 = Find.ChildByName(this, "/World/StepTriggers/RandomTrigger");
		endGameTrigger = Find.ChildByName(this, "/World/StepTriggers/EndGameTrigger");
		endgameDoor = Find.ChildByName(this, "/World/EndingDoor");
		SetLockExits(false);
	}
	private void OnDestroy() {
		Instace = null;
		LeanTween.cancel(gameObject);
	}


	public void OnEnterNewRoom(string room) {
		numberOfRooms++;
		room1 = room0;
		room0 = room;
		rooms = room0 + room1;

		if(room == "U" && numberOfRooms > 2) {
			difficultyLevel++;
		}

		Debug.Log(rooms + ", " + numberOfRooms);

		StartCoroutine(SpawnMonsters());


		numberOfMonstersInRoom = numberOfActiveMonsters;
		healthAtStartOfRoom = health;
	}

	private IEnumerator SpawnMonsters() {
		checkForHealthPickupAtEnd = false;
		if(numberOfRooms < 2) { // 0, 1
			numberOfActiveMonsters += 1;
			yield return new WaitForSeconds(1);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncer);
		} else if(numberOfRooms == 2) {
			numberOfActiveMonsters += 3;
			checkForHealthPickupAtEnd = true;
			SetLockExits(true);
			yield return new WaitForSeconds(2.5f);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncer);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncer);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncer);
		} else if(numberOfRooms == 3) {
			numberOfActiveMonsters += 4;
			yield return new WaitForSeconds(1);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncerSmall, "TL");
			yield return new WaitForSeconds(1);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncerSmall, "TR");
			yield return new WaitForSeconds(1);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncerSmall, "BL");
			yield return new WaitForSeconds(1);
			SpawnMonster(PrefabRegistry.Instance.enemyBouncerSmall, "BR");
		} else if(difficultyLevel == 4) {
			// spawn end of game boss!
			SetLockExits(true);
			numberOfActiveMonsters += 1;
			yield return new WaitForSeconds(1);
			SpawnMonster(PrefabRegistry.Instance.endBoss, "Boss");
		} else {

			if(room1 == "R" || numberOfRooms % 3 == 0 || health <= 2) {
				checkForHealthPickupAtEnd = true;
			}

			GameObject[] enemies = new GameObject[0];
			int max = difficultyLevel > 2 ? 8 : 6;
			int rnd = UnityEngine.Random.Range(0, max);

			if (rnd == 3) {
				enemies = new GameObject[] {
					PrefabRegistry.Instance.enemyRunner
				};
			} else if (rnd == 2) {
				enemies = new GameObject[] {
					PrefabRegistry.Instance.enemyBouncer,
					PrefabRegistry.Instance.enemyRunner
				};
			} else if (rnd == 0) {
				enemies = new GameObject[] {
					PrefabRegistry.Instance.enemyBouncerSmall,
					PrefabRegistry.Instance.enemyBouncerSmall,
					GetRandomEnemy()
				};
			} else if (rnd == 1) {
				enemies = new GameObject[] {
					PrefabRegistry.Instance.enemyRunnerBig,
					PrefabRegistry.Instance.enemyBouncer,
					PrefabRegistry.Instance.enemyBouncer
				};
			} else if (rnd == 4) {
				enemies = new GameObject[] {
					PrefabRegistry.Instance.enemyBouncerSmall,
					PrefabRegistry.Instance.enemyBouncerSmall,
					PrefabRegistry.Instance.enemyBouncerSmall,
					PrefabRegistry.Instance.enemyBouncerSmall,
					PrefabRegistry.Instance.enemyBouncerSmall,
					GetRandomEnemy()
				};
			} else if (rnd == 6) {
				enemies = new GameObject[] {
					PrefabRegistry.Instance.enemyBouncer,
					GetRandomEnemy(),
					GetRandomEnemy()
				};
			} else if (rnd == 5) {
				enemies = new GameObject[] {	
					GetRandomEnemy(),
					GetRandomEnemy()
				};
			} else if (rnd == 7) {
				enemies = new GameObject[] {
					PrefabRegistry.Instance.enemyRunnerBig,
					PrefabRegistry.Instance.enemyRunnerBig,
					PrefabRegistry.Instance.enemyRunner
				};
			}

			string[] spawnPos = new string[] { "", "", "", "", "TL", "TL2", "TR", "TR2", "BL", "BL2", "BR", "BR2", "C" };
			for(int i = 0; i < spawnPos.Length; i++) {
				string temp = spawnPos[i];
				int r = UnityEngine.Random.Range(i, spawnPos.Length);
				spawnPos[i] = spawnPos[r];
				spawnPos[r] = temp;
			}

			numberOfActiveMonsters += enemies.Length;
			for(int i = 0; i < enemies.Length; i++) {
				SpawnMonster(enemies[i], spawnPos[i]);
				yield return new WaitForSeconds(UnityEngine.Random.value);
			}

		}



		yield return null;
	}

	private GameObject GetRandomEnemy() {
		GameObject[] g = new GameObject[] {
			PrefabRegistry.Instance.enemyBouncer,
			PrefabRegistry.Instance.enemyBouncerSmall,
			PrefabRegistry.Instance.enemyRunner,
			PrefabRegistry.Instance.enemyRunnerBig
		};
		int r = UnityEngine.Random.Range(0, g.Length);
		return g[r];
	}


	private void SpawnMonster(GameObject enemyPrefab, string pos = "") {
		float minX = -6f;
		float maxX = 6f;
		float minY = -4f;
		float maxY = 4f;

		Vector3 monsterPos = (pos != "") ? GetPos(pos) : new Vector3(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY), -0.7f);
		Vector3 smokePos = new Vector3(0, 0, -0.1f);

		GameObject monster = Instantiate(enemyPrefab) as GameObject;
		monster.transform.position = monsterPos;

		GameObject smoke = Instantiate(PrefabRegistry.Instance.smokeSpawner) as GameObject;
		smoke.transform.parent = monster.transform;
		smoke.transform.localPosition = smokePos;

		if(pos == "Boss") {
			Find.ComponentOnGameObject<SmokeSpawner>(this, smoke).StartSmoke(1.6f, 3, 6, 30, () => Find.ComponentOnGameObject<Enemy>(monster).EnemySpawned());
		} else {
			Find.ComponentOnGameObject<SmokeSpawner>(this, smoke).StartSmoke(0.6f, 3, 6, 10, () => Find.ComponentOnGameObject<Enemy>(monster).EnemySpawned());
		}
	}

	public void OnLeaveRoom() {

	}

	public void OnPlayerHitMonster(Vector2 position) {
		Vector3 p = new Vector3(position.x, position.y, hitMonsterParticles.transform.position.z);
		hitMonsterParticles.transform.position = p;
		hitMonsterParticles.Play();
	}

	public string GetTextForSign() {

		if(numberOfRooms < 2) { // 0 or 1, first two rooms
			return "Move with arrows, attack with WASD";
		}
		if(numberOfRooms == 2) {
			return "It seems to be the same room over and over...";
		}
		if(numberOfRooms == 3) {
			return "Stronger creatures lurks northward!";
		}

		return GetRandomString(new string[] {
				"Legend talks of a purple SWORD",
				"Getting bored of same room? Refurnish!",
				"Stronger creatures lurks northward!",
				"The final challenge is found to the north!",
				"Be prepared if you're heading north!",
				"Redecorating triggers are often found south"
		});
	}

	public void OnNewMonster() {
	}

	public void OnMonsterKilled() {
		numberOfActiveMonsters--;

		if(numberOfActiveMonsters == 0) {
			if(doorsAreLocked) {
				SetLockExits(false);
			}

			if(checkForHealthPickupAtEnd && health < 5) {
				healthPickup.SetActive(true);
			} else {
				if(numberOfRooms == 3 || (room1 == "D")) {
					trigger1.SetActive(true);
				}
			}
		}
	}

	public void OnPlayerDamage() {
		health--;
		Debug.Log("OnPlayerDamage: " + health);
		Hud.Instance.SetHealth(health);
		if(health <= 0) {
			LeanTween.delayedCall(gameObject, 2, () => {
				SceneManager.LoadScene("GameOver");
			});
		}
	}

	public void OnHealthPickup() {
		health++;
		if(health > 5) {
			health = 5;
		}
		Hud.Instance.SetHealth(health);
	}

	public void OnTriggerInvoked() {
		Transform t = randomInteriors.transform;
		int rnd = UnityEngine.Random.Range(0, t.childCount);
		for(int i = 0; i < t.childCount; i++) {
			t.GetChild(i).gameObject.SetActive(i == rnd);
		}
		// shake camera?
	}

	public void EnableEndGameTrigger() {
		endGameTrigger.SetActive(true);
	}

	public void OnEndGameTriggerInvoked() {
		endgameDoor.SetActive(false);
	}

	public void SetLockExits(bool f) {
		doorsAreLocked = f;
		lockDoors.SetActive(f);
	}

	private string GetRandomString(string[] s) {
		return s[UnityEngine.Random.Range(0, s.Length)];
	}

	private Vector3 GetPos(string name) {
		Transform t = Find.ComponentOnChild<Transform>(this, positions, name);
		return t.position;
	}

	public bool IsAlive() {
		return health > 0;
	}

}