using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class StarGenerator
{
    private static Gradient starColorGradient;

    static StarGenerator()
    {
        starColorGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[7];

        colorKeys[0].color = new Color(1f, 0f, 0f);      // M (rouge)
        colorKeys[0].time = 0.000f;

        colorKeys[1].color = new Color(1f, 0.5f, 0f);    // K (orange)
        colorKeys[1].time = 0.0158f;

        colorKeys[2].color = new Color(1f, 1f, 0f);      // G (jaune)
        colorKeys[2].time = 0.042f;

        colorKeys[3].color = new Color(1f, 1f, 0.8f);    // F (jaune-blanc)
        colorKeys[3].time = 0.0683f;

        colorKeys[4].color = new Color(1f, 1f, 1f);      // A (blanc)
        colorKeys[4].time = 0.1098f;

        colorKeys[5].color = new Color(0.7f, 0.8f, 1f);  // B (bleu-blanc)
        colorKeys[5].time = 0.2933f;

        colorKeys[6].color = new Color(0.4f, 0.6f, 1f);  // O (bleu)
        colorKeys[6].time = 0.5563f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;

        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        starColorGradient.SetKeys(colorKeys, alphaKeys);
    }



    public static GameObject GenerateStar(GameObject starPrefab, float salpeterExponent = 2.35f, Vector3? position = null, float massMin = 0.08f * PhysicsConstants.ratioMsMt, float massMax = 300 * PhysicsConstants.ratioMsMt)
    {
        Vector3 finalPosition = position ?? Vector3.zero;

        GameObject star = GameObject.Instantiate(starPrefab, finalPosition, Quaternion.identity);
        Rigidbody rb = star.GetComponent<Rigidbody>();

        System.Random rng = new System.Random();
        float starMass = SalpeterMass(rng, salpeterExponent, massMin, massMax);

        Debug.Log("starMass = " + starMass);

        rb.mass = starMass;

        float radius = EstimateStellarRadius(starMass);
        star.transform.localScale = Vector3.one * radius;

        SphereCollider collider = star.GetComponent<SphereCollider>();
        if (collider != null)
        {
            collider.radius = 0.5f; // Valeur par défaut
        }

        Renderer renderer = star.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material dynamicMat = new Material(renderer.sharedMaterial); // clone safe instance
            // Color color = new Color(255, 255, 255);
            Color color = TemperatureToColor(MassToTemperature(starMass));
            dynamicMat.SetColor("_EmissionColor", color * 1f); // HDR Intensity
            dynamicMat.EnableKeyword("_EMISSION");
            renderer.material = dynamicMat;
        }
        return star;
    }

    // 0.8 et 300 en masses solaire, on multiplie par PhysicsConstants.ratioMsMt pour rester cohérent avec la masse terrestre 
    public static float SalpeterMass(System.Random rng, float salpeterExponent = 2.35f, float mMin = 0.08f * PhysicsConstants.ratioMsMt, float mMax = 300 * PhysicsConstants.ratioMsMt)
    {
        float rand = (float)rng.NextDouble();

        // Inverse de la fonction de répartition cumulative (CDF)
        float powMin = Mathf.Pow(mMin, 1 - salpeterExponent);
        float powMax = Mathf.Pow(mMax, 1 - salpeterExponent);

        float mass = Mathf.Pow(powMin + rand * (powMax - powMin), 1 / (1 - salpeterExponent));
        return mass;
    }

    public static float EstimateStellarRadius(float mass)
    {
        if (mass <= 1f * PhysicsConstants.ratioMsMt)
            return Mathf.Pow(mass, 0.8f);
        else
            return Mathf.Pow(mass, 0.57f);
    }

 
    public static float MassToTemperature(float solarMass)
    {
        // Masse minimale pour la séquence principale ~0.1 Ms
        if (solarMass < 0.1f * PhysicsConstants.ratioMsMt) solarMass = 0.1f * PhysicsConstants.ratioMsMt;

        float T_sun = 5778f; // Température solaire en K
        
        float solarTemp = T_sun * Mathf.Sqrt(solarMass);
        Debug.Log("solarTemp = " + solarTemp);

        return solarTemp; 
    }

    public static Color TemperatureToColor(float temperature)
    {
        // 9 ~ sqrt 80, 547 ~ sqrt 300 000, multiplié par 5778 ce sont les valeurs extrêmes de la température
        temperature = Mathf.Clamp(temperature, 52000, 3160000);

        // Température bornée entre 52000(rouge) et 3160000 (bleu)
        float t = Mathf.InverseLerp(52000, 3160000, temperature); // t dans [0, 1]

        Debug.Log("t = " + t);

        return starColorGradient.Evaluate(t);
    }
}
