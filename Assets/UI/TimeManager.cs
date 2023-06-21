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

	private PlayerController _playerController;

    private void Start()
    {
		_playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        currentTime = ConvertTimeToMinutes(startTime);
		DisplayTime(currentTime);
    }

    public void AddTime(float minuteDuration)
    {
        currentTime += minuteDuration;
        DisplayTime(currentTime);
        
        if (currentTime >= ConvertTimeToMinutes(targetTime))
        {
        	endGamePaper.SetActive(true);
            _playerController.gameEnded = true;
        }
    }

    private void DisplayTime(float minutes)
    {
        int hours = Mathf.FloorToInt(minutes / 60);
        int minutesLeft = Mathf.FloorToInt(minutes % 60);
        string timeString = hours.ToString("D2") + ":" + minutesLeft.ToString("D2");
        timeText.text = timeString;
    }

    private float ConvertTimeToMinutes(string time)
    {
        string[] timeArray = time.Split(':');
        int hours = int.Parse(timeArray[0]);
        int minutes = int.Parse(timeArray[1]);
        return hours * 60 + minutes;
    }
}
