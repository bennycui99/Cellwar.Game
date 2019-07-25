using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Mock;
using CellWar.View;
using UnityEngine;
using UnityEngine.UI;

public class U3D_SceneLoad : MonoBehaviour {
    private void Awake() {
        try {
            CellWar.GameData.Local.LoadAllCodingGenes();
            CellWar.GameData.Local.LoadAllChemicals();
            CellWar.GameData.Local.LoadAllRegulartoryGenes();
        } catch {
            Debug.LogError( "Local json load failed." );
            throw;
        }

        #region MOCKS
        Mocks.MockStrainList.Add( Mocks.Strain2 );
        Mocks.MockStrainList.Add( Mocks.Strain3 );
        Mocks.Strain1.PlayerSelectedGenes.AddRange( Local.AllCodingGenes );
        Mocks.Strain1.ConditionGene = ( Local.AllRegulartoryGenes[0] );
        Mocks.MockStrainList.Add( Mocks.Strain1 );
        Current.StrainList = Mocks.MockStrainList;
        #endregion

        InitStrainList();
    }


    public void InitStrainList() {
        var strainUIList = GameObject.Find( "UI_StrainList" );
        var strainUI = GameObject.Find( "UI_Strain" );
        foreach( var strain in Current.StrainList ) {
            var newStrainUI = Instantiate( strainUI );
            newStrainUI.name = strain.Name;
            newStrainUI.GetComponent<U3D_StrainPackageLogic>().Strain = strain;
            newStrainUI.transform.parent = strainUIList.transform;
        }
        strainUI.SetActive( false );
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
