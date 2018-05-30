using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementV2 : MonoBehaviour
{
    [SerializeField] float speed; 
    CharacterController cC;
    TestMov testMov;
    PlayerEnum playerEnum = PlayerEnum.NotAssigned;



    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        cC = GetComponent<CharacterController>();
    }
    private void Update()
    {

    }

    public void AssigneInput()
    {
        testMov = this.gameObject.GetComponent<TestMov>();
        playerEnum = testMov.playerEnum;
       
        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.P1_LeftStickRightAction += MoveRight;
                InputManager.P1_LeftStickLeftAction += MoveLeft;

                break;
            case PlayerEnum.PlayerTwo:
                InputManager.P2_LeftStickRightAction += MoveRight;
                InputManager.P2_LeftStickLeftAction += MoveLeft;

                break;
            case PlayerEnum.NotAssigned:
                Debug.Log("<color=red> MovementClass cant Find the PlayerEnum </color>");

                break;
        }
    }

    void MoveRight()
    {
        cC.Move(new Vector3(transform.position.x + (speed * Time.deltaTime), 0, 0));
    }
    void MoveLeft()
    {
        cC.Move(new Vector3(transform.position.x - (speed * Time.deltaTime), 0, 0));
    }
}
