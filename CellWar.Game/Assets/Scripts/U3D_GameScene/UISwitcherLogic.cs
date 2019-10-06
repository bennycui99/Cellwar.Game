using CellWar.GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellWar.View
{
    public class UISwitcherLogic : MonoBehaviour
    {
        [SerializeField]
        GameObject UI_Lab,UI_GameInformation;
        GameObject SwitchTarget;
        private bool isPaused,isSwitching,isGameStarted;
        // Start is called before the first frame update
        void Awake()
        {
            UI_GameInformation.SetActive(false);
            SwitchTarget = UI_Lab;
            SwitchTarget.SetActive( true );
            SwitchTarget.SetActive( false );
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isSwitching = true;
            }
        }
        void FixedUpdate()
        {
            isGameStarted = CellWar.Controller.GameManager.Instance.IsGameStarted;//Crutial, keep this up to date
            if (isGameStarted)
            {
                SwitchTarget = UI_GameInformation;
            }
            if (isSwitching)
            {
                SwitchUI();
                isSwitching = false;
            }
        }
        void SwitchUI()
        {
            if (CellWar.Controller.GameManager.Instance.IsGameCompleted)//Do nothing if we are already finished
            {
                return;
            }
            if (SwitchTarget.activeInHierarchy)//if the target is enabled, we disable it 
            {
                if (isGameStarted)
                {
                    CellWar.Controller.GameManager.Instance.IsPaused = false;//if the game is started, this means we are switching off GameInformation, so we need to unpause the game
                }
                SwitchTarget.SetActive(false);
                
            }
            else //Otherwise enable it. If in the enable process the game is running, we pause it.
            {
                if (isGameStarted)
                {
                    CellWar.Controller.GameManager.Instance.IsPaused = true;//if the game is started, we pause it and switch out the info interface.
                }
                // else do nothing
                SwitchTarget.SetActive(true);
            }
        }
    }
}

