using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject completeLevelUI;
    public GameObject gameOverUI;

    public GameObject settingsUI;
    public GameObject audioOn;
    public GameObject audioOff;

    public int timeToCompleteLevel;


    void Start() {
        completeLevelUI.SetActive(false);
        gameOverUI.SetActive(false);
        settingsUI.SetActive(false);

        audioOn.SetActive(true);
        audioOff.SetActive(false);
    }

    void Update() {
        
    }

    public void CompleteLevel() {
        completeLevelUI.SetActive(true);
        GameSounds.PlayerSound(GameSounds.Sound.LvLWin);        
    }

    public void EndGame() {
        gameOverUI.SetActive(true);
        GameSounds.PlayerSound(GameSounds.Sound.LvLFail);
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

    public int GetTimeToCompleteLevel () { 
        return timeToCompleteLevel;
    }     
}
