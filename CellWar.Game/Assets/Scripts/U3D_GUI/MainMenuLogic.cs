using System;
using System.Collections;
using System.Collections.Generic;
using CellWar.Controller;
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
        bool isReadyToLoad = false;

        private void Awake() {
            Check.GameDataLoaded();
        }
        // Start is called before the first frame update
        void Start() {
            isReadyToLoad = false;
        }

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
            //Debug.Log("Menu/" + states.ToString());
            transform.Find( "Menu/" + states.ToString() ).gameObject.SetActive( true );
        }

        void Switch( MenuStates state ) {
            FlushUI();
            SwitchTo( state );
        }

        #endregion

        #region EVENTS
        public void OnStageClicked() {
            
            Switch(MenuStates.Stage);
        }
        public void OnStartClicked()
        {
            StartCoroutine(VideoManager.Instance().PlayFadeoutVideo());
            VideoManager.Instance().m_videoPlayer.loopPointReached += EndReachedGeneral;
            StartCoroutine(SceneLoadWait("GameScene"));
        }


        public void OnOptionClicked() {
            Switch( MenuStates.Options );
        }
        public void OnLabClicked() {
            Switch( MenuStates.Lab );
        }
        public void OnMapEditorClicked()
        {
            StartCoroutine(VideoManager.Instance().PlayFadeoutVideo());
            VideoManager.Instance().m_videoPlayer.loopPointReached += EndReachedGeneral;
            StartCoroutine(SceneLoadWait("MapEditorScene"));

        }
        public void OnExitClicked() {
            Application.Quit();
        }
        public void OnBackClicked() {
            Switch( MenuStates.MainMenu );
        }
        private IEnumerator SceneLoadWait(string SceneName)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(2);
            //Mystery, if the wait is 1 second, it wont work, but with 2 second it works perfectly - only on my machine, I need a check on lower performance machines. - Suomi.
            while (!isReadyToLoad)
            {
                yield return waitForSeconds;
                break;
                //While the video is not finished, we continue to wait.
            }
            SceneManager.LoadScene(SceneName);
            //When we are ready, load the target scene.
        }
        #endregion
        void EndReachedStageScene(UnityEngine.Video.VideoPlayer vp)
        {
            
         SceneManager.LoadScene("GameScene");
            //Event Handler for VideoManager.
            //Obselete
        }

        void EndReachedEditorScene(UnityEngine.Video.VideoPlayer vp)
        {
           
          SceneManager.LoadScene("MapEditorScene");
            //Event Handler for VideoManager.
            //Obselete
        }
        void EndReachedGeneral(UnityEngine.Video.VideoPlayer vp)
        {
            Debug.Log("Video Played");
            isReadyToLoad = true;
            //General Event handler. May still have some bugs because now it's run on 2 coroutines.
        }
    }
}
