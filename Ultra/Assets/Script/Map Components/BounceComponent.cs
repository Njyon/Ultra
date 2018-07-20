using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BounceComponent : MonoBehaviour
{
    public float xPower, yPower;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            RaycastHit hit;
            if (Physics.Linecast(collision.transform.position, this.transform.position, out hit, 9, QueryTriggerInteraction.Ignore))
            {
                Vector3 vel = collision.gameObject.GetComponent<Rigidbody>().velocity;
                collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(vel.x *= xPower, vel.y *= yPower, 0);
                collision.gameObject.GetComponent<MyCharacter>().SpecialBounce(0.01f);
            }
        }
    }
    
}
  