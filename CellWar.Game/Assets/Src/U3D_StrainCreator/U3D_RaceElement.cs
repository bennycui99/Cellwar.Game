using System.Collections;
using System.Collections.Generic;
using CellWar.Contoller;
using CellWar.GameData;
using CellWar.Utils;
using UnityEngine;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;

public class U3D_RaceElement : MonoBehaviour {
    public Race Race { get; set; }

    private void Start() {
        GetComponent<Toggle>().isOn = false;
        UIHelper.ChangeText( transform.Find( "Label" ).gameObject, Race.Name );
    }

    public void ChangeText() {
        StrainCreatorCurrent.NewStrain.BasicRace = this.Race;
        U3D_CreatorSceneLoad.FreshLength();
    }
}
