using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbits 
{
    public static float ComputeRocheLimit(GameObject body1, GameObject body2)
    {
        float rocheLimit = 0;

        Rigidbody rb1 = body1.GetComponent<Rigidbody>();
        Rigidbody rb2 = body2.GetComponent<Rigidbody>();

        float rho1 = rb1.mass / ((4 / 3) * Mathf.PI * rb1.transform.localScale.x);
        float rho2 = rb2.mass / ((4 / 3) * Mathf.PI * rb2.transform.localScale.x);

        rocheLimit = 2.5f * rb1.transform.localScale.x * Mathf.Pow(rho1 / rho2, 1f / 3f);

        return rocheLimit;
    }

    
}
