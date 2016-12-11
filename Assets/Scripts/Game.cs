using UnityEngine;
using System.Collections;

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


    public void OnPlayerHitMonster(Vector2 position) {
        Vector3 p = new Vector3(position.x, position.y, hitMonsterParticles.transform.position.z);
        hitMonsterParticles.transform.position = p;
        hitMonsterParticles.Play();
    }
}
