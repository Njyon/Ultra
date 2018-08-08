using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailer_FocusCam : MonoBehaviour
{
    public MultiTargetCamera cam;
    public Transform go;
    public Transform go2;

    private void Awake()
    {
        cam.AddTarget(go);
        cam.AddTarget(go2);
    }
}
