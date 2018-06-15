using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Pannel")]
    [SerializeField] GameObject main;
    [SerializeField] GameObject options;
    [SerializeField] GameObject credits;
    [SerializeField] GameObject playerSelect;

    [Header("EventSystem")] 
    [SerializeField] UnityEngine.EventSystems.EventSystem eventSystem;

    [Header("Buttons")]
    /// <summary>
    /// Lists of all Buttons that get Selected First
    /// </summary>
    [SerializeField] List<GameObject> buttons;

    [Header("CharacterSelecter")]
    [SerializeField] CharacterSelecterV2 cS;

    void Start()
    {
        if(main == null || options == null || credits == null || playerSelect == null)
        {
            Debug.Log("Missing Pannels on " + gameObject.name);
            return;
        }

        // Deactivate all Pannels exept main
        options.SetActive(false);
        credits.SetActive(false);
        playerSelect.SetActive(false);
    }

    // Funtions to turn Pannels on and off
    /// <summary>
    /// Turn Main pannel on and the rest off
    /// </summary>
    public void ShowMain()
    {
        main.SetActive(true);
        options.SetActive(false);
        credits.SetActive(false);
        playerSelect.SetActive(false);

        cS.HiddeNav();
        cS.RemoveInput();
        eventSystem.SetSelectedGameObject(buttons[0]);
    }
    /// <summary>
    /// Turn Options pannel on and the rest off
    /// </summary>
    public void ShowOptions()
    {
        main.SetActive(false);
        options.SetActive(true);
        credits.SetActive(false);
        playerSelect.SetActive(false);
    }
    /// <summary>
    /// Turn Cretis pannel on and the rest off
    /// </summary>
    public void ShowCredits()
    {
        main.SetActive(false);
        options.SetActive(false);
        credits.SetActive(true);
        playerSelect.SetActive(false);
    }
    /// <summary>
    /// Turn PlayerSelect pannel on and the rest off
    /// </summary>
    public void ShowPlayerSelect()
    {
        main.SetActive(false);
        options.SetActive(false);
        credits.SetActive(false);
        playerSelect.SetActive(true);

        cS.ShowNav();
        cS.ApplyInput();
    }
}
