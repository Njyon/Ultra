using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BounceComponent : MonoBehaviour
{
    public float xPower, yPower;

    public bool shouldWigle;
    public GameObject go;
    Vector3 pos;

    void Start()
    {
        if(shouldWigle)
           pos = go.transform.position;    
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            Vector3 vel = collision.gameObject.GetComponent<Rigidbody>().velocity;
            collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(vel.x *= xPower, vel.y *= yPower, 0);

            Debug.Log("hmm");
            if (collision.gameObject.GetComponent<MyCharacter>().isDisabled && shouldWigle)
            {
                Debug.Log("lol");
                StartCoroutine(Wigle());
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

    IEnumerator Wigle()
    {
        float wigleTime = 3f;
        float speed = 10f;

        Debug.Log("Start");
        while(wigleTime > 0)
        {
            Debug.Log("Wigle");
            go.transform.position = new Vector3(pos.x + Mathf.PingPong(Time.time * speed, wigleTime), pos.y, pos.z);

            wigleTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("End");

        go.transform.position = pos;
        yield return null;
    }
}
  