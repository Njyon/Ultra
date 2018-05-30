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
            if (myCharacter.enemy == null)
                myCharacter.enemy = other.gameObject;
            if (myCharacter.enemyCharacter == null)
                myCharacter.enemyCharacter = other.gameObject.GetComponent<MyCharacter>();

            switch(collisionEnum)
            {
                case CollisionEnum.XHitNormal:
                    myCharacter.xNormalHitBox = true;
                    break;
                case CollisionEnum.XHitUp:
                    myCharacter.xUpHitBox = true;
                    break;
                case CollisionEnum.XHitDown:
                    myCharacter.xDownHitBox = true;
                    break;
                case CollisionEnum.YBHitUp:
                    myCharacter.sUpHitBox = true;
                    break;
                case CollisionEnum.YBHitDown:
                    myCharacter.sDownHitBox = true;
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
                case CollisionEnum.XHitUp:
                    myCharacter.xUpHitBox = false;
                    break;
                case CollisionEnum.XHitDown:
                    myCharacter.xDownHitBox = false;
                    break;
                case CollisionEnum.YBHitUp:
                    myCharacter.sUpHitBox = false;
                    break;
                case CollisionEnum.YBHitDown:
                    myCharacter.sDownHitBox = false;
                    break;
            }
        }
        else if (other.tag == "ScrollButton")
        {
            myCharacter.hasSlider = false;
        }
    }
}
