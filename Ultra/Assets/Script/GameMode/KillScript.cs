using UnityEngine;

public class KillScript : MonoBehaviour
{
    //Deprecated
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            //other.GetComponent<MyCharacter>().Respawn();
        }
    }
}