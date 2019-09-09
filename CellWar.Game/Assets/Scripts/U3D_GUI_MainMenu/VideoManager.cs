using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace CellWar.View
{
    public class VideoManager : MonoBehaviour
    {
        private static VideoManager m_Instance = null;
        public GameObject videoCanvasPrefab;
        public VideoPlayer m_videoPlayer;
        private GameObject m_imageCanvas;
        private RawImage m_imageLayer;
        #region VIDEOS
        public VideoClip m_FadeinVideo;
        public VideoClip m_FadeoutVideo;
        #endregion
        void Awake()
        {
            SetupVideoMngSingleton();
        }
        void Update()
        {
            if (!m_imageCanvas)
            {
                //Initialize a image Canvas
                m_imageCanvas =  Instantiate(videoCanvasPrefab);
                m_imageLayer = m_imageCanvas.GetComponentInChildren<RawImage>();
                m_videoPlayer = m_imageCanvas.GetComponentInChildren<VideoPlayer>();
                m_imageCanvas.GetComponent<Canvas>().sortingOrder = 1919;//Ensure Overlay
                m_imageLayer.enabled = false;//Disable image first
            }
        }
        void SetupVideoMngSingleton()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
                DontDestroyOnLoad(gameObject);//This will keep video manager eternally exist in any scene
            }
            else if (m_Instance != null)
            {
                Destroy(gameObject);
            }
        }
        public static VideoManager Instance()
        {
            return m_Instance;
        }
        #region VIDEO_IENUMERATORS
        //Anytime you want play some video, first enable the image layer, then use Startcoroutine(VideoManager.Instance.(Some IEnumerator))
        
        public IEnumerator PlayFadeinVideo()
        {
            m_videoPlayer.clip = m_FadeinVideo;
            m_videoPlayer.Prepare();
            WaitForSeconds waitForSeconds = new WaitForSeconds(1);
            while (!m_videoPlayer.isPrepared)
            {
                yield return waitForSeconds;
                break;
            }
            m_imageLayer.texture = m_videoPlayer.texture;
            m_imageLayer.enabled = true;
            m_videoPlayer.Play();
        }
        public IEnumerator PlayFadeoutVideo()
        {
            m_videoPlayer.clip = m_FadeoutVideo;
            m_videoPlayer.Prepare();
            WaitForSeconds waitForSeconds = new WaitForSeconds(1);
            while (!m_videoPlayer.isPrepared)
            {
                yield return waitForSeconds;
                break;
            }
            m_imageLayer.texture = m_videoPlayer.texture;
            m_imageLayer.enabled = true;
            m_videoPlayer.Play();
        }
        public void DisableImage()
        {
            m_imageLayer.enabled = false;
            m_videoPlayer.enabled = false;
            //Upon videoplay complete - disable the image layer.
            //Maybe only needs to be used after fade in.
            
        }
        #endregion
    }
}

