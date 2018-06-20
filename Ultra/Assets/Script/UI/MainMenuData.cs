using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuData : MonoBehaviour
{
    [Header("Pannel")]
    public GameObject main;
    public GameObject options;
    public GameObject credits;
    public GameObject playerSelect;

    [Header("EventSystem")]
    public UnityEngine.EventSystems.EventSystem eventSystem;

    [Header("Buttons OBJs")]
    /// <summary>
    /// Lists of all Buttons that get Selected First
    /// </summary>
    public List<GameObject> buttons;
    public List<GameObject> optionsButton;
    public List<GameObject> optionsPannel;

    [Header("Buttons")]
    public GameObject[] headerButtons;
    public List<GameObject> VideoButtons;
    public List<GameObject> AudioButtons;

    [Header("CharacterSelecter")]
    public CharacterSelecterV2 cS;

    [Header("GreyPannel")]
    public GameObject headerGreyPannel;
    public GameObject bodyGreyPannel;

    /// <summary>
    /// Dependend on the oP State, turn all other oP Pannel off
    /// </summary>
    /// <param name="oP"></param>
    public void TurnOptionsPannelOff(OptionsPannel oP)
    {
        switch (oP)
        {
            case OptionsPannel.Video:
                optionsPannel[0].SetActive(true);
                optionsPannel[1].SetActive(false);
                optionsPannel[2].SetActive(false);
                break;
            case OptionsPannel.Audio:
                optionsPannel[0].SetActive(false);
                optionsPannel[1].SetActive(true);
                optionsPannel[2].SetActive(false);
                break;
            case OptionsPannel.Controlls:
                optionsPannel[0].SetActive(false);
                optionsPannel[1].SetActive(false);
                optionsPannel[2].SetActive(true);
                break;
        }
    }
    /// <summary>
    /// Set the new Selected GameObject in the EventSystem (for Controller Input)
    /// </summary>
    /// <param name="go"></param>
    public void SetSelectedGameObject(GameObject go)
    {
        eventSystem.SetSelectedGameObject(go);
    }
    /// <summary>
    /// Disable Header Buttons
    /// </summary>
    public void DisableHeaderButtons()
    {
        Debug.Log(headerButtons.Length);
        for (int i = 0; i < headerButtons.Length; i++)
        {
            headerButtons[i].GetComponent<Button>().enabled = false;
        }
    }

    //Maybe Lerp below
    public void SetHeaderGrey(bool state)
    {
        headerGreyPannel.SetActive(state);
    }
    public void SetBodyGrey(bool state)
    {
        headerGreyPannel.SetActive(state);
    }
}