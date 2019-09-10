using UnityEngine;
using UnityEngine.UI;
using CellWar.Controller;

namespace CellWar.View
{
    public class GeneralFadeInLogic : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        RawImage m_blackOut;
        // Update is called once per frame

        void Update()
        {
            if (VideoManager.Instance().m_videoPlayer.enabled)
            {
                StartCoroutine(VideoManager.Instance().PlayFadeinVideo());//Play fade in video
                VideoManager.Instance().m_videoPlayer.started += FadeInBegin;
                VideoManager.Instance().m_videoPlayer.loopPointReached += FadeInComplete;
            }
        }
        void FadeInComplete(UnityEngine.Video.VideoPlayer vp)
        {
            //EventHandler
            VideoManager.Instance().DisableImage();
        }
        void FadeInBegin(UnityEngine.Video.VideoPlayer vp)
        {
            //EventHandler
            Destroy(m_blackOut);
        }
    }
}


