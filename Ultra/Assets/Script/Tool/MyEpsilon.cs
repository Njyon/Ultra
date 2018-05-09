using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEpsilon
{
	public static bool Epsilon(float a, float b, float epsilon)
    {
        if(a >= b - epsilon && a <= b + epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
