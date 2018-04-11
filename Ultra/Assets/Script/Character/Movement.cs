using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    // Helper
    MyCharacter myCharacter;
    Rigidbody rb;
    PlayerEnum playerEnum = PlayerEnum.NotAssigned;

    // Movement
    public float movementSpeed;
    float wallDetectionLength = 0.6f;
    bool isFalling = false;
   
    // Jump
    public float jumpVelocity;
    [Range(0, 10)]
    public float fallSpeed;
    int jumps = 0;
    [Header("How much Jumps in a Row")]
    public int maxJumps;

    //Dash
    [Header("Dash")]
    public float dashLength;
    public float dashTime;
    float currentDashTime;
    float journeyLength;
    float dashWallDistance = 0.6f;
    bool isDashing = false;
    Vector3 dashEndPoint;

    //WallSlide
    bool isOnWallRight = false;
    bool isOnWallLeft = false;

    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    //////////////////////////////////////////////////
    ////////////////       Update       //////////////
    //////////////////////////////////////////////////

    void Update()
    {
        Falling();
        Dash();


        if(this.isOnWallRight)
        {
            RaycastHit hit;
            if (this.gameObject.transform.localPosition.x < 0)
            {
                if (!Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
                {
                    this.isOnWallRight = false;
                }
            }
            else
            {
                if (!Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
                {
                    this.isOnWallRight = false;
                }
            }
        }
        else if(this.isOnWallLeft)
        {
            RaycastHit hit;
            if (this.gameObject.transform.localPosition.x < 0)
            {
                if (!Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
                {
                    this.isOnWallLeft = false;
                    Debug.Log("LEFT");
                }
            }
            else
            {
                if (!Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
                {
                    Debug.Log("LefT");
                    this.isOnWallLeft = false;
                }
            }
        }
    }

    //////////////////////////////////////////////////
    ////////////////       Helper       //////////////
    //////////////////////////////////////////////////

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
    
    void DashInvinceble()
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
                    DashRayCast(true);
                }
                else if(Input.GetAxisRaw("P1_Horizontal") == -1)
                {
                    //Left
                    DashRayCast(false);
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
                    DashRayCast(true);

                }
                else if (Input.GetAxisRaw("P2_Horizontal") == -1)
                {
                    //Left
                    DashRayCast(false);
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
    // Change Gravity for Jump Balancing!  Edit -> Project Setting -> Physics -> Gravity Y
    void Falling()
    {
        if(this.isOnWallLeft || this.isOnWallRight)
        {
            if(jumps == 0)
                rb.velocity = -Vector3.up * 2;
        }
        else if (rb.velocity.y == 0)
        {
            isFalling = false;
            if (jumps > 0)
                ResetJumps();
        }
        else if (rb.velocity.y < 0)
        {
            isFalling = true;
            rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        }
    }

    void Dash()
    {
        if (isDashing)
        {
            currentDashTime -= Time.deltaTime;
            float travel = currentDashTime / journeyLength;

            this.transform.position = Vector3.Lerp(this.gameObject.transform.localPosition, this.dashEndPoint, travel);
        }
    }

    //////////////////////////////////////////////////
    //////////////// Movement Functions //////////////
    //////////////////////////////////////////////////

    void MoveRight()
    {
        RaycastHit hit;

        if(this.transform.rotation.y != 0)
        {
            this.transform.rotation = new Quaternion(0,0,0,0);
        }
        if (this.gameObject.transform.localPosition.x < 0)
        {
            if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
            {
                //Wallrun
                if(!this.isOnWallLeft && !this.isOnWallRight)
                {
                    this.isOnWallRight = true;
                    ResetJumps();
                }
            }
            else
            {
                this.gameObject.transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
            {
                //Wallrun
                if (!this.isOnWallLeft && !this.isOnWallRight)
                {
                    this.isOnWallRight = true;
                    ResetJumps();
                }
            }
            else
            {
                this.gameObject.transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            }
        }
    }

    void MoveLeft()
    {
        RaycastHit hit;

        if (this.transform.rotation.y != 180)
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        if (this.gameObject.transform.localPosition.x < 0)
        {
            if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
            {
                //Wallrun
                if (!this.isOnWallLeft && !this.isOnWallRight)
                {
                    this.isOnWallLeft = true;
                    ResetJumps();
                }
            }
            else
            {
                this.gameObject.transform.position += Vector3.left * movementSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
            {
                //Wallrun
                if (!this.isOnWallLeft && !this.isOnWallRight)
                {
                    this.isOnWallLeft = true;
                    ResetJumps();
                }
            }
            else
            {
                this.gameObject.transform.position += Vector3.left * movementSpeed * Time.deltaTime;
            }
        }
    }

    void Jump()
    {
        if (this.isOnWallLeft)
        {
            if(this.gameObject.transform.position.x < 0)
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.right * jumpVelocity;
                jumps++;
            }
            else
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.right * jumpVelocity;
                jumps++;
            }
            Debug.Log("Left");
        }
        else if(this.isOnWallRight)
        {
            if (this.gameObject.transform.position.x < 0)
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.left * jumpVelocity;
                jumps++;
            }
            else
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.left * jumpVelocity;
                jumps++;
            }
        }
        else if (jumps < maxJumps)
        {
            this.rb.velocity = Vector3.up * jumpVelocity;
            jumps++;
        }
    }

    void DashStanding()
    {
        myCharacter.canGetDamaged = false;
        Invoke("DashCoolDown", myCharacter.dashTime);
    }

    void DashRayCast(bool directionRight)
    {
        currentDashTime = dashTime;
        isDashing = true;
        RaycastHit hit;
        StartCoroutine(TEST());

        if (directionRight)     //Right
        {
            if (this.gameObject.transform.localPosition.x < 0)
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, dashLength, 9, QueryTriggerInteraction.Ignore))            //WALLHIT
                {
                    Debug.Log(hit.transform.gameObject.name);
                    Debug.DrawLine(this.gameObject.transform.position, hit.point, Color.red, 10f);

                    dashEndPoint = new Vector3(hit.point.x - wallDetectionLength, hit.point.y, hit.point.z);
                }
                else            //NO WALL
                {
                    dashEndPoint = new Vector3(this.gameObject.transform.localPosition.x + dashLength, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
                }
                journeyLength = Vector3.Distance(this.gameObject.transform.localPosition, dashEndPoint);
            }
            else
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, dashLength, 9, QueryTriggerInteraction.Ignore))             //WALLHIT
                {
                    Debug.Log(hit.transform.gameObject.name);
                    Debug.DrawLine(this.gameObject.transform.position, hit.point, Color.red, 10f);

                    dashEndPoint = new Vector3(hit.point.x + wallDetectionLength, hit.point.y, hit.point.z);
                }
                else                // NO WALL
                {
                    dashEndPoint = new Vector3(this.gameObject.transform.localPosition.x + dashLength, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
                }
                journeyLength = Vector3.Distance(this.gameObject.transform.localPosition, dashEndPoint);
            }
        }
        else                //Left
        {
            if (this.gameObject.transform.localPosition.x < 0)
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, dashLength, 9, QueryTriggerInteraction.Ignore))         //WALLHIT
                {
                    Debug.Log(hit.transform.gameObject.name);
                    Debug.DrawLine(this.gameObject.transform.position, hit.point, Color.red, 10f);

                    dashEndPoint = new Vector3(hit.point.x - wallDetectionLength, hit.point.y, hit.point.z);
                }
                else                        // NO WALL
                {
                    dashEndPoint = new Vector3(this.gameObject.transform.localPosition.x - dashLength, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
                }
                journeyLength = Vector3.Distance(this.gameObject.transform.localPosition, dashEndPoint);
            }
            else
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, dashLength, 9, QueryTriggerInteraction.Ignore))        //WALLHIT
                {
                    Debug.Log(hit.transform.gameObject.name);
                    Debug.DrawLine(this.gameObject.transform.position, hit.point, Color.red, 10f);

                    dashEndPoint = new Vector3(hit.point.x + wallDetectionLength, hit.point.y, hit.point.z);
                }
                else                        // NO WALL
                {
                    dashEndPoint = new Vector3(this.gameObject.transform.localPosition.x - dashLength, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
                }
                journeyLength = Vector3.Distance(this.gameObject.transform.localPosition, dashEndPoint);
            }
        }
    }

    IEnumerator TEST()
    {
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
    }
}
