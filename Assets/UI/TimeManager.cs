using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI endGameText;
    public float minuteDuration = 0.25f;
    public string targetTime = "13:00";

    private float currentTime = 0f;
    private bool isGameEnded = false;

    void Start()
    {
        currentTime = ConvertTimeToMinutes("11:00");
        InvokeRepeating("AddTime", 0f, minuteDuration);
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

        if (currentTime >= ConvertTimeToMinutes(targetTime))
        {
            EndGame();
        }
    }

    void AddTime()
    {
        if (isGameEnded)
            return;

        currentTime += minuteDuration;
        DisplayTime(currentTime);
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
        CancelInvoke("AddTime");
        endGameText.gameObject.SetActive(true);
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
