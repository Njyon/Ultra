using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] MainMenuData mMD;
    MenuState menuState = MenuState.Main;
    OptionsPannel optionsPannel = OptionsPannel.Video;
    bool inSubMenu = false;

    void Start()
    {
        if (mMD.main == null || mMD.options == null || mMD.credits == null || mMD.championSelect == null || mMD.arenaSelect == null)
        {
            Debug.Log("Missing Pannels on " + gameObject.name);
            return;
        }

        GetResolutions();
        GetQualityLevels();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        mMD.DeActivateButtons(mMD.videoButtons);
        mMD.DeActivateButtons(mMD.audioButtons);
        mMD.DeActivateButtons(mMD.headerButtons);

        mMD.DeactivatePanels(mMD.optionsPannel);

        // Deactivate all Pannels exept main
        mMD.options.SetActive(false);
        mMD.credits.SetActive(false);
        mMD.championSelect.SetActive(false);
        mMD.arenaSelect.SetActive(false);
    }

    void Update()
    {
        // if the Player hits the mouse button the last main button get reSelected
        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            mMD.eventSystem.SetSelectedGameObject(mMD.mainbuttons[(int)menuState]);
        }

        // Fast Gate 02 Hack, Remove as Soon as Possible!
        if(Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if(menuState == MenuState.Credits)
            {
                mMD.ActivateButtons(mMD.mainbuttons);
                ShowMain();
            }
            else if(menuState == MenuState.Options && !inSubMenu)
            {
                mMD.ActivateButtons(mMD.mainbuttons);
                ShowMain();
            }
            else if (menuState == MenuState.Options && inSubMenu)
            {
                //Leaves subMenu
                inSubMenu = false;
                // Select the button of the subMenu
                mMD.ActivateButtons(mMD.headerButtons);
                //Go to the Optons Menu
                OptionsOn();
            }
            else if(menuState == MenuState.PlayerSelect && mMD.cS.NoCharSelected())
            {
                mMD.ActivateButtons(mMD.mainbuttons);
                ShowMain();
            }
        }

        if(menuState == MenuState.Options)
        {
            if(optionsPannel != OptionsPannel.Video && mMD.eventSystem.currentSelectedGameObject == mMD.headerButtons[0])
            {
                optionsPannel = OptionsPannel.Video;
                mMD.TurnOptionsPannelOff(optionsPannel);
            }
            else if (optionsPannel != OptionsPannel.Audio && mMD.eventSystem.currentSelectedGameObject == mMD.headerButtons[1])
            {
                optionsPannel = OptionsPannel.Audio;
                mMD.TurnOptionsPannelOff(optionsPannel);
            }
            else if (optionsPannel != OptionsPannel.Controlls &&mMD.eventSystem.currentSelectedGameObject == mMD.headerButtons[2])
            {
                optionsPannel = OptionsPannel.Controlls;
                mMD.TurnOptionsPannelOff(optionsPannel);
            }
        }
    }
    
    void GetResolutions()
    {
        mMD.resolutions = Screen.resolutions;
        foreach(Resolution res in mMD.resolutions)
        {
            mMD.videoButtons[0].GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(res.ToString()));
        }
        // fast hack to display a value at start
        mMD.videoButtons[0].GetComponent<Dropdown>().value = mMD.videoButtons[0].GetComponent<Dropdown>().options.Count - 2;
    }
    void GetQualityLevels()
    {
        mMD.qulaityLevels = QualitySettings.names;
        foreach(string level in mMD.qulaityLevels)
        {
            mMD.videoButtons[1].GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(level));
        }
        // fast hack to display a value at start
        mMD.videoButtons[1].GetComponent<Dropdown>().value = mMD.videoButtons[1].GetComponent<Dropdown>().options.Count - 1;
    }

    #region Buttons

    /// <summary>
    /// Turn Main pannel on and the rest off
    /// </summary>
    public void ShowMain()
    {
        if(menuState == MenuState.Options)
        {
            mMD.OptionsAnimator.SetBool("ButtonsOut", true);
        }
        mMD.main.SetActive(true);
        Invoke("MainOn", 0.4f);
    }
    void MainOn()
    {
        mMD.main.SetActive(true);
        mMD.options.SetActive(false);
        mMD.credits.SetActive(false);
        mMD.championSelect.SetActive(false);
        mMD.arenaSelect.SetActive(false);
        mMD.background.SetActive(true);
        mMD.DeactivatePanels(mMD.optionsPannel);

        mMD.cS.HiddeNav();
        mMD.cS.RemoveInput();
        menuState = MenuState.Main;
        mMD.eventSystem.SetSelectedGameObject(mMD.mainbuttons[(int)menuState]);

        mMD.MainAnimator.SetBool("ButtonsOut", false);
    }
    /// <summary>
    /// Turn Options pannel on and the rest off
    /// </summary>
    public void ShowOptions()
    {
        mMD.MainAnimator.SetBool("ButtonsOut", true);
        Invoke("OptionsOn", 0.4f);
    }
    void OptionsOn()
    {
        mMD.main.SetActive(false);
        mMD.options.SetActive(true);
        mMD.credits.SetActive(false);
        mMD.championSelect.SetActive(false);
        mMD.arenaSelect.SetActive(false);
        mMD.background.SetActive(true);

        //Deactivate objects
        mMD.DeActivateButtons(mMD.videoButtons);
        mMD.DeActivateButtons(mMD.audioButtons);
        mMD.DeActivateButtons(mMD.mainbuttons);
        //Activate the HEader buttons
        mMD.ActivateButtons(mMD.headerButtons);

        mMD.TurnOptionsPannelOff(optionsPannel);
        //Set menuState to Options
        menuState = MenuState.Options;
        //Set the selected Object to the last selected Object
        mMD.eventSystem.SetSelectedGameObject(mMD.headerButtons[(int)optionsPannel]);

        mMD.OptionsAnimator.SetBool("ButtonsOut", false);
    }
    /// <summary>
    /// Turn Cretis pannel on and the rest off
    /// </summary>
    public void ShowCredits()
    {
        mMD.main.SetActive(false);
        mMD.options.SetActive(false);
        mMD.credits.SetActive(true);
        mMD.championSelect.SetActive(false);
        mMD.arenaSelect.SetActive(false);

        menuState = MenuState.Credits;
    }
    /// <summary>
    /// Turn PlayerSelect pannel on and the rest off
    /// </summary>
    public void ShowPlayerSelect()
    {
        mMD.main.SetActive(false);
        mMD.options.SetActive(false);
        mMD.credits.SetActive(false);
        mMD.championSelect.SetActive(true);
        mMD.arenaSelect.SetActive(false);
        mMD.background.SetActive(false);

        mMD.myCamera.Play();

        menuState = MenuState.PlayerSelect;
        mMD.cS.ShowNav();
        mMD.cS.ApplyInput();
    }
    public void OnVideoEdit()
    {
        //Enters subMenu
        inSubMenu = true;

        //Activate the Objects in the VideoPanelBody
        mMD.ActivateButtons(mMD.videoButtons);
        // Select the first object
        mMD.SetSelectedGameObject(mMD.videoButtons[0]);

        //Deactivate the HeaderButtons
        mMD.DeActivateButtons(mMD.headerButtons);
    }
    public void OnSoundEdit()
    {
        //Enters subMenu
        inSubMenu = true;

        //Activate the Objects in the VideoPanelBody
        mMD.ActivateButtons(mMD.audioButtons);
        // Select the first object
        mMD.SetSelectedGameObject(mMD.audioButtons[0]);

        //Deactivate the HeaderButtons
        mMD.DeActivateButtons(mMD.headerButtons);
    }

    #endregion
    #region Settings

    public void VSync(GameObject go)
    {
        if(go.GetComponent<Toggle>().isOn)
        {
            QualitySettings.vSyncCount = 2;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
    public void FullScreen(GameObject go)
    {
        if (go.GetComponent<Toggle>().isOn)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }
    public void OnResChange()
    {
        Screen.SetResolution(mMD.resolutions[mMD.videoButtons[0].GetComponent<Dropdown>().value].width,
            mMD.resolutions[mMD.videoButtons[0].GetComponent<Dropdown>().value].height,
            Screen.fullScreen
            );
    }
    public void OnQualityChange()
    {
        QualitySettings.SetQualityLevel(mMD.videoButtons[1].GetComponent<Dropdown>().value);
    }
    public void OnExit()
    {
        Application.Quit();
    }

    #endregion
}

public enum MenuState
{
    Main,
    Options,
    Credits,
    PlayerSelect
}

public enum OptionsPannel
{
    Video,
    Audio,
    Controlls
}