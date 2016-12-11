﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int HP = 3;
    public bool canAttack = true;


    private float lastHitTime = 0f;
    private static float HIT_COOLDOWN = 1f;
    private bool isStarted;

    private void Start() {

    }

    void Update() {

    }

    public void EnemySpawned() {
        isStarted = true;
        SendMessage("EnemyStart", SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (HP <= 0 || !isStarted) {
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

                //Collider2D c = GetComponent<Collider2D>();
                //if(c != null) {
                //    c.isTrigger = true;
                //}
            }
            else {
                SendMessage("OnEnemyHit", SendMessageOptions.DontRequireReceiver);

            }

        }
    }

    public bool GivePlayerDamage() {
        return canAttack && Time.time > lastHitTime + HIT_COOLDOWN && HP > 0 && isStarted;
    }
}
