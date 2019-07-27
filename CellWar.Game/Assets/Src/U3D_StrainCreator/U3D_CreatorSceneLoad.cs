using System;
using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Utils;
using CellWar.View;
using UnityEngine;
using static CellWar.Model.Substance.Strain;

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

        UIHelper.InitUIList( "UI_RaceList", "UI_Ele_Race", Local.AllRaces,
            ( GameObject g, Race obj ) => {
                g.GetComponent<U3D_RaceElement>().Race = obj;
                g.name = obj.Name;
            } );

        UIHelper.InitUIList( "UI_CodList", "UI_Ele_Cod", Local.AllCodingGenes,
            ( GameObject g, CodingGene obj ) => {
                g.GetComponent<U3D_CodElement>().CodingGene = obj;
                g.name = obj.Name;
            } );

        UIHelper.InitUIList( "UI_RegList", "UI_Ele_Reg", Local.AllRegulartoryGenes,
            ( GameObject g, RegulatoryGene obj ) => {
                g.GetComponent<U3D_RegElement>().RegulatoryGene = obj;
                g.name = obj.Name;
            } );
    }

    // Update is called once per frame
    void Update() {

    }
}
