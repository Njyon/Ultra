using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuData : MonoBehaviour
{
    [Header("Pannel")]
    public GameObject main;
    public GameObject options;
    public GameObject arenaSelect;
    public GameObject championSelect;
    public GameObject credits;
    public GameObject background;
    public GameObject optionsBody;
    public List<GameObject> optionsPannel;

    [Header("EventSystem")]
    public UnityEngine.EventSystems.EventSystem eventSystem;

    [Header("Buttons OBJs")]
    public List<GameObject> mainbuttons;

    [Header("Options OBJs")]
    public List<GameObject> headerButtons;
    public List<GameObject> videoButtons;
    public List<GameObject> audioButtons;

    [Header("CharacterSelecter")]
    public CharacterSelecterV2 cS;

    [Header("Camera")]
    public MainMenuCam myCamera;

    [Header("Animator")]
    public Animator MainAnimator;
    public Animator OptionsAnimator;
    public Animator TransitionAnimator;
    public Animator ChooseColorAnimator;

    [HideInInspector] public Resolution[] resolutions;
    [HideInInspector] public string[] qulaityLevels;

    public VideoScript vS;

    [Header("Ready Texts")]
    public GameObject p1_Ready;
    public GameObject p2_Ready;

    [Header("Player Animator")]
    public Animator p1_Animator;
    public Animator p2_Animator;

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
    /// Deactivates all GameObjects in "goList"
    /// </summary>
    /// <param name="goList"></param>
    public void DeactivatePanels(List<GameObject> goList)
    {
        foreach(GameObject go in goList)
        {
            go.SetActive(false);
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
    /// Deactivates all Buttons, Slider and Dropdownmenus in "goList"
    /// </summary>
    /// <param name="goList"></param>
    public void DeActivateButtons(List<GameObject> goList)
    {
        foreach (GameObject go in goList)
        {
            if (go.GetComponent<Button>())
            {
                go.GetComponent<Button>().interactable = false;
            }
            else if (go.GetComponent<Slider>())
            {
                go.GetComponent<Slider>().interactable = false;
            }
            else if (go.GetComponent<Dropdown>())
            {
                go.GetComponent<Dropdown>().interactable = false;
            }
            else if (go.GetComponent<Toggle>())
            {
                go.GetComponent<Toggle>().interactable = false;
            }
        }
    }
    /// <summary>
    /// Activates all Buttons, Slider and Dropdownmenus in "goList"
    /// </summary>
    /// <param name="goList"></param>
    public void ActivateButtons(List<GameObject> goList)
    {
        foreach (GameObject go in goList)
        {
            if (go.GetComponent<Button>())
            {
                go.GetComponent<Button>().interactable = true;
            }
            else if (go.GetComponent<Slider>())
            {
                go.GetComponent<Slider>().interactable = true;
            }
            else if (go.GetComponent<Dropdown>())
            {
                go.GetComponent<Dropdown>().interactable = true;
            }
            else if (go.GetComponent<Toggle>())
            {
                go.GetComponent<Toggle>().interactable = true;
            }
        }
    }

}