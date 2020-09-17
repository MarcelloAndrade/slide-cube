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
    public bool activeTouchlUI = true;
    public bool lvlComplete = false;
    public float scoreLvL;    
    
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
        }
    }

    public void CompleteLevel() {
        Time.timeScale = 0f;
        lvlComplete = true;
        SetScoreValue();
        completeLevelUI.SetActive(true);
        GameSounds.PlayerSound(GameSounds.Sound.LvLWin);        
    }

    private void SetScoreValue() {
        if (scoreLvL < PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name +"HighScore", 9999)) {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "HighScore", scoreLvL);
            highScoreLvlUI.text = GameUtil.ConvertScore(scoreLvL);             
        } else {
            float loadHighScore = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "HighScore");
            highScoreLvlUI.text = GameUtil.ConvertScore(loadHighScore);
        }

        scoreLvlUI.text = GameUtil.ConvertScore(scoreLvL);
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
