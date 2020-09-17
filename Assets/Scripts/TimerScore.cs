using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScore : MonoBehaviour {

    public GameManager gameManager;
    public Text timerMinutes;
    public Text timerSeconds;
    public Text timerMilliseconds;
    private float timer;
    private float startTime;

    void Start() {
        startTime = Time.time;
        timerMinutes.text = "00";
        timerSeconds.text = "00";
        timerMilliseconds.text = "00";
    }

    void Update() {
        if (gameManager.gameIsRunning) {
            timer = Time.time - startTime;
            int minutes = (int)timer / 60;
            int seconds = (int)timer % 60;
            int milliseconds = (int)(Math.Floor((timer - (seconds + minutes * 60)) * 100));

            timerMinutes.text = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
            timerSeconds.text = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();
            timerMilliseconds.text = milliseconds < 1 ? "0" + milliseconds.ToString() : milliseconds.ToString();

            gameManager.scoreLevelMinutes = timerMinutes.text;
            gameManager.scoreLevelSeconds = timerSeconds.text;
            gameManager.scoreLevelMilliseconds = timerMilliseconds.text;
        }
    }
}
