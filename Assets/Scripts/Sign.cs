using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sign : MonoBehaviour {

    private bool isReading;
    private int index;
    private GameObject canvas;
    private string text;
    private float countDown = 0f;

    private Text sign;

    void Start() {
        canvas = Find.ChildByName(this, "Canvas");
        sign = Find.ComponentOnChild<Text>(this, "Canvas/Text");
        sign.text = "";
        canvas.SetActive(false);
    }

    void Update() {
        if (isReading) {
            countDown -= Time.deltaTime;
            if (countDown <= 0f) {
                sign.text = text.Substring(0, index);
                if (index++ == text.Length) {
                    isReading = false;
                }
                countDown = 0.05f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Player")) {
            isReading = true;
            index = 0;
            sign.text = "";
            canvas.SetActive(true);
            text = Game.Instace.GetTextForSign();
            countDown = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Player")) {
            isReading = false;
            sign.text = "";
            canvas.SetActive(false);
        }
    }
}
