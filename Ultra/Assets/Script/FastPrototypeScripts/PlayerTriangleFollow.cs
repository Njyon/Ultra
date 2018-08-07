using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTriangleFollow : MonoBehaviour
{
    private Image triangle;

    //public Sprite p1;
    //public Sprite p2;

    public void SetPlayer(PlayerEnum pE)
    {
        switch (pE)
        {
            case PlayerEnum.PlayerOne:
                triangle = GameObject.Find("pI_p1").GetComponent<Image>();
                break;
            case PlayerEnum.PlayerTwo:
                triangle = GameObject.Find("pI_p2").GetComponent<Image>();
                break;
        }
    }


    public void ColorTriangle(Color color)
    {
        triangle.color = color;
    }

    void Update()
    {
        Vector3 imagePos = Camera.main.WorldToScreenPoint(this.transform.position);
        triangle.transform.position = imagePos;
    }
}
