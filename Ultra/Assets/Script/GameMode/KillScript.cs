using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            other.GetComponent<MyCharacter>().Respawn();
        }
    }
}