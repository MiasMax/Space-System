using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gaussian
{
    public static float NormalDistribution(float mean = 0, float stdDev = 1, float min = - 4, float max = 4)
    {
        float value;

        do
        {
            // Box-Muller transform : génère une variable normale (Z ~ N(0, 1))
            float u1 = Random.value;
            float u2 = Random.value;
            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

            // Ajuste selon moyenne et écart-type
            value = mean + stdDev * randStdNormal;
        }
        while (value < min || value > max); // Rejet si hors bornes

        return value;
    }
}
