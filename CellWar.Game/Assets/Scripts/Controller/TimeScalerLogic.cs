using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CellWar.Controller {
    public class TimeScalerLogic : MonoBehaviour
    {
        [SerializeField]
        float m_TimeScale=1.0f;
        [SerializeField]
        Button m_Slower, m_Faster, m_Pause, m_Fastest;
        Slider m_TimeScaler;
        float m_ScaleMin,m_ScaleMax;
        // Start is called before the first frame update
        void Start()
        {
            m_TimeScaler = GetComponent<Slider>();
            m_TimeScaler.value = 1.0f;
            m_ScaleMax = m_TimeScaler.maxValue;
            m_ScaleMin = m_TimeScaler.minValue;
        }
        public void OnSlowerClicked()
        {
            
            if (m_TimeScale > 1.0f)
            {
                m_TimeScale -= 1.0f;
            }
            else
            {
                m_TimeScale -= 0.1f;
            }
            if(m_TimeScale < m_ScaleMin)
            {
                m_TimeScale = m_ScaleMin;
            }
            if (m_TimeScale != 0)
            {
                GameManager.Instance.MaxUpdateCount = 1 / m_TimeScale;
            }
            else
            {
                // Timescale == 0 -> pause
                GameManager.Instance.IsPaused = true;
            }
            m_TimeScaler.value = m_TimeScale;
        }
        public void OnFasterClicked()
        {
            if (m_TimeScale >= 1.0f)
            {
                m_TimeScale += 1.0f;
            }else
            {
                m_TimeScale += 0.1f;
            }
            if(m_TimeScale != 0)
            {
                GameManager.Instance.MaxUpdateCount = 1 / m_TimeScale;
            }
            if (m_TimeScale > m_ScaleMax)
            {
                m_TimeScale = m_ScaleMax;
            }
            m_TimeScaler.value = m_TimeScale;
            GameManager.Instance.IsPaused = false;
        }
        public void OnPauseClicked()
        {
            m_TimeScaler.value = 0;
            GameManager.Instance.IsPaused = true;
        }
        public void OnFastestClicked()
        {
            m_TimeScale = m_ScaleMax;
            m_TimeScaler.value = m_TimeScale;
            GameManager.Instance.MaxUpdateCount = 1 / m_TimeScale;
            GameManager.Instance.IsPaused = false;

        }
    }
}

