using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRayCast 
{
    /// <summary>
    /// Returns a position to the right from Camera Perspective
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector3 RaycastRight(Vector3 charPosition, float length)
    {
        RaycastHit hit;

        // Check if the Character is under or above 0
        if (charPosition.x < 0)
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y + 0.5f, 0);
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return hit point - offest as Destination
                    return new Vector3(hit.point.x, hit.point.y - 0.5f, 0);
                }
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x + length, charPosition.y, 0);
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y + 0.5f, 0);
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return hit point - offest as Destination
                    return new Vector3(hit.point.x, hit.point.y - 0.5f, 0);
                }
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x + length, charPosition.y, 0);
            }
        }
    }
    /// <summary>
    /// Returns a position to the left from Camera Perspective
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector3 RaycastLeft(Vector3 charPosition, float length)
    {
        RaycastHit hit;

        // Check if the Character is under or above 0
        if (charPosition.x < 0)
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y + 0.5f, 0);
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return hit point - offest as Destination
                    return new Vector3(hit.point.x, hit.point.y - 0.5f, 0);
                }
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x - length, charPosition.y, 0);
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y + 0.5f, 0);
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return hit point - offest as Destination
                    return new Vector3(hit.point.x, hit.point.y - 0.5f, 0);
                }
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x - length, charPosition.y, 0);
            }
        }
    }
    /// <summary>
    /// Returns a position up from Camera Perspective
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector3 RaycastUp(Vector3 charPosition, float length)
    {
        RaycastHit hit;

        // Check if the Character is under or above 0
        if (charPosition.y < 0)
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(charPosition, new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y, 0);
            }
            else
            {
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x, charPosition.y + length, 0);
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(charPosition, new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y, 0);
            }
            else
            {
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x, charPosition.y + length, 0);
            }
        }
    }
    /// <summary>
    /// Returns a position down from Camera Perspective
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector3 RaycastDown(Vector3 charPosition, float length)
    {
        RaycastHit hit;

        // Check if the Character is under or above 0
        if (charPosition.y < 0)
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(charPosition, new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y, 0);
            }
            else
            {
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x, charPosition.y - length, 0);
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(charPosition, new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y, 0);
            }
            else
            {
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x, charPosition.y - length, 0);
            }
        }
    }

    /// <summary>
    /// Check if there is an Obisticle to the Right
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool RayCastHitRight(Vector3 charPosition, float length, out RaycastHit hit)
    {
        // Check if the Character is under or above 0
        if (charPosition.x < 0)
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit true
                    return true;
                }
                // if no Obsticle found return false
                return false;
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return true
                    return true;
                }
                // if no Obsticle found return false
                return false;
            }
        }
    }
    /// <summary>
    /// Check if there is an Obisticle to the Left
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool RayCastHitLeft(Vector3 charPosition, float length, out RaycastHit hit)
    {
        // Check if the Character is under or above 0
        if (charPosition.x < 0)
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit true
                    return true;
                }
                // if no Obsticle found return false
                return false;
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return true
                    return true;
                }
                // if no Obsticle found return false
                return false;
            }
        }
    }
    /// <summary>
    /// Check if there is an Obisticle above
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool RayCastHitUp(Vector3 charPosition, float length, out RaycastHit hit)
    {
        // Check if the Character is under or above 0
        if (charPosition.y < 0)
        {
            if (Physics.Raycast(charPosition, new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // if no Obsticle found return false
                return false;
            }
        }
        else
        {
            if (Physics.Raycast(charPosition, new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // if no Obsticle found return false
                return false;
            }
        }
    }
    /// <summary>
    /// Check if there is an Obisticle below
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    public static bool RayCastHitDown(Vector3 charPosition, float length, out RaycastHit hit)
    {
        // Check if the Character is under or above 0
        if (charPosition.y < 0)
        {
            if (Physics.Raycast(charPosition, new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // if no Obsticle found return false
                return false;
            }
        }
        else
        {
            if (Physics.Raycast(charPosition, new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // if no Obsticle found return false
                return false;
            }
        }
    }
    /// <summary>
    /// Check if there is an Obisticle to the Right
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static bool RayCastHitRight(Vector3 charPosition, float length)
    {
        RaycastHit hit;
        // Check if the Character is under or above 0
        if (charPosition.x < 0)
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit true
                    return true;
                }
                // if no Obsticle found return false
                return false;
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return true
                    return true;
                }
                // if no Obsticle found return false
                return false;
            }
        }
    }
    /// <summary>
    /// Check if there is an Obisticle to the Left
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static bool RayCastHitLeft(Vector3 charPosition, float length)
    {
        RaycastHit hit;
        // Check if the Character is under or above 0
        if (charPosition.x < 0)
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit true
                    return true;
                }
                // if no Obsticle found return false
                return false;
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + 0.5f, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return true
                    return true;
                }
                // if no Obsticle found return false
                return false;
            }
        }
    }
    /// <summary>
    /// Check if there is an Obisticle above
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static bool RayCastHitUp(Vector3 charPosition, float length)
    {
        RaycastHit hit;
        // Check if the Character is under or above 0
        if (charPosition.y < 0)
        {
            if (Physics.Raycast(charPosition, new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // if no Obsticle found return false
                return false;
            }
        }
        else
        {
            if (Physics.Raycast(charPosition, new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // if no Obsticle found return false
                return false;
            }
        }
    }
    /// <summary>
    /// Check if there is an Obisticle below
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static bool RayCastHitDown(Vector3 charPosition, float length)
    {
        RaycastHit hit;
        // Check if the Character is under or above 0
        if (charPosition.y < 0)
        {
            if (Physics.Raycast(charPosition, new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // if no Obsticle found return false
                return false;
            }
        }
        else
        {
            if (Physics.Raycast(charPosition, new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return true
                return true;
            }
            else
            {
                // if no Obsticle found return false
                return false;
            }
        }
    }
}
  