using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPause = false;
    public GameObject PauseMenuUI;
    public static float OldTimeScale;
    public Slider mySlider; // Drag your slider from the UI to this field in the Inspector


    private void Start()
    {
        PauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = OldTimeScale;
        GameIsPause = false;
    }

    void Pause()
    {
        OldTimeScale = Time.timeScale;
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    public void RestartSystem()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Resume();
    }

    public void SlideTimeScale()
    {
        int slideTimeScale = (int)mySlider.value;

        TimeSpeedController.ApplyTimeScale(slideTimeScale);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

