using UnityEngine.SceneManagement;
using System;
using UnityEngine;
using UnityEngine.UI;
using CellWar.Controller;

namespace CellWar.View {
    public class GameOverLogic : MonoBehaviour {
        [SerializeField]
        Text m_GameOverText, m_Time, m_Population;
        private static GameOverLogic _instance;
        public static GameOverLogic Instance { get { return _instance; } }

        private void Awake()
        {

            //GetComponent<Canvas>().enabled = false;
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void SetGameOverCondition( float TimeinSecond, int Population, bool Win ) {
            m_GameOverText.text = Win ? "You Win!" : "You Lose!";

            if( TimeinSecond <= TimeSpan.MaxValue.TotalSeconds ) {
                TimeSpan time = TimeSpan.FromSeconds(TimeinSecond);
                m_Time.text = "Time: " + time.ToString( @"hh\:mm\:ss\:fff" );
            }
            else {
                m_Time.text = "";
            }
            m_Population.text = "Population: " + Population.ToString();
            GetComponent<Canvas>().enabled = true;
        }

        public void OnRestartClicked() {
            StartCoroutine( VideoManager.Instance().PlayFadeoutVideo() );
            VideoManager.Instance().m_videoPlayer.loopPointReached += EndReachedRestart;
        }

        public void OnQuitClicked() {
            StartCoroutine( VideoManager.Instance().PlayFadeoutVideo() );
            VideoManager.Instance().m_videoPlayer.loopPointReached += EndReachedQuit;
        }

        void EndReachedRestart( UnityEngine.Video.VideoPlayer vp ) {

            SceneManager.LoadScene( "GameScene" );
            //Event Handler for VideoManager.
        }

        void EndReachedQuit( UnityEngine.Video.VideoPlayer vp ) {

            SceneManager.LoadScene( "MainMenuScene" );
            //Event Handler for VideoManager.
        }
    }

}

