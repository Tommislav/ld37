using UnityEngine;
using System.Collections;

public class Physics : MonoBehaviour {


    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;


    private Raycaster downMid;
    void Start() {
        downMid = Find.ComponentOnChild<Raycaster>(this, "DownMid");
    }

    void Update() {

       Debug.Log("hitGround: " + downMid.CheckCollision());
    }
}
