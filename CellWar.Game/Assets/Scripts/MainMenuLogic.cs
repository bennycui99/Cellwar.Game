using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour {
    enum MenuStates {
        MainMenu,
        StageSelect,
        Options,
    }

    [SerializeField]
    GameObject m_BackButton;
    [SerializeField]
    GameObject m_Mainmenu;
    [SerializeField]
    GameObject m_StageSelect;
    [SerializeField]
    GameObject m_Options;

    // Start is called before the first frame update
    void Start() {
    }

    #region STATE_SWITCH

    /// <summary>
    /// inactive all children of Menu, but leave Menu active.
    /// </summary>
    private void FlushUI() {
        foreach( Transform menu in transform.Find( "Menu" ) ) {
            menu.gameObject.SetActive( false );
        }
    }

    private void SwitchTo( MenuStates states ) {
        transform.Find( "Menu/" + states.ToString() ).gameObject.SetActive( true );
    }

    void Switch( MenuStates state ) {
        FlushUI();
        SwitchTo( state );
    }

    #endregion

    #region STAGE
    void LoadStage( string stage ) {
        Debug.Log( "Try to load scene: " + stage );

        SceneManager.LoadScene( stage );
    }
    #endregion

    #region EVENTS
    public void OnStartClicked() {
        Switch( MenuStates.StageSelect );
    }
    public void OnOptionClicked() {
        Debug.Log( "Options clicked(unimplemented)" );
        Switch( MenuStates.Options );
    }
    public void OnQuitClicked() {
        Debug.Log( "Quit" );
        Application.Quit();
    }
    public void OnTutorialSelected() {
        Debug.Log( "Tutorial Selected" );
        LoadStage( "SampleScene" );
    }
    public void OnStage_1Selected() {
        Debug.Log( "Stage_1 Selected" );
        LoadStage( "DemoScene" );
    }
    public void OnStage_2Selected() {
        Debug.Log( "Stage_2 Selected" );
        //SceneManager.LoadScene("DemoScene");
    }
    public void OnStage_3Selected() {
        Debug.Log( "Stage_3 Selected" );
        //SceneManager.LoadScene("DemoScene");
    }
    public void OnBackClicked() {
        Switch( MenuStates.MainMenu );
    }
    #endregion
}
