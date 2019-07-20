using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U3D_SceneLoad : MonoBehaviour {
    private void Awake() {
        CellWar.GameData.Local.LoadAllCodingGenes();
        CellWar.GameData.Local.LoadAllChemicals();
        Debug.Log( CellWar.GameData.Local.AllCodingGenes );
        Debug.Log( CellWar.GameData.Local.AllChemicals );
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
