using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType {
    Hit,
}
public class AudioParticleSystemEvents : MonoBehaviour {
    public ParticleType type;

	// Use this for initialization
	void Start () {
        switch (type) {
            case ParticleType.Hit:
                Fabric.EventManager.Instance.PostEvent("HitVFX", this.gameObject);
                break;
            default:
                break;
        }
    }
}
