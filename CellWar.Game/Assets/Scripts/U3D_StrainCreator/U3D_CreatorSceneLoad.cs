using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U3D_CreatorSceneLoad : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        try {
            CellWar.GameData.Local.LoadAllCodingGenes();
            CellWar.GameData.Local.LoadAllChemicals();
            CellWar.GameData.Local.LoadAllRegulartoryGenes();
            CellWar.GameData.Local.
        } catch {
            Debug.LogError( "Local json load failed." );
            throw;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
