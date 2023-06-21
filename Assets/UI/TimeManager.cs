using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public GameObject endGamePaper;
    public string startTime = "11:00";
    public string targetTime = "13:00";

    private float currentTime = 0f;
    private bool isGameEnded = false;

    void Start()
    {
        currentTime = ConvertTimeToMinutes(startTime);
		DisplayTime(currentTime);
    }

    void Update()
    {
        if (isGameEnded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                QuitGame();
            }
            return;
        }
    }

    void AddTime(float minuteDuration)
    {
        if (isGameEnded)
            return;

        currentTime += minuteDuration;
        DisplayTime(currentTime);
        
        if (currentTime >= ConvertTimeToMinutes(targetTime))
        {
            EndGame();
        }
    }

    void DisplayTime(float minutes)
    {
        int hours = Mathf.FloorToInt(minutes / 60);
        int minutesLeft = Mathf.FloorToInt(minutes % 60);
        string timeString = hours.ToString("D2") + ":" + minutesLeft.ToString("D2");
        timeText.text = timeString;
    }

    float ConvertTimeToMinutes(string time)
    {
        string[] timeArray = time.Split(':');
        int hours = int.Parse(timeArray[0]);
        int minutes = int.Parse(timeArray[1]);
        return hours * 60 + minutes;
    }

    void EndGame()
    {
        isGameEnded = true;
        endGamePaper.SetActive(true);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
