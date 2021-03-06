﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyRayCast
{
    static float head = 0.5f;
    static float feed = 0.5f;
    static float with = 0.1f;

    static float AngelLength(float length)
    {
        float pow = Mathf.Pow(length, 2);
        return Mathf.Sqrt(pow + pow);
    }

    public static bool RayCastInDirection(Vector3 position, Vector3 direction, out RaycastHit hit, float length)
    {
        RaycastHit coreHit;
        RaycastHit headHit;
        RaycastHit feedHit;

        float coreDis = 100;
        float headDis = 100;
        float feedDis = 100;

        bool didHit = false;

        if (Physics.Raycast(position, direction, out coreHit, length, 9, QueryTriggerInteraction.Ignore))
        {
            coreDis = Vector3.Distance(position, coreHit.point);
            didHit = true;
        }
        if (Physics.Raycast(new Vector3(position.x, position.y + head, 0), direction, out headHit, length, 9, QueryTriggerInteraction.Ignore))
        {
            headDis = Vector3.Distance(new Vector3(position.x, position.y + head, 0), coreHit.point);
            didHit = true;
        }
        if (Physics.Raycast(new Vector3(position.x, position.y - feed, 0), direction, out feedHit, length, 9, QueryTriggerInteraction.Ignore))
        {
            feedDis = Vector3.Distance(new Vector3(position.x, position.y - feed, 0), coreHit.point);
            didHit = true;
        }


        if (coreDis < headDis && coreDis < feedDis)
        {
            hit = coreHit;
        }
        else if(headDis < coreDis && headDis < feedDis)
        {
            hit = headHit;
        }
        else if(feedDis < coreDis && feedDis < headDis)
        {
            hit = feedHit;
        }
        else
        {
            hit = new RaycastHit();
        }

        return didHit;          
    }

    /// <summary>
    /// returns a Position thats in front of the Character at an angle 45° Up
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector3 RayCastUpAngeled(Transform charPosition, float length, bool right)
    {
        RaycastHit headHit;
        RaycastHit coreHit;
        RaycastHit feedHit;

        bool didHit = false;

        // Check if the Player would hit an obstical with his body
        if (Physics.Raycast(new Vector3(charPosition.position.x, charPosition.position.y + head, 0),charPosition.right + charPosition.up, out headHit, length + 1f, 9, QueryTriggerInteraction.Ignore))
        {
            didHit = true;
        }
        if (Physics.Raycast(new Vector3(charPosition.position.x, charPosition.position.y - feed, 0), charPosition.right + charPosition.up, out feedHit, length + 1f, 9, QueryTriggerInteraction.Ignore))
        {
            didHit = true;
        }
        if (Physics.Raycast(new Vector3(charPosition.position.x, charPosition.position.y, 0), charPosition.right + charPosition.up, out coreHit, length + 1f, 9, QueryTriggerInteraction.Ignore))
        {
            didHit = true;
        }

        // Calc the destination if no Obstical found
        if (!didHit)
        {
            if (right)
            {
                float a = length * Mathf.Cos(45);
                float b = Mathf.Sqrt(Mathf.Pow(length, 2) - Mathf.Pow(a, 2));
                return new Vector3(charPosition.position.x + b, charPosition.position.y + a, 0);
            }
            else
            {
                float a = length * Mathf.Cos(45);
                float b = Mathf.Sqrt(Mathf.Pow(length, 2) - Mathf.Pow(a, 2));
                return new Vector3(charPosition.position.x - b, charPosition.position.y + a, 0);
            }
        }
        else   // Calc the destination if character collides
        {
            float distanceHead = Vector3.Distance(charPosition.position, headHit.point);
            float distancecore = Vector3.Distance(charPosition.position, coreHit.point);
            float distancefeed = Vector3.Distance(charPosition.position, feedHit.point);

            if(!right)
            {
                // Check witch distance is the shortest
                if (distanceHead <= distancecore && distanceHead <= distancefeed)
                {
                    if (charPosition.position.x < headHit.point.x)
                    {
                        return new Vector3(headHit.point.x + 0.5f, headHit.point.y - head, 0);
                    }
                    else if (charPosition.position.x > headHit.point.x)
                    {
                        return new Vector3(headHit.point.x - 0.5f, headHit.point.y - head, 0);
                    }
                    else
                    {
                        return new Vector3(headHit.point.x, headHit.point.y - head, 0);
                    }
                }
                else if (distancecore <= distanceHead && distancecore <= distancefeed)
                {
                    if (charPosition.position.x < coreHit.point.x)
                    {
                        return new Vector3(coreHit.point.x + 0.5f, coreHit.point.y, 0);
                    }
                    else if (charPosition.position.x > coreHit.point.x)
                    {
                        return new Vector3(coreHit.point.x - 0.5f, coreHit.point.y, 0);
                    }
                    else
                    {
                        return coreHit.point;
                    }
                }
                else
                {
                    if (distancefeed <= distanceHead && distancefeed <= distancecore)
                    {
                        return new Vector3(feedHit.point.x + 0.5f, feedHit.point.y + feed, 0);
                    }
                    else if (charPosition.position.x > feedHit.point.x)
                    {
                        return new Vector3(feedHit.point.x - 0.5f, feedHit.point.y + feed, 0);
                    }
                    else
                    {
                        return new Vector3(feedHit.point.x, feedHit.point.y + feed, 0);
                    }
                }
            }
            else
            {
                // Check witch distance is the shortest
                if (distanceHead <= distancecore && distanceHead <= distancefeed)
                {
                    if (charPosition.position.x < headHit.point.x)
                    {
                        return new Vector3(headHit.point.x - 0.5f, headHit.point.y - head, 0);
                    }
                    else if (charPosition.position.x > headHit.point.x)
                    {
                        return new Vector3(headHit.point.x + 0.5f, headHit.point.y - head, 0);
                    }
                    else
                    {
                        return new Vector3(headHit.point.x, headHit.point.y - head, 0);
                    }
                }
                else if (distancecore <= distanceHead && distancecore <= distancefeed)
                {
                    if (charPosition.position.x < coreHit.point.x)
                    {
                        return new Vector3(coreHit.point.x - 0.5f, coreHit.point.y, 0);
                    }
                    else if (charPosition.position.x > coreHit.point.x)
                    {
                        return new Vector3(coreHit.point.x + 0.5f, coreHit.point.y, 0);
                    }
                    else
                    {
                        return coreHit.point;
                    }
                }
                else
                {
                    if (distancefeed <= distanceHead && distancefeed <= distancecore)
                    {
                        return new Vector3(feedHit.point.x - 0.5f, feedHit.point.y + feed, 0);
                    }
                    else if (charPosition.position.x > feedHit.point.x)
                    {
                        return new Vector3(feedHit.point.x + 0.5f, feedHit.point.y + feed, 0);
                    }
                    else
                    {
                        return new Vector3(feedHit.point.x, feedHit.point.y + feed, 0);
                    }
                }
            }

            
        } 
    }
    /// <summary>
    /// returns a Position thats in front of the Character at an angle 45° Down
    /// </summary>
    /// <param name="charPosition"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector3 RayCastDownAngeled(Transform charPosition, float length, bool right)
    {
        RaycastHit headHit;
        RaycastHit coreHit;
        RaycastHit feedHit;

        bool didHit = false;

        if (Physics.Raycast(new Vector3(charPosition.position.x, charPosition.position.y, 0), charPosition.right + -charPosition.up, out coreHit, length + 1f, 9, QueryTriggerInteraction.Ignore))
        {
            didHit = true;
        }
        if (Physics.Raycast(new Vector3(charPosition.position.x, charPosition.position.y - feed, 0), charPosition.right + -charPosition.up, out feedHit, length + 1f, 9, QueryTriggerInteraction.Ignore))
        {
            didHit = true;
        }    
        if (Physics.Raycast(new Vector3(charPosition.position.x, charPosition.position.y + head, 0), charPosition.right + -charPosition.up, out headHit, length + 1f, 9, QueryTriggerInteraction.Ignore))
        {
            didHit = true;
        }
        
        if(!didHit)
        {
            if (right)
            {
                float a = length * Mathf.Cos(45);
                float b = Mathf.Sqrt(Mathf.Pow(length, 2) - Mathf.Pow(a, 2));
                return new Vector3(charPosition.position.x + b, charPosition.position.y - a, 0);
            }
            else
            {
                float a = length * Mathf.Cos(45);
                float b = Mathf.Sqrt(Mathf.Pow(length, 2) - Mathf.Pow(a, 2));
                return new Vector3(charPosition.position.x - b, charPosition.position.y - a, 0);
            }
        }
        else
        {
            float distanceHead = Vector3.Distance(charPosition.position, headHit.point);
            float distancecore = Vector3.Distance(charPosition.position, coreHit.point);
            float distancefeed = Vector3.Distance(charPosition.position, feedHit.point);

            if (!right)
            {
                // Check witch distance is the shortest
                if (distanceHead <= distancecore && distanceHead <= distancefeed)
                {
                    if (charPosition.position.x < headHit.point.x)
                    {
                        return new Vector3(headHit.point.x - 0.5f, headHit.point.y - head, 0);
                    }
                    else if (charPosition.position.x > headHit.point.x)
                    {
                        return new Vector3(headHit.point.x + 0.5f, headHit.point.y - head, 0);
                    }
                    else
                    {
                        return new Vector3(headHit.point.x, headHit.point.y - head, 0);
                    }
                }
                else if (distancecore <= distanceHead && distancecore <= distancefeed)
                {
                    if (charPosition.position.x < coreHit.point.x)
                    {
                        return new Vector3(coreHit.point.x - 0.5f, coreHit.point.y, 0);
                    }
                    else if (charPosition.position.x > coreHit.point.x)
                    {
                        return new Vector3(coreHit.point.x + 0.5f, coreHit.point.y, 0);
                    }
                    else
                    {
                        return coreHit.point;
                    }
                }
                else
                {
                    if (distancefeed <= distanceHead && distancefeed <= distancecore)
                    {
                        return new Vector3(feedHit.point.x - 0.5f, feedHit.point.y + feed, 0);
                    }
                    else if (charPosition.position.x > feedHit.point.x)
                    {
                        return new Vector3(feedHit.point.x + 0.5f, feedHit.point.y + feed, 0);
                    }
                    else
                    {
                        return new Vector3(feedHit.point.x, feedHit.point.y + feed, 0);
                    }
                }
            }
            else
            {
                // Check witch distance is the shortest
                if (distanceHead <= distancecore && distanceHead <= distancefeed)
                {
                    if (charPosition.position.x < headHit.point.x)
                    {
                        return new Vector3(headHit.point.x - 0.5f, headHit.point.y - head, 0);
                    }
                    else if (charPosition.position.x > headHit.point.x)
                    {
                        return new Vector3(headHit.point.x + 0.5f, headHit.point.y - head, 0);
                    }
                    else
                    {
                        return new Vector3(headHit.point.x, headHit.point.y - head, 0);
                    }
                }
                else if (distancecore <= distanceHead && distancecore <= distancefeed)
                {
                    if (charPosition.position.x < coreHit.point.x)
                    {
                        return new Vector3(coreHit.point.x - 0.5f, coreHit.point.y, 0);
                    }
                    else if (charPosition.position.x > coreHit.point.x)
                    {
                        return new Vector3(coreHit.point.x + 0.5f, coreHit.point.y, 0);
                    }
                    else
                    {
                        return coreHit.point;
                    }
                }
                else
                {
                    if (distancefeed <= distanceHead && distancefeed <= distancecore)
                    {
                        return new Vector3(feedHit.point.x - 0.5f, feedHit.point.y + feed, 0);
                    }
                    else if (charPosition.position.x > feedHit.point.x)
                    {
                        return new Vector3(feedHit.point.x + 0.5f, feedHit.point.y + feed, 0);
                    }
                    else
                    {
                        return new Vector3(feedHit.point.x, feedHit.point.y + feed, 0);
                    }
                }
            }
        }
    }

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
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x - 0.5f, hit.point.y + feed, 0);
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return hit point - offest as Destination
                    return new Vector3(hit.point.x - 0.5f, hit.point.y - head, 0);
                }
                else
                {
                    // Check the Middle of the body for an Obsticle
                    if (Physics.Raycast(charPosition, new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                    {
                        // if an Obsticle hit return hit point - offest as Destination
                        return new Vector3(hit.point.x, hit.point.y, 0);
                    }
                }
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x + length, charPosition.y, 0);
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x - 0.5f, hit.point.y + feed, 0);
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return hit point - offest as Destination
                    return new Vector3(hit.point.x - 0.5f, hit.point.y - head, 0);
                }
                else
                {
                    // Check the Middle of the body for an Obsticle
                    if (Physics.Raycast(charPosition, new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                    {
                        // if an Obsticle hit return hit point - offest as Destination
                        return new Vector3(hit.point.x - 0.5f, hit.point.y, 0);
                    }
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
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x + 0.5f, hit.point.y + feed, 0);
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return hit point - offest as Destination
                    return new Vector3(hit.point.x + 0.5f, hit.point.y - head, 0);
                }
                else
                {
                    // Cast a Ray at the Middle of the Body and check for an Obsticle
                    if (Physics.Raycast(charPosition, new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                    {
                        // if an Obsticle hit return hit point - offest as Destination
                        return new Vector3(hit.point.x + 0.5f, hit.point.y, 0);
                    }
                }
                // if no Obsticle found return Destination, Position + Lenght
                return new Vector3(charPosition.x - length, charPosition.y, 0);
            }
        }
        else
        {
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x + 0.5f, hit.point.y + feed, 0);
            }
            else
            {
                // Cast a Ray at the head and Check for an Obsticle
                if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                {
                    // if an Obsticle hit return hit point - offest as Destination
                    return new Vector3(hit.point.x + 0.5f, hit.point.y - head, 0);
                }
                else
                {
                    // Cast a Ray at the Middle of the Body and check for an Obsticle
                    if (Physics.Raycast(charPosition, new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
                    {
                        // if an Obsticle hit return hit point - offest as Destination
                        return new Vector3(hit.point.x + 0.5f, hit.point.y, 0);
                    }
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
            if (Physics.Raycast(charPosition, new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y + 0.5f , 0);
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
            if (Physics.Raycast(charPosition, new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y - 0.5f, 0);
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
            if (Physics.Raycast(charPosition, new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y - 0.5f, 0);
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
            if (Physics.Raycast(charPosition, new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                // if an Obsticle hit return hit point + offest as Destination
                return new Vector3(hit.point.x, hit.point.y + 0.5f, 0);
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
            // Cast a Ray at the feed, Middle and Head and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore)
                )
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
            // Cast a Ray at the feed, Middle and Head and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore)
                )
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
            // Cast a Ray at the feed, Middle and Head and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore)
                )
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
            // Cast a Ray at the feed, Middle and Head and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore)
                )
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
            if (Physics.Raycast(new Vector3(charPosition.x + with, charPosition.y + head, 0), new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x - with, charPosition.y + head, 0), new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
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
            if (Physics.Raycast(new Vector3(charPosition.x + with, charPosition.y + head, 0), new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x - with, charPosition.y + head, 0), new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
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
            if (Physics.Raycast(new Vector3(charPosition.x + with, charPosition.y - feed, 0), new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x - with, charPosition.y - feed, 0), new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
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
            if (Physics.Raycast(new Vector3(charPosition.x + with, charPosition.y - feed, 0), new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x - with, charPosition.y - feed, 0), new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
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
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore)
                )
            {
                if (hit.transform.tag == "noSlide")
                    return false;
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
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore)
                )
            {
                if (hit.transform.tag == "noSlide")
                    return false;
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
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y, 0), new Vector3(charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore)
                )
            {
                if (hit.transform.tag == "noSlide")
                    return false;
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
            // Cast a Ray at the feed and Check for an Obsticle
            if (Physics.Raycast(new Vector3(charPosition.x, charPosition.y - feed, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y + head, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x, charPosition.y, 0), new Vector3(-charPosition.x, 0, 0), out hit, length, 9, QueryTriggerInteraction.Ignore)
                )
            {
                if (hit.transform.tag == "noSlide")
                    return false;
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
            if (Physics.Raycast(new Vector3(charPosition.x + with, charPosition.y + head, 0), new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x - with, charPosition.y + head, 0), new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag == "noSlide")
                    return false;
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
            if (Physics.Raycast(new Vector3(charPosition.x + with, charPosition.y + head, 0), new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x - with, charPosition.y + head, 0), new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag == "noSlide")
                    return false;
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
            if (Physics.Raycast(new Vector3(charPosition.x + with, charPosition.y - feed, 0), new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) || 
                Physics.Raycast(new Vector3(charPosition.x - with, charPosition.y - feed, 0), new Vector3(0, charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag == "noSlide")
                    return false;
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
            if (Physics.Raycast(new Vector3(charPosition.x + with, charPosition.y - feed, 0), new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore) ||
                Physics.Raycast(new Vector3(charPosition.x - with, charPosition.y - feed, 0), new Vector3(0, -charPosition.y, 0), out hit, length, 9, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag == "noSlide")
                    return false;
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
