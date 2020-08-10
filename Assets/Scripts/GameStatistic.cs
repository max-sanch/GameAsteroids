using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatistic : MonoBehaviour
{
    public GameObject scoreObj;
    public List<GameObject> LivesList;
    public static List<GameObject> livesList;
    public static int numberLives;
    public static int score;

    void Start()
    {
        livesList = LivesList;
        numberLives = LivesList.Count;
        score = 0;
    }

    void Update()
    {
        scoreObj.GetComponent<Text>().text = score.ToString();
        LivesCheck();
    }

    void LivesCheck()
    {
        if (numberLives == 0)
        {
            LivesList[0].SetActive(false);
            PauseMenu.isGameOver = true;
        }
        else if (numberLives < LivesList.Count && LivesList[numberLives].activeSelf)
        {
            LivesList[numberLives].SetActive(false);
        }
    }

    public static void AddLives()
    {
        numberLives = livesList.Count;
        foreach (GameObject obj in livesList)
        {
            if (!obj.activeSelf)
                obj.SetActive(true);
        }
    }
}
