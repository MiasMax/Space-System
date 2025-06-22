using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{    
    
    
    float PlanetMass()
    {
        float mMin = 0.01f; // masse minimale en masses terrestre
        float mMax = 1000f;   // masse maximale en masses terrestre

        float alpha = 1.7f;
        float rand = Random.value;

        float powMin = Mathf.Pow(mMin, 1 - alpha);
        float powMax = Mathf.Pow(mMax, 1 - alpha);

        float mass = Mathf.Pow(powMin + rand * (powMax - powMin), 1 / (1 - alpha));
        return mass;
    }
}
