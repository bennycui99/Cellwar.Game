using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeinLogic : MonoBehaviour
{
    [SerializeField]
    CanvasGroup m_canvasGroup;
    void Awake()
    {
        m_canvasGroup.alpha = 0;
        //On entering the game, hide the UI elements.
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(IncreaseAlpha());
        }
    }
    void OnTriggerEnter()
    {
        StartCoroutine(IncreaseAlpha());
    }
    IEnumerator IncreaseAlpha()
    {
        while(m_canvasGroup.alpha < 1)
        {
            m_canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
        enabled = false;//disable itself
        yield return null;
    }
}
