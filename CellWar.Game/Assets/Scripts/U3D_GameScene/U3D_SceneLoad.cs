﻿using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Test.Mock;
using CellWar.Model.Substance;
using CellWar.Utils;
using CellWar.View;
using UnityEngine;
using UnityEngine.UI;

namespace CellWar.View {
    public class U3D_SceneLoad : MonoBehaviour {
        private void Awake() {
            try {
                Local.LoadAllCodingGenes();
                Local.LoadAllChemicals();
                Local.LoadAllRegulartoryGenes();
                Local.LoadAllRaces();
            } catch {
                Debug.LogError( "Local json load failed." );
                throw;
            }

            //#region MOCKS
            //Mocks.MockStrainList.Add( Mocks.Strain2 );
            //Mocks.MockStrainList.Add( Mocks.Strain3 );
            //Mocks.Strain1.PlayerSelectedGenes.AddRange( Local.AllRegulartoryGenes );
            //Mocks.MockStrainList.Add( Mocks.Strain1 );
            //MainGameCurrent.StrainList = Mocks.MockStrainList;
            //#endregion

            MainGameCurrent.StrainList = Save.Strains;

            UIHelper.InitUIList<Strain>( "UI_StrainList", "UI_Strain", MainGameCurrent.StrainList,
                ( GameObject g, Strain obj ) => {
                    g.GetComponent<U3D_StrainPackageLogic>().Strain = obj;
                    g.name = obj.Name;
                } );
        }

        /*
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
        */
    }
}