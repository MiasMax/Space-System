using System;
using UnityEngine;

public class TimeSpeedController : MonoBehaviour
{
    public static float[] timeScales = { 0f, 0.1f, 1f, 10f, 100f}; // Valeurs possibles
    private static int currentIndex = 2; // Par d�faut : 1x (index 2)

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
                currentIndex = 2; // Revenir � 1x si on �tait en pause
            ApplyTimeScale();
        }
    }

    public static void ApplyTimeScale(int index = -1)
    {
        currentIndex = index == -1f ? currentIndex : index;
        Time.timeScale = timeScales[currentIndex];
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // adapter FixedUpdate � l��chelle de temps

        Debug.Log("Time scale: x" + timeScales[currentIndex]);
    }
}
