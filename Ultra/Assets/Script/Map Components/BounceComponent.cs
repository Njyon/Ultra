using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BounceComponent : MonoBehaviour
{
    public float xPower, yPower;

    public bool shouldWigle;
    public bool shouldScale;
    public GameObject go;
    Vector3 pos;
    
    [Header("Scale Curve")]
    public AnimationCurve curve;

    public enum BounceType {
        Metal,
        Concrete,
        Stone,
        Rubber,
        Glass,
        MetalGlass,
    }

    [Header("Audio")]
    public BounceType audioMaterial;

    Vector3 normalScale;
    double audioTriggerLast;

    void Start()
    {
        if(shouldWigle)
           pos = go.transform.position;

        if (shouldScale)
            normalScale = go.transform.localScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            Vector3 vel = collision.gameObject.GetComponent<Rigidbody>().velocity;
            collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(vel.x *= xPower, vel.y *= yPower, 0);
            
            if (collision.gameObject.GetComponent<MyCharacter>().isDisabled && shouldWigle)
            {
                StartCoroutine(Wigle());
            }
            else if (shouldScale)
            {
                StartCoroutine(Scale());
            }

            //RaycastHit hit;
            //if (Physics.Linecast(collision.transform.position, this.transform.position, out hit, 9, QueryTriggerInteraction.Ignore))
            //{
            //    Vector3 vel = collision.gameObject.GetComponent<Rigidbody>().velocity;
            //    collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(vel.x *= xPower, vel.y *= yPower, 0);
            //    collision.gameObject.GetComponent<MyCharacter>().SpecialBounce(0.01f);
            //}
        }
    }

    IEnumerator Scale()
    {
        float scaleTime = 1f;
        float currentTime = 0f;
        float scaleMultiplayer = 1.5f;
        float speed = 3f;

        Vector3 scaleUp = new Vector3(normalScale.x * scaleMultiplayer, normalScale.y * scaleMultiplayer, normalScale.z);

        while(scaleTime > currentTime)
        {
            float curvePos = curve.Evaluate(currentTime);
            go.transform.localScale = Vector3.Lerp(normalScale, scaleUp, curvePos);

            currentTime += Time.deltaTime * speed;
            yield return null;
        }

        yield return null;
    }

    IEnumerator Wigle()
    {
        float wigleTime = 0.5f;
        float speed = 5f;
        
        while(wigleTime > 0)
        {

        HandleAudio();

            go.transform.position = new Vector3(pos.x + Mathf.PingPong(Time.time * speed, wigleTime), pos.y, pos.z);

            wigleTime -= Time.deltaTime;
            yield return null;
        }

        go.transform.position = pos;
        yield return null;
    }

    private void HandleAudio() {
        if (AudioSettings.dspTime - audioTriggerLast < 0.1d) {
            return;
        }

        audioTriggerLast = AudioSettings.dspTime;

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
  