using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScipt : MonoBehaviour
{
    [Header("Type of the Collision")] public CollisionEnum collisionEnum;
    [Header("Character")] public GameObject player;
    MyCharacter myCharacter;

    private void Awake()
    {
        myCharacter = player.GetComponent<MyCharacter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            if (myCharacter.enemy == null)
                myCharacter.enemy = other.gameObject;

            switch(collisionEnum)
            {
                case CollisionEnum.XHitNormal:
                    myCharacter.XNormalHitBox = true;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            switch (collisionEnum)
            {
                case CollisionEnum.XHitNormal:
                    myCharacter.XNormalHitBox = false;
                    break;
            }
        }
    }
}
