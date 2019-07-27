using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using UnityEngine;

public class U3D_CreatorSceneLoad : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        try {
            CellWar.GameData.Local.LoadAllCodingGenes();
            CellWar.GameData.Local.LoadAllChemicals();
            CellWar.GameData.Local.LoadAllRegulartoryGenes();
            CellWar.GameData.Local.LoadAllRaces();
        } catch {
            Debug.LogError( "Local json load failed." );
            throw;
        }

        InitRaceList();
    }

    public void InitRaceList() {
        var raceUIList = GameObject.Find( "UI_RaceList" );
        var raceUI = GameObject.Find( "UI_Ele_Race" );
        foreach( var race in Local.AllRaces ) {
            var newRaceUI = Instantiate( raceUI );
            newRaceUI.name = race.Name;
            newRaceUI.GetComponent<U3D_RaceElement>().Race = race;
            newRaceUI.transform.parent = raceUIList.transform;
        }
        raceUI.SetActive( false );
    }

    // Update is called once per frame
    void Update() {

    }
}
