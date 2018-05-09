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
        if(other.tag == "player" && !other.isTrigger)
        {
            Debug.Log(other.name);
            if (myCharacter.enemy == null)
                myCharacter.enemy = other.gameObject;
            if (myCharacter.enemyCharacter == null)
                myCharacter.enemyCharacter = other.gameObject.GetComponent<MyCharacter>();

            switch(collisionEnum)
            {
                case CollisionEnum.XHitNormal:
                    myCharacter.xNormalHitBox = true;
                    break;
            }
        }
        else if (other.tag == "ScrollButton")
        {
            myCharacter.hasSlider = true;
            myCharacter.slider = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            switch (collisionEnum)
            {
                case CollisionEnum.XHitNormal:
                    myCharacter.xNormalHitBox = false;
                    break;
            }
        }
        else if (other.tag == "ScrollButton")
        {
            myCharacter.hasSlider = false;
        }
    }
}
