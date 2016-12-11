﻿using UnityEngine;
using System.Collections;

public class PrefabRegistry : MonoBehaviour {

    public static PrefabRegistry Instance;

    [System.Serializable]
    public struct Prefab {
        public GameObject prefab;
        public string name;
    }


    //public GameObject hitMonsterParticlesc;
    public GameObject smoke;
    public GameObject smokeSpawner;

    public GameObject enemyBouncer;
	public GameObject enemyBouncerSmall;




    void Awake() {
        Instance = this;
    }
    void OnDestroy() {
        Instance = null;
    }


}
