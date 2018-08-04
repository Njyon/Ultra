using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BounceType {
    Metal,
    Concrete,
    Stone,
    Rubber,
    Glass,
    MetalGlass,
}

public class AudioMaterialBounces : MonoBehaviour {

    private double bounceAudioLastTrigger;

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
