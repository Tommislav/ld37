using UnityEngine;
using System.Collections;

public class BuildFloor : MonoBehaviour {

	public GameObject defaultFloorTile;
	public GameObject defaultWallTile;
	public int numW;
	public int numH;

	public float ppu = 100f;

	void Awake () {
		float unit = 32 / ppu;

		float halfUnit = unit / 2f;

		float left = -numW * unit / 2f;
		float up = numH * unit / 2f;


		for (int y=0; y< numH; y++) {
			for (int x=0; x< numW; x++) {
				bool wall = y == 0 || x == 0 || y == numH - 1 || x == numW - 1;

				GameObject go = Instantiate(wall ? defaultWallTile : defaultFloorTile) as GameObject;
				go.transform.parent = transform;
				go.transform.localPosition = new Vector3(left + x * unit, up - y * unit);
			}
		}
		
		Destroy(this);
	
	}

}
