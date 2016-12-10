using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    private Rigidbody2D rb;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {

        bool moveLeft = Input.GetKey(KeyCode.LeftArrow);
        bool moveRight = Input.GetKey(KeyCode.RightArrow);
        bool move = moveLeft || moveRight;
        bool jump = Input.GetKey(KeyCode.UpArrow);
        bool atk1 = Input.GetKey(KeyCode.Z);
        bool atk2 = Input.GetKey(KeyCode.X);

        if(moveLeft) {
            rb.AddForce(new Vector2(-100, 0), ForceMode2D.Force);
        }
        if(moveRight) {
            rb.AddForce(new Vector2(100, 0), ForceMode2D.Force);
        }

        rb.drag = move ? 1 : 5;


        if(jump) {
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }

        if(atk1) {

        }
        if(atk2) {

        }
    }


    private void OnCollisionEnter2D(Collision2D coll) {
        Debug.Log("ON THE GROUND");
    }
}