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

    public float timer;
    private float startTime;

    private int minutes;
    private int seconds;
    private int milliseconds;

    void Start() {
        startTime = Time.time;
        timerMinutes.text = "00";
        timerSeconds.text = "00";
        timerMilliseconds.text = "00";
    }

    void Update() {
        if (!gameManager.activeTouchlUI) {
            timer = Time.time - startTime;
            minutes = (int)timer / 60;
            seconds = (int)timer % 60;
            milliseconds = (int)(Math.Floor((timer - (seconds + minutes * 60)) * 100));

            timerMinutes.text = GameUtil.ZeroOnLeft(minutes);
            timerSeconds.text = GameUtil.ZeroOnLeft(seconds);
            timerMilliseconds.text = milliseconds.ToString();

            gameManager.scoreLvL = timer;
        }
    }

}
