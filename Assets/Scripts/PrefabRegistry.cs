using UnityEngine;
using System.Collections;

public class PrefabRegistry : MonoBehaviour {

    public static PrefabRegistry Instance;

    [System.Serializable]
    public struct Prefab {
        public GameObject prefab;
        public string name;
    }


    //public GameObject hitMonsterParticles;




    void Awake() {
        Instance = this;
    }
    void OnDestroy() {
        Instance = null;
    }


}
