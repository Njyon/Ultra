using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turning
{
    bool islookingToTheRight = false;
    bool isTurningRight = false;
    bool isTurningLeft = false;

    //Delegate
    public delegate void EventDelegate(EventState eventState);
    public EventDelegate eventDelegate;

    /// <summary>
    /// Update the class
    /// </summary>
    public void IUpdate(Transform transform)
    {
        TurningLerp(transform);
    }
    /// <summary>
    /// Let the character Turn Right
    /// </summary>
    /// <param name="rotation"></param>
    public void LookRight(Quaternion rotation)
    {
        if (rotation.y != 0)
        {
            this.isTurningRight = true;
            this.islookingToTheRight = true;

            this.isTurningLeft = false;
        }
    }
    /// <summary>
    /// Let the Character Turn Left
    /// </summary>
    /// <param name="rotation"></param>
    public void LookLeft(Quaternion rotation)
    {
        if (rotation.y != 180)
        {
            this.isTurningLeft = true;
            this.islookingToTheRight = false;

            this.isTurningRight = false;
        }
    }
    /// <summary>
    /// Lerp the rotation of the character when he should turn
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    void TurningLerp(Transform transform)
    {
        if (this.isTurningRight)
        {
            if (transform.rotation == new Quaternion(0, 0, 0, 1))
            {
                this.isTurningRight = false;
                return;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, 0, 0, 1), 0.2f);
        }
        else if (this.isTurningLeft)
        {
            if (transform.rotation == new Quaternion(0, 1, 0, 0))
            {
                this.isTurningLeft = false;
                return;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, 1, 0, 0), 0.2f);
        }
    }
}