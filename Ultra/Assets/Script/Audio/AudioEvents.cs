using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvents : MonoBehaviour {
    private float yVelocity = 0;
    private Movement movement;
    private MyCharacter character;
    private delegate void EventDelegate(EventState eventState);
    private EventDelegate eventDelegate;
    [HideInInspector] public static bool firstGetHit = false;

    public  delegate void ActionTimer();
    public static ActionTimer actionTimer;
   

    void Awake () {
        movement = gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Movement>();
        character = gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<MyCharacter>();
    }

    private void OnEnable() {
        character.eventDelegate += EventCheck;
        movement.eventDelegate += EventCheck;
        eventDelegate += EventCheck;
    }

    private void OnDisable() {
        character.eventDelegate -= EventCheck;
        movement.eventDelegate -= EventCheck;
        eventDelegate -= EventCheck;
    }

    void EventCheck(EventState eventState) {
        switch (eventState) {
            case EventState.isDisabled:
                break;
            case EventState.EndDisabled:
                break;
            case EventState.Idle:
                break;
            case EventState.Walking:
                break;
            case EventState.JumpOnGround:
                Fabric.EventManager.Instance.PostEvent("Jump1", this.gameObject);
                break;
            case EventState.JumpInAir:
                Fabric.EventManager.Instance.PostEvent("Jump2", this.gameObject);
                break;
            case EventState.JumpInAir2:
                Fabric.EventManager.Instance.PostEvent("Jump3", this.gameObject);
                break;
            case EventState.Fall:
                break;
            case EventState.Landing:
                var rand = Random.Range(0f, 1f);
                Fabric.EventManager.Instance.SetParameter("Landing", "LandingImpact", yVelocity, this.gameObject);
                Fabric.EventManager.Instance.PostEvent("Landing", this.gameObject);
                break;
            case EventState.Dodge:
                Fabric.EventManager.Instance.PostEvent("DodgeStart", this.gameObject);
                break;
            case EventState.OnWall:
                break;
            case EventState.AttackUp:
                Fabric.EventManager.Instance.PostEvent("DashUp", this.gameObject);
                break;
            case EventState.AttackUpAngled:
                Fabric.EventManager.Instance.PostEvent("DashUp", this.gameObject);
                break;
            case EventState.AttackSide:
                Fabric.EventManager.Instance.PostEvent("DashSide", this.gameObject);
                break;
            case EventState.AttackDownAngled:
                Fabric.EventManager.Instance.PostEvent("DashDown", this.gameObject);
                break;
            case EventState.AttackDown:
                Fabric.EventManager.Instance.PostEvent("DashDown", this.gameObject);
                break;
            case EventState.Slash:
                Fabric.EventManager.Instance.PostEvent("DashStab", this.gameObject);
                break;
            case EventState.Bounce:
                Fabric.EventManager.Instance.PostEvent("Bounce", this.gameObject);
                break;
            case EventState.Parry:
                Fabric.EventManager.Instance.PostEvent("Parry", this.gameObject);
                break;
            case EventState.GetHit:
                if (firstGetHit == false)
                {
                    firstGetHit = true;
                    Fabric.EventManager.Instance.PostEvent("MusicAction");
                }
                if(actionTimer != null)
                    actionTimer();
                break;
            case EventState.isFalling:
                break;
            case EventState.ChangeDirectionLeft:
                break;
            case EventState.ChangeDirectionRight:
                break;
            case EventState.AttackEnd:
                break;
            case EventState.AttackHit:
                break;
            case EventState.ResetDashes:
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (movement.fallComp == null)
            return;

        if(movement.fallComp.isFalling)
        {
            yVelocity = Mathf.Abs(movement.rb.velocity.y) / 32;
        }
    }

    public void Footstep() {
        Fabric.EventManager.Instance.PostEvent("Footsteps", this.gameObject);
    }
}
