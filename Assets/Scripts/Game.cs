using UnityEngine;
using System.Collections;
using System;

public class Game : MonoBehaviour {

    public static Game Instace;

    public static bool isRoomTransition;

    public ParticleSystem hitMonsterParticles;

    void Awake() {
        Instace = this;
    }
    private void OnDestroy() {
        Instace = null;
    }

    void Update() {

    }

    public void OnEnterNewRoom(string room) {
        LeanTween.delayedCall(gameObject, 1f, SpawnMonster);
    }

    private void SpawnMonster() {
        float minX = -6f;
        float maxX = 6f;
        float minY = -4f;
        float maxY = 4f;
        Vector3 monsterPos = new Vector3(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY), -0.7f);
        Vector3 smokePos = monsterPos - new Vector3(0, 0, 0.1f);

        GameObject monster = Instantiate(PrefabRegistry.Instance.enemyBouncer) as GameObject;
        monster.transform.position = monsterPos;
        GameObject smoke = Instantiate(PrefabRegistry.Instance.smokeSpawner) as GameObject;
        smoke.transform.position = smokePos;

        Find.ComponentOnGameObject<SmokeSpawner>(this, smoke).StartSmoke(0.5f, 10, 20, ()=> Find.ComponentOnGameObject<Enemy>(monster).EnemySpawned());
    }

    public void OnLeaveRoom() {

    }

    public void OnPlayerHitMonster(Vector2 position) {
        Vector3 p = new Vector3(position.x, position.y, hitMonsterParticles.transform.position.z);
        hitMonsterParticles.transform.position = p;
        hitMonsterParticles.Play();
    }
}
