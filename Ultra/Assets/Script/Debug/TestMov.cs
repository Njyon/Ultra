using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMov : MonoBehaviour
{
    [HideInInspector] public PlayerEnum playerEnum = PlayerEnum.NotAssigned; 
    Rigidbody rb;
    MovementV2 mov;
    public void Posses()
    {
        mov = gameObject.GetComponent<MovementV2>();
        mov.AssigneInput();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Input
        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
               
                break;
            case PlayerEnum.PlayerTwo:
                
                break;
            case PlayerEnum.NotAssigned:
            default:
                Debug.Log("Coult not Assign Input");
                break;
        }
    }
}