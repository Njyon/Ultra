using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using System.Collections;

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

    [Header("Camera")]
    public Camera cam;

    /// <summary>
    /// Gets modified by Move() -> SmoothDamp()
    /// </summary>
    Vector3 vel;
    List<Transform> targets = new List<Transform>();
    
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

    public void Shake(bool isHeavy)
    {
        if(isHeavy)
            CameraShaker.Instance.ShakeOnce(0.7f, 11f, 0.3f, 0.7f);
        else
            CameraShaker.Instance.ShakeOnce(0.4f, 10f, 0.1f, 0.4f);
            
        //StartCoroutine(IShake());
    }

    IEnumerator IShake()
    {
        float time = 0;
        float maxTime = 1;
        float magnitude = 2;
        Vector3 pos = transform.position;

        while (time < maxTime)
        {
            float x = Random.Range(-1, 1f) * magnitude;
            float y = Random.Range(-1, 1f) * magnitude;

            transform.position = new Vector3(pos.x + x, pos.y + y, pos.z);

            time += Time.deltaTime;

            //CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
            yield return null;
        }

        transform.position = pos;
        yield return null;
    }
}