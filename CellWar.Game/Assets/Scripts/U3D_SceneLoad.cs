using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U3D_SceneLoad : MonoBehaviour {
    private void Awake() {
        try {
            CellWar.GameData.Local.LoadAllCodingGenes();
            CellWar.GameData.Local.LoadAllChemicals();
            CellWar.GameData.Local.LoadAllRegulartoryGenes();
            Debug.Log( CellWar.GameData.Local.AllCodingGenes );
            Debug.Log( CellWar.GameData.Local.AllChemicals );
        } catch {
            Debug.LogError("Local json load failed.");
            throw;
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
