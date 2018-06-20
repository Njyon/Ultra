using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{
    [Header("Distance of the Camera")]
    [SerializeField] Vector3 offset;

    [Header("Time of the smooth")]
    [SerializeField] float smoothTime;

    [Header("Zoom")]
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;
    [SerializeField] float zoomLimiter;
    
    /// <summary>
    /// Gets modified by Move() -> SmoothDamp()
    /// </summary>
    Vector3 vel;
    List<Transform> targets = new List<Transform>();
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>(); 
    }
    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
        Zoom();
    }
    /// <summary>
    /// Changes the Field of View
    /// </summary>
    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
    /// <summary>
    /// Get the greatest distance between the targets
    /// </summary>
    /// <returns></returns>
    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        float size;
        if(bounds.size.x >= bounds.size.y)
        {
            size = bounds.size.x;
        }
        else
        {
            size = bounds.size.y;
        }

        return size;
    }
    /// <summary>
    /// Moves the Camera Between both targets
    /// </summary>
    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;

        //Loot At Tartget Smooth
        Quaternion rot = Quaternion.LookRotation(centerPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 6);
        
        //transform.LookAt(centerPoint);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref vel, smoothTime);
    }
    /// <summary>
    /// return a Vector3 what is the center of a bound from all targets
    /// </summary>
    /// <returns></returns>
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    /// <summary>
    /// Add a new Target
    /// </summary>
    /// <param name="t"></param>
    public void AddTarget(Transform t)
    {
        targets.Add(t);
    }

}