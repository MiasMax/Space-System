using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingSetup : MonoBehaviour
{
    public Material skyboxMaterial;

    void Start()
    {
        // Cr?er un Volume
        GameObject volumeGO = new GameObject("Global Volume");
        Volume volume = volumeGO.AddComponent<Volume>();
        volume.isGlobal = true;
        volume.priority = 0;

        // Ajouter un profil
        VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();
        volume.sharedProfile = profile;

        // Ajouter un effet de bloom
        Bloom bloom;
        if (!profile.TryGet(out bloom))
        {
            bloom = profile.Add<Bloom>(true);
        }
        bloom.intensity.overrideState = true;
        bloom.intensity.value = 7f;

        // Facultatif : scatter léger et seuil
        bloom.scatter.overrideState = true;
        bloom.scatter.value = 0.7f;
        bloom.threshold.overrideState = true;
        bloom.threshold.value = 0.7f;

        // Placer le volume en (0, 0, 0)
        volumeGO.transform.position = Vector3.zero;

        // Définir la Skybox
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }
    }
}