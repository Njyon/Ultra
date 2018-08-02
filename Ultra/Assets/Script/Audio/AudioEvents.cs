using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvents : MonoBehaviour {
    private float yVelocity = 0;
    private Movement movement;
    private MyCharacter character;
    private delegate void EventDelegate(EventState eventState);
    private EventDelegate eventDelegate;
    private double bounceAudioLastTrigger;

    public enum BounceType {
        Metal,
        Concrete,
        Stone,
        Rubber,
        Glass,
        MetalGlass,
    }

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
                Debug.Log("Velocity: " + yVelocity);
                break;
            case EventState.Dodge:
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
                break;
            case EventState.GetHit:
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

    void Update() {
        if(movement.fallComp.isFalling) {
            yVelocity = Mathf.Abs(movement.rb.velocity.y)/32;
        }
    }

    public void Footstep() {
        Fabric.EventManager.Instance.PostEvent("Footsteps", this.gameObject);
    }

    public void Bounce(BounceType audioMaterial) {
        if (AudioSettings.dspTime - bounceAudioLastTrigger < 0.1d) {
            return;
        }

        bounceAudioLastTrigger = AudioSettings.dspTime;

        switch (audioMaterial) {
            case BounceType.Metal:
                Fabric.EventManager.Instance.PostEvent("ParticleMetal", this.gameObject);
                break;
            case BounceType.Concrete:
                Fabric.EventManager.Instance.PostEvent("ParticleRocks", this.gameObject);
                break;
            case BounceType.Stone:
                Fabric.EventManager.Instance.PostEvent("ParticleStones", this.gameObject);
                break;
            case BounceType.Rubber:
                Fabric.EventManager.Instance.PostEvent("RubberPitch", this.gameObject);
                Fabric.EventManager.Instance.PostEvent("RubberNoPitch", this.gameObject);
                break;
            case BounceType.Glass:
                Fabric.EventManager.Instance.PostEvent("ParticleGlass", this.gameObject);
                break;
            case BounceType.MetalGlass:
                Fabric.EventManager.Instance.PostEvent("ParticleGlass", this.gameObject);
                Fabric.EventManager.Instance.PostEvent("ParticleMetal", this.gameObject);
                break;
            default:
                break;
        }
    }
}
