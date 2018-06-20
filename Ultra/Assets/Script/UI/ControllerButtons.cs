using UnityEngine;
using UnityEngine.UI;

public class ControllerButtons : MonoBehaviour
{
    Button button;
    UnityEngine.EventSystems.BaseEventData bED;
    bool selected = false;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (!selected)
        {
            if (bED.selectedObject == button)
            {
                selected = true;

                InputManager.P1_LeftStickLeftAction += lol;
                InputManager.P2_LeftStickLeftAction += lol;
                InputManager.P1_LeftStickRightAction += lol;
                InputManager.P2_LeftStickRightAction += lol;
            }
        }
        else
        {
            if (bED.selectedObject != button)
            {
                selected = false;

                InputManager.P1_LeftStickLeftAction -= lol;
                InputManager.P2_LeftStickLeftAction -= lol;
                InputManager.P1_LeftStickRightAction -= lol;
                InputManager.P2_LeftStickRightAction -= lol;
            }
        }
    }

    void lol()
    { }
}