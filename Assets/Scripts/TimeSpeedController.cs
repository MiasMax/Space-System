using UnityEngine;

public class TimeSpeedController : MonoBehaviour
{
    public float[] timeScales = { 0f, 0.1f, 1f, 10f, 100f, 1000f }; // Valeurs possibles
    private int currentIndex = 2; // Par défaut : 1x (index 2)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = Mathf.Min(currentIndex + 1, timeScales.Length - 1);
            ApplyTimeScale();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = Mathf.Max(currentIndex - 1, 0);
            ApplyTimeScale();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = 0; // Pause
            ApplyTimeScale();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (timeScales[currentIndex] == 0f)
                currentIndex = 2; // Revenir à 1x si on était en pause
            ApplyTimeScale();
        }
    }

    void ApplyTimeScale()
    {
        Time.timeScale = timeScales[currentIndex];
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // adapter FixedUpdate à l’échelle de temps

        Debug.Log("Time scale: x" + timeScales[currentIndex]);
    }
}
