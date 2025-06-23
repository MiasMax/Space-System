using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class OrbitalTrail : MonoBehaviour
{
    public int maxPoints = 500; // Nombre max de points
    private LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();

    public float trailWidthFactor = 0.5f; // Échelle de largeur du trail

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.useWorldSpace = true;

        SetupRenderer();
    }

    void Update()
    {
        if (Time.timeScale != 0f)
        {
           
            positions.Add(transform.position);

            if (positions.Count > maxPoints)
                positions.RemoveAt(0);

            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }
    }

    void SetupRenderer()
    {
        // Largeur proportionnelle à la taille de l’objet
        float baseScale = transform.localScale.x * trailWidthFactor;

        AnimationCurve widthCurve = new AnimationCurve();
        widthCurve.AddKey(0f, baseScale * 0.5f); // plus ancien point
        widthCurve.AddKey(1f, baseScale);        // point récent
        lineRenderer.widthCurve = widthCurve;

        // Dégradé de couleur : sombre vers clair
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.black, 0f),
                new GradientColorKey(Color.white, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0.2f, 0f),
                new GradientAlphaKey(1.0f, 1f)
            }
        );
        lineRenderer.colorGradient = gradient;

        // Matériau unlit
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
    }
}
