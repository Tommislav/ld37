using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int HP = 3;

    private float lastHitTime = 0f;
    private static float HIT_COOLDOWN = 0.7f;

    private void Start() {

    }

    void Update() {

    }


    private void OnTriggerEnter2D(Collider2D coll) {
        if (HP <= 0) {
            return;
        }
        if (coll.CompareTag("PlayerAtk")) {

            if (Time.time < lastHitTime + HIT_COOLDOWN) {
                return;
            }
            lastHitTime = Time.time;

            Vector2 p = transform.position;
            Vector2 e = coll.transform.position;
            Vector2 mid = (e - p) * 0.5f;
            Game.Instace.OnPlayerHitMonster(p + mid);


            HP--;
            if (HP <= 0) {
                SendMessage("OnEnemyDie", SendMessageOptions.DontRequireReceiver);
            }
            else {
                SendMessage("OnEnemyHit", SendMessageOptions.DontRequireReceiver);
            }

        }
    }
}
