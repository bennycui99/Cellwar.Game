using System;
using System.Collections;
using CellWar.GameData;
using CellWar.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CellWar.View {
    /// <summary>
    /// Bind with: ApplicationStartScene - LoadGameData
    /// </summary>
    public class U3D_LoadGameData : MonoBehaviour {

        private GameObject uiTextInfo;
        private int fakeTimeWaitSecond = 0;
        private bool isLoaded = false;

        void Info( string textInfo ) {
            UIHelper.ChangeText( uiTextInfo, textInfo );
        }
        private void Awake() {
            Application.targetFrameRate = 120;
        }
        private void Start() {
            StartCoroutine( LoadGameData() );
        }
        IEnumerator LoadGameData() {
            if( !isLoaded ) {
                uiTextInfo = GameObject.Find( "UI_InfoText" );
                Exception exception = null;
                try {
                    Local.LoadAllCodingGenes();
                    Local.LoadAllChemicals();
                    Local.LoadAllRegulartoryGenes();
                    Local.LoadAllRaces();
                    Local.LoadAllNpcs();
                    Local.LoadAllNpcStrains();
                    Info( "Static Game Data loaded successfully..." );

                } catch( Exception ex ) {
                    exception = ex;
                    Info( "Error when loading local data. Detail: " + ex.Message );
                }

                yield return new WaitForSeconds( fakeTimeWaitSecond );
                try {
                    Save.Strains = Save.LoadStrainsWithFilePath("Save/Strains.json");

                    Info( "User Saved Game Data loaded successfully..." );

                } catch( Exception ex ) {
                    exception = ex;
                    Info( "Error when loading save data. Detail: " + ex.Message );
                }
                yield return new WaitForSeconds( fakeTimeWaitSecond );

                if( exception == null ) {
                    Info( "All Game Data loaded successfully." );

                    yield return new WaitForSeconds( fakeTimeWaitSecond / 2 );

                    Info( "Entering Cell World..." );

                    yield return new WaitForSeconds( fakeTimeWaitSecond * 2 );

                    SceneManager.LoadScene( "MainMenuScene" );

                    isLoaded = true;
                }
            }
        }

    }
}
