using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] MainMenuData mMD;
    MenuState menuState = MenuState.Main;
    OptionsPannel optionsPannel = OptionsPannel.Video;

    void Start()
    {
        if (mMD.main == null || mMD.options == null || mMD.credits == null || mMD.playerSelect == null)
        {
            Debug.Log("Missing Pannels on " + gameObject.name);
            return;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Deactivate all Pannels exept main
        mMD.options.SetActive(false);
        mMD.credits.SetActive(false);
        mMD.playerSelect.SetActive(false);
    }

    void Update()
    {
        // if the Player hits the mouse button the last main button get reSelected
        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            mMD.eventSystem.SetSelectedGameObject(mMD.buttons[(int)menuState]);
        }

        if(menuState == MenuState.Options)
        {
            if(optionsPannel != OptionsPannel.Video && mMD.eventSystem.currentSelectedGameObject == mMD.optionsButton[0])
            {
                optionsPannel = OptionsPannel.Video;
                mMD.TurnOptionsPannelOff(optionsPannel);
            }
            else if (optionsPannel != OptionsPannel.Audio && mMD.eventSystem.currentSelectedGameObject == mMD.optionsButton[1])
            {
                optionsPannel = OptionsPannel.Audio;
                mMD.TurnOptionsPannelOff(optionsPannel);
            }
            else if (optionsPannel != OptionsPannel.Controlls &&mMD.eventSystem.currentSelectedGameObject == mMD.optionsButton[2])
            {
                optionsPannel = OptionsPannel.Controlls;
                mMD.TurnOptionsPannelOff(optionsPannel);
            }
        }
    }
    
    // Funtions to turn Pannels on and off
    /// <summary>
    /// Turn Main pannel on and the rest off
    /// </summary>
    public void ShowMain()
    {
        mMD.main.SetActive(true);
        mMD.options.SetActive(false);
        mMD.credits.SetActive(false);
        mMD.playerSelect.SetActive(false);

        mMD.cS.HiddeNav();
        mMD.cS.RemoveInput();
        menuState = MenuState.Main;
        mMD.eventSystem.SetSelectedGameObject(mMD.buttons[(int)menuState]);
    }
    /// <summary>
    /// Turn Options pannel on and the rest off
    /// </summary>
    public void ShowOptions()
    {
        mMD.main.SetActive(false);
        mMD.options.SetActive(true);
        mMD.credits.SetActive(false);
        mMD.playerSelect.SetActive(false);

        mMD.SetHeaderGrey(false);
        mMD.SetBodyGrey(true);

        menuState = MenuState.Options;
        mMD.eventSystem.SetSelectedGameObject(mMD.buttons[(int)menuState]);
    }
    /// <summary>
    /// Turn Cretis pannel on and the rest off
    /// </summary>
    public void ShowCredits()
    {
        mMD.main.SetActive(false);
        mMD.options.SetActive(false);
        mMD.credits.SetActive(true);
        mMD.playerSelect.SetActive(false);
    }
    /// <summary>
    /// Turn PlayerSelect pannel on and the rest off
    /// </summary>
    public void ShowPlayerSelect()
    {
        mMD.main.SetActive(false);
        mMD.options.SetActive(false);
        mMD.credits.SetActive(false);
        mMD.playerSelect.SetActive(true);

        mMD.cS.ShowNav();
        mMD.cS.ApplyInput();
    }

    public void OnVideoEdit()
    {
        mMD.SetHeaderGrey(true);
        mMD.SetBodyGrey(false);

        mMD.DisableHeaderButtons();
    }
}

public enum MenuState
{
    Main,
    Options,
    Credits,
}

public enum OptionsPannel
{
    Video,
    Audio,
    Controlls
}