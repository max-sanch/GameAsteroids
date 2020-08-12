using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuObj;
    public GameObject textObj;
    public GameObject resumeButton;
    public GameObject controlButton;
    public static bool gameIsPause = true;
    public static bool isStartMenu = true;
    public static bool isGameOver = false;
    public static bool isMouseAndKeyboard = false;

    private void Start()
    {
        Time.timeScale = 0;
        int MAK = PlayerPrefs.GetInt("MAK", 2);
        if (MAK == 1)
        { 
            controlButton.transform.Find("Text").GetComponent<Text>().text = "Мышь и клав.";
            isMouseAndKeyboard = true;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isStartMenu && !isGameOver)
        {
            if (gameIsPause)
                Resume();
            else
                Pause();
        }
        if (isGameOver)
        {
            textObj.GetComponent<Text>().text = "GameOver";
            resumeButton.GetComponent<Button>().interactable = false;
            resumeButton.transform.Find("Text").GetComponent<Text>().color = new Color(0.35f, 0.35f, 0.35f);
            Pause();
        }
    }

    public void Resume()
    {
        menuObj.SetActive(false);
        gameIsPause = false;
        Time.timeScale = 1;
    }

    void Pause()
    {
        if (!isGameOver)
        {
            textObj.GetComponent<Text>().text = "Пауза";
            resumeButton.GetComponent<Button>().interactable = true;
            resumeButton.transform.Find("Text").GetComponent<Text>().color = Color.white;
        }
        menuObj.SetActive(true);
        gameIsPause = true;
        Time.timeScale = 0;
    }

    public void NewGame()
    {
        if (isStartMenu)
        {
            isStartMenu = false;
            SpawnPointAsteroid.RandomazeAsteroid();
            Resume();
        }
        else
        {
            isGameOver = false;
            Resume();
            UFO.isDespawnUFO = true;
            Player.isRespawnPlayer = true;
            Asteroid.DespawnAsteroid();
            SpawnPointAsteroid.isStartCountAsteroid = true;
            GameStatistic.score = 0;
            GameStatistic.AddLives();
        }
    }

    public void Сontrol()
    {
        if (isMouseAndKeyboard)
        {
            controlButton.transform.Find("Text").GetComponent<Text>().text = "Клавиатура";
            isMouseAndKeyboard = false;
            PlayerPrefs.SetInt("MAK", 0);
        }
        else
        {
            controlButton.transform.Find("Text").GetComponent<Text>().text = "Мышь и клав.";
            isMouseAndKeyboard = true;
            PlayerPrefs.SetInt("MAK", 1);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
