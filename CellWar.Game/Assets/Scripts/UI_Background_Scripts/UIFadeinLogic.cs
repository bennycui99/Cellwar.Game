using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeinLogic : MonoBehaviour
{
    
    public CanvasGroup m_canvasGroup;
    void Awake()
    {
        m_canvasGroup.alpha = 0;
        //On entering the game, hide the UI elements.
    }
    void OnTriggerEnter()
    {
        Debug.Log("RP1 collider hit");
        StartCoroutine(IncreaseAlpha());
    }
    IEnumerator IncreaseAlpha()
    {
        while(m_canvasGroup.alpha < 1)
        {
            m_canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
