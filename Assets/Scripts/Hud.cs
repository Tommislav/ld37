using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour {

	public static Hud Instance;

	private GameObject[] health;

	void Awake() {
		Instance = this;

		health = new GameObject[] {
			Find.ChildByName(this, "Life1"),
			Find.ChildByName(this, "Life2"),
			Find.ChildByName(this, "Life3"),
			Find.ChildByName(this, "Life4"),
			Find.ChildByName(this, "Life5")
		};
	}

	void OnDestroy() {
		Instance = null;
	}
	
	public void SetHealth(int n) {
		for (int i=0; i<5; i++) {
			health[i].SetActive(i < n);
		}
	}
}
