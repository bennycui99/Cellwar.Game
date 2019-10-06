using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CellWar.Controller;
using CellWar.GameData;

namespace CellWar.View
{
    public class GameInformationLogic : MonoBehaviour
    {
        [SerializeField]
        Text m_PlayerStrainInfo, m_NPCStrainInfo;
        [SerializeField]
        ScrollRect m_GameInfoScroll;
        // Start is called before the first frame update
        void Start()
        {
            m_NPCStrainInfo.text = "";
            m_PlayerStrainInfo.text = "";
            m_GameInfoScroll.content = m_PlayerStrainInfo.rectTransform;
            OnInsPlayerClicked();
        }

        // Update is called only when Gameinformation is invoked.
        public void GetInfo()
        {
            m_PlayerStrainInfo.text = MainGameCurrent.GetGameInfomationByStrainList(MainGameCurrent.StrainList);
            m_NPCStrainInfo.text = MainGameCurrent.GetGameInfomationByStrainList(Local.AllNpcStrains);
        }

        //Two Onclick functions - hide another text, change the render target of scrollrect to the corresponding one.
        public void OnInsPlayerClicked()
        {
            //GetInfo();
            m_NPCStrainInfo.enabled = false;
            m_PlayerStrainInfo.enabled = true;
            m_GameInfoScroll.content = m_PlayerStrainInfo.rectTransform;
        }
        public void OnInsNPCClicked()
        {
            //GetInfo();
            m_NPCStrainInfo.enabled = true;
            m_PlayerStrainInfo.enabled = false;
            m_GameInfoScroll.content = m_PlayerStrainInfo.rectTransform;
        }

    }
}

