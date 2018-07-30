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

    Vector3 normalScale;

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

        Vector3 scaleUp = new Vector3(go.transform.localScale.x * scaleMultiplayer, go.transform.localScale.y * scaleMultiplayer, go.transform.localScale.z);

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
            go.transform.position = new Vector3(pos.x + Mathf.PingPong(Time.time * speed, wigleTime), pos.y, pos.z);

            wigleTime -= Time.deltaTime;
            yield return null;
        }

        go.transform.position = pos;
        yield return null;
    }
}
  