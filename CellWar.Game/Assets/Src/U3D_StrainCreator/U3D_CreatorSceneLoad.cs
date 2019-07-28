using System;
using System.Collections;
using System.Collections.Generic;
using CellWar.Contoller;
using CellWar.GameData;
using CellWar.Utils;
using CellWar.View;
using UnityEngine;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;
using CellWar.Utils.Object;
public class U3D_CreatorSceneLoad : MonoBehaviour {

    // Start is called before the first frame update
    void Awake() {
        try {
            CellWar.GameData.Local.LoadAllCodingGenes();
            CellWar.GameData.Local.LoadAllChemicals();
            CellWar.GameData.Local.LoadAllRegulartoryGenes();
            CellWar.GameData.Local.LoadAllRaces();
        } catch {
            Debug.LogError( "Local json load failed." );
            throw;
        }
        #region INIT_UIs
        UIHelper.InitUIList( "UI_RaceList", "UI_Ele_Race", Local.AllRaces,
            ( GameObject g, Race obj ) => {
                g.GetComponent<U3D_RaceElement>().Race = obj;
                g.name = obj.Name;
            } );

        UIHelper.InitUIList( "UI_CodList", "UI_Ele_Cod", Local.AllCodingGenes,
            ( GameObject g, CodingGene obj ) => {
                g.GetComponent<U3D_CodElement>().CodingGene = ObjectHelper.Clone( obj );
                g.name = obj.Name;
            } );

        UIHelper.InitUIList( "UI_RegList", "UI_Ele_Reg", Local.AllRegulartoryGenes,
            ( GameObject g, RegulatoryGene obj ) => {
                g.GetComponent<U3D_RegElement>().RegulatoryGene = ObjectHelper.Clone( obj, obj.GetType() );
                g.name = obj.Name;
            } );
        #endregion

        StrainCreatorCurrent.NewStrain = new CellWar.Model.Substance.Strain();
    }
    public static void FreshLength() {
        StrainContoller contoller = new StrainContoller();
        if( StrainCreatorCurrent.NewStrain == null ) {
            return;
        }

        int maxLength = StrainCreatorCurrent.NewStrain.BasicRace.MaxLength;
        int currentLength = contoller.GetTotalLength( StrainCreatorCurrent.NewStrain?.PlayerSelectedGenes );

        string alert = maxLength < currentLength ? "Overflowed!!!!!" : "";
        
        StrainCreatorCurrent.IsLengthOverflowed = alert == "" ? false : true;

        GameObject.Find( "UI_MaxLength" ).GetComponent<Text>().text =
            "Max Length: " + maxLength.ToString() + "/" + currentLength.ToString()
            + "\n" + alert;
    }

    public static void EmitAlert( string message ) {
        GameObject.Find( "Canvas" ).transform.Find( "UI_AlertPanel" ).gameObject.SetActive( true );
        UIHelper.ChangeText( GameObject.Find( "UI_AlertMsg" ), message );
    }
}
