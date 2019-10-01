using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using CellWar.GameData;

namespace CellWar.Controller
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance { get { return _instance; } }

        private EventSystem m_EventSystem;
        private GraphicRaycaster m_GraphicRaycaster;

        private GameObject m_GameOverFrameObject;
        private GameObject m_IntroductionFrameObject;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            m_GraphicRaycaster = GameObject.Find("BlockingCanvas").GetComponent<GraphicRaycaster>();

            m_GameOverFrameObject = GameObject.Find("GameOverFrame");
            m_IntroductionFrameObject = GameObject.Find("IntroductionFrame");
        }

        /// <summary>
        /// 检测UI穿透
        /// </summary>
        /// <returns></returns>
        public bool CheckGuiRaycastObjects()
        {
            // 找不到就让UI穿透
            if (m_EventSystem == null || m_GraphicRaycaster == null)
            {
                return false;
            }

            PointerEventData eventData = new PointerEventData(m_EventSystem)
            {
                pressPosition = Input.mousePosition,
                position = Input.mousePosition
            };

            List<RaycastResult> list = new List<RaycastResult>();
            m_GraphicRaycaster.Raycast(eventData, list);
            //Debug.Log(list.Count);
            return list.Count > 0;
        }

        public void UIDisplayGameOver()
        {
            m_GameOverFrameObject.SetActive(true);
            // Set Text here
        }

        public void OnGameSceneExitClick()
        {
            MainGameCurrent.FocusedHexBlock = null;

            SceneManager.LoadScene("MainMenuScene");
        }

        public void OnEditorSceneExitClick()
        {
            MainGameCurrent.FocusedHexBlock = null;

            SceneManager.LoadScene("MainMenuScene");
        }

        public void OnIntroductionContinueClick()
        {
            m_IntroductionFrameObject.SetActive(false);
        }
    }

}
