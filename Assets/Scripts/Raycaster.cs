using UnityEngine;
using System.Collections;

public class Raycaster : MonoBehaviour {

    private static Vector2[] directions = new Vector2[] { Vector2.left, Vector2.up, Vector2.right, Vector2.down };

    private Vector2 rayDirection = new Vector2(1, 0);
    public float maxDistance = 4f;

    private int layerMask;
    private RaycastHit2D hit;

    private void Awake() {
        SetLayer("World");

        int r = Mathf.RoundToInt(transform.localRotation.eulerAngles.z / 360 * 4);
        Debug.Log("Rotation = " + r);

        Vector3 dir = transform.localRotation * Vector3.right;
        rayDirection = new Vector2(dir.x, dir.y);
    }

    public void SetLayer(string name) {
        layerMask = 1 << LayerMask.NameToLayer("World");
    }

    public bool CheckCollision() {
        hit = Physics2D.Raycast(transform.position, rayDirection, maxDistance, layerMask);
        //hit.normal
        Debug.DrawRay(transform.position, rayDirection, (hit.collider == null) ? Color.blue : Color.red, 0.016f);
        return hit.collider != null;
    }

    public Vector2 MoveOutVector() {
        Vector2 rayToHit = hit.point;
        return rayToHit;

    }

    public float GetHitDist() {
        return hit.distance;
    }


}
