using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TELEPORT : MonoBehaviour
{
    void OnEnable()
    {
        DELEGATE.OnClickAction += Teleport;
        DELEGATE.OnClickAction += TurnColor;
    }
    void OnDisable()
    {
        DELEGATE.OnClickAction -= Teleport;
        DELEGATE.OnClickAction -= TurnColor;
    }

    void Teleport()
    {
        Vector3 pos = transform.position;
        pos.y = Random.Range(1.0f, 3.0f);
        transform.position = pos;
    }

    void TurnColor()
    {
        Color col = new Color(Random.value, Random.value, Random.value);
        GetComponent<Renderer>().material.color = col;
    }
}
