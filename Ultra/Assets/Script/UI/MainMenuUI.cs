﻿using System.Collections;
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
        
        mMD.musikMixer.SetFloat("music", 0);
        mMD.musikMixer.SetFloat("sfx", 0);
        mMD.audioButtons[0].GetComponent<Slider>().value = 1;
        mMD.audioButtons[1].GetComponent<Slider>().value = 1;

        mMD.p1_Ready.SetActive(false);
        mMD.p2_Ready.SetActive(false);

        mMD.DeActivateButtons(mMD.videoButtons);
        mMD.DeActivateButtons(mMD.audioButtons);
        mMD.DeActivateButtons(mMD.headerButtons);

        mMD.DeactivatePanels(mMD.optionsPannel);

        // Deactivate all Pannels exept main
        mMD.options.SetActive(false);
        mMD.credits.SetActive(false);
        mMD.championSelect.SetActive(false);
        mMD.arenaSelect.SetActive(false);
        mMD.optionsBody.SetActive(false);
        Fabric.EventManager.Instance.PostEvent("MenuStart", this.gameObject);

        mMD.cS.p1_Ready += P1_IsReady;
        mMD.cS.p2_Ready += P2_IsReady;
    }

    private void OnDisable()
    {
        mMD.cS.p1_Ready -= P1_IsReady;
        mMD.cS.p2_Ready -= P2_IsReady;
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
                mMD.TransitionAnimator.SetBool("makeTransition", false);
                Fabric.EventManager.Instance.PostEvent("MenuStart", this.gameObject);
                Invoke("ShowMain", 0.3f);
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
        mMD.videoButtons[0].GetComponent<Dropdown>().value = mMD.videoButtons[0].GetComponent<Dropdown>().options.Count;
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
            Fabric.EventManager.Instance.PostEvent("MenuOptionsLeave", this.gameObject);
        }
        if (menuState == MenuState.Credits)
        {
            mMD.TransitionAnimator.SetBool("credits", false);
            Fabric.EventManager.Instance.PostEvent("MenuOptionsLeave", this.gameObject);
        }
        mMD.main.SetActive(true);
        mMD.background.SetActive(true);
        mMD.credits.SetActive(false);
        mMD.vS.IStart();
        Invoke("MainOn", 0.4f);
    }
    void MainOn()
    {
        mMD.optionsBody.SetActive(false);
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
        mMD.optionsBody.SetActive(true);
        mMD.MainAnimator.SetBool("ButtonsOut", true);
        Fabric.EventManager.Instance.PostEvent("MenuOptionsEnter", this.gameObject);
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
        mMD.TransitionAnimator.SetBool("credits", true);
        mMD.MainAnimator.SetBool("ButtonsOut", true);
        Fabric.EventManager.Instance.PostEvent("MenuOptionsEnter", this.gameObject);
        Invoke("CreditsOn", 1f);
    }
    void CreditsOn()
    {
        mMD.main.SetActive(false);
        mMD.options.SetActive(false);
        mMD.credits.SetActive(true);
        mMD.championSelect.SetActive(false);
        mMD.arenaSelect.SetActive(false);

        menuState = MenuState.Credits;
    }
    public void Play()
    {
        mMD.MainAnimator.SetBool("ButtonsOut", true);
        mMD.TransitionAnimator.SetBool("makeTransition", true);
        Fabric.EventManager.Instance.PostEvent("MenuMainLeaveEnterCharacterSelection", this.gameObject);
        Invoke("ShowPlayerSelect", 0.5f);
    }
    /// <summary>
    /// Turn PlayerSelect pannel on and the rest off
    /// </summary>
    public void ShowPlayerSelect()
    {
        //    mMD.ChooseColorAnimator.SetBool("DragBarAway", false);

        mMD.main.SetActive(false);
        mMD.options.SetActive(false);
        mMD.credits.SetActive(false);
        mMD.championSelect.SetActive(true);
        mMD.arenaSelect.SetActive(false);
        mMD.background.SetActive(false);

        //mMD.myCamera.Play();

        menuState = MenuState.PlayerSelect;
        mMD.cS.ShowNav();
        mMD.cS.ApplyInput();

        mMD.ChooseColorAnimator.SetBool("DragBarAway", true);
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

    public void OnShowControls()
    {
        
    }

    #endregion
    #region Settings


    public void OnMusicChange()
    { 
        mMD.musikMixer.SetFloat("music", ((mMD.audioButtons[0].GetComponent<Slider>().value * 80) - 80));
        Fabric.EventManager.Instance.PostEvent("MenuOptionsAdjustUp", this.gameObject);
    }
    public void OnSFXChange()
    {
        mMD.musikMixer.SetFloat("sfx", ((mMD.audioButtons[1].GetComponent<Slider>().value * 80) - 80));
        Fabric.EventManager.Instance.PostEvent("MenuOptionsAdjustDown", this.gameObject);
    }

    public void VSync(GameObject go)
    {
        if(go.GetComponent<Toggle>().isOn)
        {
            QualitySettings.vSyncCount = 2;
            Fabric.EventManager.Instance.PostEvent("MenuOptionsAdjustUp");
            Debug.Log("Test");
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Fabric.EventManager.Instance.PostEvent("MenuOptionsAdjustDown");
            Debug.Log("Test");
        }
    }
    public void FullScreen(GameObject go)
    {
        if (go.GetComponent<Toggle>().isOn)
        {
            Screen.fullScreen = true;
            Fabric.EventManager.Instance.PostEvent("MenuOptionsAdjustUp");
            Debug.Log("Test");
        }
        else
        {
            Screen.fullScreen = false;
            Fabric.EventManager.Instance.PostEvent("MenuOptionsAdjustDown");
            Debug.Log("Test");
        }
    }
    public void OnResChange()
    {
        Screen.SetResolution(mMD.resolutions[mMD.videoButtons[0].GetComponent<Dropdown>().value].width,
            mMD.resolutions[mMD.videoButtons[0].GetComponent<Dropdown>().value].height,
            Screen.fullScreen
            );
        Fabric.EventManager.Instance.PostEvent("MenuSubmenuBackward");
    }
    public void OnQualityChange()
    {
        QualitySettings.SetQualityLevel(mMD.videoButtons[1].GetComponent<Dropdown>().value);
        Fabric.EventManager.Instance.PostEvent("MenuSubmenuClick");
    }
    public void OnExit()
    {
        Application.Quit();
    }

    #endregion

    void P1_IsReady(bool isReady)
    {
        if(isReady)
        {
            mMD.p1_Ready.SetActive(true);
            mMD.p1_Animator.SetBool("Ready", true);
            Fabric.EventManager.Instance.PostEvent("MenuCharSelectReady", mMD.p1_Animator.gameObject);
        }
        else
        {
            mMD.p1_Ready.SetActive(false);
            mMD.p1_Animator.SetBool("Ready", false);
            Fabric.EventManager.Instance.PostEvent("MenuCharSelectUnready", mMD.p1_Animator.gameObject);
        }
    }
    void P2_IsReady(bool isReady)
    {
        if(isReady)
        {
            mMD.p2_Ready.SetActive(true);
            mMD.p2_Animator.SetBool("Ready", true);
            Fabric.EventManager.Instance.PostEvent("MenuCharSelectReady", mMD.p2_Animator.gameObject);
        }
        else
        {
            mMD.p2_Ready.SetActive(false);
            mMD.p2_Animator.SetBool("Ready", false);
            Fabric.EventManager.Instance.PostEvent("MenuCharSelectUnready", mMD.p2_Animator.gameObject);
        }
    }

    public void SelectMenuEntry()
    {
        Fabric.EventManager.Instance.PostEvent("MenuMainSelect", mMD.p2_Animator.gameObject);
    }

    public void OptionsVsyncFullScreenBool (Toggle value) {
        Fabric.EventManager.Instance.PostEvent(value.isOn ? "MenuOptionsAdjustUp" : "MenuOptionsAdjustDown");
    }
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