using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float jumpVelocity;
    public float movementSpeed;
    [Range(0,10)]
    public float fallSpeed;
    MyCharacter myCharacter;
    Rigidbody rb;

    PlayerEnum playerEnum = PlayerEnum.NotAssigned;

    int jumps = 0;
    [Header("How much Jumps in a Row")]
    public int maxJumps;

    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.sleepThreshold = 0;
    }

    void Update()
    {
        if(rb.velocity.y == 0)
        {
            if (jumps > 0)
                ResetJumps();
        }
        else if(rb.velocity.y < 0)
        {
          rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        }
    }

    #region Input
    public void AssigneInput()
    {
        // GET MyCharacter and SET the Player ENUM
        myCharacter = this.gameObject.GetComponent<MyCharacter>();
        playerEnum = myCharacter.playerEnum;

        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.P1_LeftStickRightAction += MoveRight;
                InputManager.P1_LeftStickLeftAction += MoveLeft;
                InputManager.P1_AButtonDownAction += Jump;
                InputManager.P1_LeftTriggerDownAction += DashCheck;
                InputManager.P1_RightTiggerDownAction += DashCheck;

                break;
            case PlayerEnum.PlayerTwo:
                InputManager.P2_LeftStickRightAction += MoveRight;
                InputManager.P2_LeftStickLeftAction += MoveLeft;
                InputManager.P2_AButtonDownAction += Jump;
                InputManager.P2_LeftTriggerDownAction += DashCheck;
                InputManager.P2_RightTiggerDownAction += DashCheck;

                break;
            case PlayerEnum.NotAssigned:
                Debug.Log("<color=red> MovementClass cant Find the PlayerEnum </color>");
                UnityEditor.EditorApplication.isPlaying = false;
                
                break;
        }
    }

    void RemoveInput()
    {
        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:

                break;
            case PlayerEnum.PlayerTwo:

                break;
            case PlayerEnum.NotAssigned:
                Debug.Log("<color=red> MovementClass cant Find the PlayerEnum </color>");
                break;
        }
    }
    #endregion

    public void ResetJumps()
    {
        jumps = 0;
    }
    
    void DashCoolDown()
    {
        myCharacter.canGetDamaged = true;
    }

    void DashCheck()
    {
        switch(playerEnum)
        {
            case PlayerEnum.PlayerOne:
                if(Input.GetAxisRaw("P1_Horizontal") == 1)
                {
                    //Right
                    DashRayCast();
                }
                else if(Input.GetAxisRaw("P1_Horizontal") == -1)
                {
                    //Left

                }
                else
                {
                    DashStanding();
                }
                break;
            case PlayerEnum.PlayerTwo:
                if (Input.GetAxisRaw("P2_Horizontal") == 1)
                {
                    //Right
                    DashRayCast();

                }
                else if (Input.GetAxisRaw("P2_Horizontal") == -1)
                {
                    //Left

                }
                else
                {
                    DashStanding();
                }
                break;
            case PlayerEnum.NotAssigned:
                Debug.Log("<color=red> MovementClass -> DashCheck() cant Find the PlayerEnum </color>");
                break;
        }
    }

    //////////////////////////////////////////////////
    //////////////// Movement Functions //////////////
    //////////////////////////////////////////////////

    void MoveRight()
    {
        if(this.transform.rotation.y != 0)
        {
            this.transform.rotation = new Quaternion(0,0,0,0);
        }
        this.gameObject.transform.position += Vector3.right * movementSpeed * Time.deltaTime;
    }

    void MoveLeft()
    {
        if (this.transform.rotation.y != 180)
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        this.gameObject.transform.position += Vector3.left * movementSpeed * Time.deltaTime;
    }

    void Jump()
    {
        if (jumps < maxJumps)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * jumpVelocity;
            jumps++;
        }
    }

    void DashStanding()
    {
        myCharacter.canGetDamaged = false;
        Invoke("DashCoolDown", myCharacter.dashTime);
    }

    void DashRayCast()
    {
        RaycastHit hit;
        Debug.Log("FUCKUUUUUUUUUUUUUUUUUUUU");
        
        Debug.DrawRay(this.gameObject.transform.position + Vector3.up, Vector3.right * 10, Color.red, 10.0f);
        if(Physics.Raycast(this.gameObject.transform.position + Vector3.up, Vector3.right, out hit, 5.0f))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }


}
