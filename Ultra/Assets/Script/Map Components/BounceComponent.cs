using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BounceComponent : MonoBehaviour
{
    public float power;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            RaycastHit hit;
            if (Physics.Linecast(collision.transform.position, this.transform.position, out hit, 9, QueryTriggerInteraction.Ignore))
            {
                collision.gameObject.GetComponent<Rigidbody>().velocity *= power;
                collision.gameObject.GetComponent<MyCharacter>().SpecialBounce(0.1f);
            }
        }
    }
    
}
  