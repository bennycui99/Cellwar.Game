using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour
{
    enum menustate
    {
        Main,
        Stage,
        Options,
    }
    [SerializeField]
    GameObject BackButton;
    [SerializeField]
    GameObject Mainmenu;
    [SerializeField]
    GameObject StageSelect;
    [SerializeField]
    GameObject Options;
    menustate state;
    // Start is called before the first frame update
    void Start()
    {
        state = menustate.Main;
    }
    void LoadStage(string stage)
    {
        Debug.Log("Try to load scene: " + stage);
        SceneManager.LoadScene(stage);
    }
    void SwitchoutMainmenu(string target)
    {
        //Input an empty string to switch back to mainmenu
        switch (target)
        {
            case "Main":
                Debug.Log("Back to Main");
                if (state == menustate.Options)
                {
                    Options.SetActive(false); 
                }
                else if (state == menustate.Stage)
                {
                    StageSelect.SetActive(false);
                }
                Mainmenu.SetActive(true);
                BackButton.SetActive(false);
                state = menustate.Main;
                break;
            case "Stage":
                Mainmenu.SetActive(false);
                StageSelect.SetActive(true);
                BackButton.SetActive(true);
                state = menustate.Stage;
                break;
            case "Options":
                Mainmenu.SetActive(false);
                Options.SetActive(true);
                BackButton.SetActive(true);
                state = menustate.Options;
                break;
        }
    }

    //OnClick Functions
    public void OnStartClicked()
    {
        SwitchoutMainmenu("Stage");
    }
    public void OnOptionClicked()
    {
        Debug.Log("Options clicked(unimplemented)");
        SwitchoutMainmenu("Options");
    }
    public void OnQuitClicked()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void OnTutorialSelected()
    {
        Debug.Log("Tutorial Selected");
        LoadStage("SampleScene");
    }
    public void OnStage_1Selected()
    {
        Debug.Log("Stage_1 Selected");
        LoadStage("DemoScene");
    }
    public void OnStage_2Selected()
    {
        Debug.Log("Stage_2 Selected");
        //SceneManager.LoadScene("DemoScene");
    }
    public void OnStage_3Selected()
    {
        Debug.Log("Stage_3 Selected");
        //SceneManager.LoadScene("DemoScene");
    }
    public void OnBackClicked()
    {
        SwitchoutMainmenu("Main");
    }
}
