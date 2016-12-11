using UnityEngine;
using System.Collections;
using System;

public class PlayerControls : MonoBehaviour {

    public static PlayerControls Instance;

    private Rigidbody2D rb;
    private float moveForce = 200f;

    private Transform animationTransform;
    private Animator animator;

    private GameObject[] swords;
    private bool isAttacking;

    private int moveLastMove;

    private float freezeTime = 0;
    private bool isBlinking;

    void Awake() {
        Instance = this;
        rb = Find.ComponentOnGameObject<Rigidbody2D>(this);
        animationTransform = Find.ComponentOnChild<Transform>(this, "PlayerGfx");
        animator = Find.ComponentOnGameObject<Animator>(this);
        swords = new GameObject[] { Find.ChildByName(this, "SwordU"), Find.ChildByName(this, "SwordD"), Find.ChildByName(this, "SwordL"), Find.ChildByName(this, "SwordR") };
        HideAndGet(-1);
    }
    void OnDestroy() {
        Instance = null;
    }
    public void OnRoomTransitionStarted() {
        rb.velocity = Vector3.zero;
    }
    public void OnRoomTranisiontCompleted() {
    }

    void Update() {

        if (isBlinking) {
            animationTransform.gameObject.SetActive(!animationTransform.gameObject.activeSelf);
        }

        if(Game.isRoomTransition || Time.time < freezeTime) {
            return;
        }

        if (isBlinking) {
            isBlinking = false;
            animationTransform.gameObject.SetActive(true);
        }

        bool moveLeft = Input.GetKey(KeyCode.LeftArrow);
        bool moveRight = Input.GetKey(KeyCode.RightArrow);
        bool moveUp = Input.GetKey(KeyCode.UpArrow);
        bool moveDown = Input.GetKey(KeyCode.DownArrow);
        bool move = moveLeft || moveRight || moveUp || moveDown;

        if(moveLeft) {
            rb.AddForce(new Vector2(-moveForce, 0), ForceMode2D.Force);
        }
        if(moveRight) {
            rb.AddForce(new Vector2(moveForce, 0), ForceMode2D.Force);
        }
        if(moveUp) {
            rb.AddForce(new Vector2(0, moveForce));
        }
        if(moveDown) {
            rb.AddForce(new Vector2(0, -moveForce));
        }

        rb.drag = move ? 5 : 15;

        UpdateAnimations(moveUp, moveDown, moveLeft, moveRight);

        int atk = -1;
        atk = Input.GetKey(KeyCode.W) ? 0 : atk;
        atk = Input.GetKey(KeyCode.S) ? 1 : atk;
        atk = Input.GetKey(KeyCode.A) ? 2 : atk;
        atk = Input.GetKey(KeyCode.D) ? 3 : atk;

        if(atk != -1 && !isAttacking) {
            LeanTween.cancel(gameObject);
            isAttacking = true;
            GameObject sword = HideAndGet(atk);
            sword.SetActive(true);
            LeanTween.delayedCall(gameObject, 0.18f, () => sword.SetActive(false));
        }
        if(atk == -1) {
            isAttacking = false;
        }
    }

    private void UpdateAnimations(bool moveUp, bool moveDown, bool moveLeft, bool moveRight) {
        int U = 1;
        int D = 2;
        int L = 3;
        int R = 4;
        if(moveLeft) {
            if (moveLastMove != L) {
                animationTransform.localScale = new Vector3(-1, 1);
                animator.Play("Player_Walk_Right");
                moveLastMove = L;
            }
        }
        else if(moveRight) {
            if (moveLastMove != R) {
                animationTransform.localScale = new Vector3(1, 1);
                animator.Play("Player_Walk_Right");
                moveLastMove = R;
            }
        }
        else if(moveUp) {
            if (moveLastMove != U) {
                animationTransform.localScale = new Vector3(1, 1);
                animator.Play("Player_Walk_Up");
                moveLastMove = U;
            }
        }
        else if(moveDown) {
            if (moveLastMove != D) {
                animationTransform.localScale = new Vector3(1, 1);
                animator.Play("Player_Walk_Down");
                moveLastMove = D;
            }
        }
        else if (!moveLeft && !moveRight && !moveUp && !moveDown) {
            if (moveLastMove != 0) {
                string[] states = new string[] { "", "Player_Idle_Up", "Player_Idle", "Player_Idle_Right", "Player_Idle_Right" };
                animator.Play(states[moveLastMove]);
                moveLastMove = 0;
            }
        }
    }

    private GameObject HideAndGet(int dir) {
        for(int i = 0; i < swords.Length; i++) {
            swords[i].SetActive(false);
        }
        if(dir >= 0 && dir < swords.Length) {
            return swords[dir];
        }
        return null;
    }

    private void OnCollisionEnter2D(Collision2D coll) {

        if (Time.time < freezeTime) {
            return;
        }

        if (coll.gameObject.CompareTag("Enemy")) {

            Enemy enemy = Find.ComponentOnGameObject<Enemy>(this, coll.gameObject);
            if (!enemy.GivePlayerDamage()) {
                Debug.Log("Cannot give player damage!");
                return;
            }

            Vector2 e = coll.transform.position;
            Vector2 p = transform.position;
            Vector2 delta = (p - e).normalized * (moveForce * 1f);

            rb.AddForce(delta, ForceMode2D.Impulse);
            freezeTime = Time.time + 1f;
            isBlinking = true;

        }
    }
}