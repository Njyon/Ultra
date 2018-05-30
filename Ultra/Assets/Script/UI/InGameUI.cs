using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] Text life;
    [SerializeField] Text prozent;

    public void ChangeProzent(int prozent)
    {
        this.prozent.text = prozent.ToString() + "%";
    }
    public void ChangeLife(int life)
    {
        this.life.text = "Life: " + life.ToString();
    }
}