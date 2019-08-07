using System;
using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CellWar.View {
    class MainMenuLogic : MonoBehaviour {
        enum MenuStates {
            MainMenu,
            Stage,
            Lab,
            Options,
            Exit
        }
        private void Awake() {
            Check.GameDataLoaded();
        }
        // Start is called before the first frame update
        void Start() { }

        #region STATE_SWITCH

        /// <summary>
        /// 将menu下的所有栏目隐藏
        /// </summary>
        private void FlushUI() {
            foreach( Transform menu in transform.Find( "Menu" ) ) {
                menu.gameObject.SetActive( false );
            }
        }

        private void SwitchTo( MenuStates states ) {
            transform.Find( "Menu/" + states.ToString() ).gameObject.SetActive( true );
        }

        void Switch( MenuStates state ) {
            FlushUI();
            SwitchTo( state );
        }

        #endregion

        #region EVENTS
        public void OnStageClicked() {
            SceneManager.LoadScene( "GameScene" );
        }
        public void OnOptionClicked() {
            Switch( MenuStates.Options );
        }
        public void OnLabClicked() {
            Switch( MenuStates.Lab );
        }
        public void OnExitClicked() {
            Application.Quit();
        }
        public void OnBackClicked() {
            Switch( MenuStates.MainMenu );
        }
        #endregion
    }
}
