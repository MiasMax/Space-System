using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlanetarySystemGenerator : MonoBehaviour
{
    [Header("Système stellaire")]
    public GameObject planetPrefab;
    public GameObject starPrefab;
    //public int numberOfPlanets;
    //public int numberOfStars;

    [Header("Paramètres d'orbites")]
    public float initialDistance = 50f;
    public float distanceStep = 30f;
    public float planetMass = 1; // Terre


    private float salpeterExponent = 1.2f;
    private float minSeparationFactor = 3f; // Valeur minimale du facteur de séparation
    private float maxSeparationFactor = 7f; // Valeur maximale du facteur de séparation
    private float meanSeparationFactor = 5f; // Moyenne utilisée dans la distribution normale


    void Start()
    {
        GameObject star1 = StarGenerator.GenerateStar(starPrefab, salpeterExponent);
        GameObject star2 = StarGenerator.GenerateStar(starPrefab, salpeterExponent);

        float rocheLimit = Mathf.Max(
            Orbits.ComputeRocheLimit(star1, star2),
            Orbits.ComputeRocheLimit(star2, star1)
        );
        float separation = rocheLimit * Mathf.Max(minSeparationFactor, Mathf.Min(Gaussian.NormalDistribution(meanSeparationFactor), maxSeparationFactor)); // Tirage aléatoire autour de 5 Bornage entre 3 et 7

        GenerateBinarySystem(star1, star2, separation);
    }

    void GenerateBinarySystem(GameObject star1, GameObject star2, float separation)
    {
        Rigidbody rb1 = star1.GetComponent<Rigidbody>();
        Rigidbody rb2 = star2.GetComponent<Rigidbody>();

        if (rb1 == null || rb2 == null)
        {
            Debug.LogError("Les deux objets doivent avoir un Rigidbody.");
            return;
        }

        float m1 = rb1.mass;
        float m2 = rb2.mass;
        float totalMass = m1 + m2;

        // Direction aléatoire dans le plan XY
        Vector3 direction = UnityEngine.Random.insideUnitCircle.normalized;
        Vector3 dir = new Vector3(direction.x, direction.y, 0f);

        // Positionner les deux étoiles autour du barycentre (origine)
        Vector3 pos1 = -dir * (separation * m2 / totalMass);
        Vector3 pos2 = dir * (separation * m1 / totalMass);
        star1.transform.position = pos1;
        star2.transform.position = pos2;

        // Vitesse orbitale relative
        float v = Mathf.Sqrt(PhysicsConstants.G * totalMass / separation);

        // Direction de la vitesse tangentielle
        Vector3 velocityDir = Vector3.Cross(dir, Vector3.forward).normalized;

        // Vitesse de chaque étoile (barycentre fixe)
        rb1.velocity = velocityDir * v * (m2 / totalMass);
        rb2.velocity = -velocityDir * v * (m1 / totalMass);
    }
}
