using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject completeLevelUI;
    public GameObject settingsUI;
    public GameObject audioOn;
    public GameObject audioOff;
    public GameObject startTouchlUI;
    public Text scoreLvlUI;
    public Text highScoreLvlUI;

    public bool gameIsRunning { get; set; } = false;
    public bool activeTouchlUI { get; set; } = true;

    public string scoreLevelMinutes { get; set; }
    public string scoreLevelSeconds { get; set; }
    public string scoreLevelMilliseconds { get; set; }
    
    void Start() {
        completeLevelUI.SetActive(false);
        settingsUI.SetActive(false);
        audioOn.SetActive(true);
        audioOff.SetActive(false);
        startTouchlUI.SetActive(true);
    }

    private void Update() {
        if (!activeTouchlUI) {
            startTouchlUI.SetActive(false);
            gameIsRunning = true;
        }
    }

    public void CompleteLevel() {
        Time.timeScale = 0f;
        gameIsRunning = false;
        SetScoreValue();
        completeLevelUI.SetActive(true);
        GameSounds.PlayerSound(GameSounds.Sound.LvLWin);        
    }

    private void SetScoreValue() {
        int minutes = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "-HighScore-Minutes", 0);
        int seconds = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "-HighScore-Seconds", 0);
        int milliseconds = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "-HighScore-Milliseconds", 0);

        Debug.Log(minutes +"-"+ seconds + "-" + milliseconds);

        if (int.Parse(scoreLevelMinutes) <= minutes &&
            int.Parse(scoreLevelSeconds) <= seconds &&
            int.Parse(scoreLevelMilliseconds) <= milliseconds) {
            Debug.Log("SAVE-" +int.Parse(scoreLevelMinutes) + "-" + int.Parse(scoreLevelSeconds) + "-" + int.Parse(scoreLevelMilliseconds));


            int minutesSave = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "-HighScore-Minutes", int.Parse(scoreLevelMinutes));
            int secondsSave = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "-HighScore-Seconds", int.Parse(scoreLevelSeconds));
            int millisecondsSave = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "-HighScore-Milliseconds", int.Parse(scoreLevelMilliseconds));

            highScoreLvlUI.text = (minutesSave < 10 ? "0" + minutesSave.ToString() : minutesSave.ToString()) + ":" +
                                  (secondsSave < 10 ? "0" + secondsSave.ToString() : secondsSave.ToString())  + ":" +
                                  (millisecondsSave < 10 ? "0" + millisecondsSave.ToString() : millisecondsSave.ToString());
        }
        scoreLvlUI.text = scoreLevelMinutes + ":" + scoreLevelSeconds + ":" + scoreLevelMilliseconds;
    }

    public void Restart() {
        OpenOrClouseUISettings(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenOrClouseUISettings(bool isVisible) {
        if (isVisible) {
            settingsUI.SetActive(true);
            Time.timeScale = 0f;
        } else {
            settingsUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void SetVolume() {
        if (audioOn.activeInHierarchy) {
            audioOn.SetActive(false);
            audioOff.SetActive(true);
            GameSounds.SetVolume(0);
        } else {
            audioOn.SetActive(true);
            audioOff.SetActive(false);
            GameSounds.SetVolume(1);
        }
    }

}
