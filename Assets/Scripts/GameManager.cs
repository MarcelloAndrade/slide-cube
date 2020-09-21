using System;
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
    public Animator animBestScore;

    public bool lvlComplete { get; set; }  = false;
    public float scoreLvL { get; set; }
    public bool activeTouchlUI { get; set; } = true;
    public bool pauseGame { get; set; } = false;

    void Start() {
        GetVolume();
        completeLevelUI.SetActive(false);
        settingsUI.SetActive(false);
        startTouchlUI.SetActive(true);        
    }

    private void Update() {
        if (!activeTouchlUI) {
            startTouchlUI.SetActive(false);
            pauseGame = false;
        }
    }

    public void CompleteLevel() {
        lvlComplete = true;
        pauseGame = true;
        GameSounds.PlayerSound(GameSounds.Sound.LvLWin);        
        completeLevelUI.SetActive(true);
        SetScoreValue();
    }

    private void SetScoreValue() {
        String scoreName = SceneManager.GetActiveScene().name + "HighScore";
        if (scoreLvL < PlayerPrefs.GetFloat(scoreName, float.MaxValue)) {
            PlayerPrefs.SetFloat(scoreName, scoreLvL);
            highScoreLvlUI.text = GameUtil.ConvertScore(scoreLvL);
            animBestScore.SetTrigger("best-score-alert");
        } else {
            float loadHighScore = PlayerPrefs.GetFloat(scoreName);
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
            pauseGame = true;
        } else {
            settingsUI.SetActive(false);
            Time.timeScale = 1f;
            pauseGame = false;
        }
    }

    public void SetVolume() {
        if (PlayerPrefs.GetInt("Volume", 1) != 1) {
            audioOn.SetActive(true);
            audioOff.SetActive(false);
            GameSounds.SetVolume(1);
            PlayerPrefs.SetInt("Volume", 1);
        } else {
            audioOn.SetActive(false);
            audioOff.SetActive(true);            
            GameSounds.SetVolume(0);
            PlayerPrefs.SetInt("Volume", 0);
        }
    }


    public void GetVolume() {
        if (PlayerPrefs.GetInt("Volume", 1) == 1) {
            audioOn.SetActive(true);
            audioOff.SetActive(false);
            GameSounds.SetVolume(1);            
        } else {
            audioOn.SetActive(false);
            audioOff.SetActive(true);
            GameSounds.SetVolume(0);            
        }
    }

    public void NextLevel() {
        if (PlayerPrefs.GetInt("LevelSelect") >= PlayerPrefs.GetInt("LastLevelComplete")) {
            PlayerPrefs.SetInt("LastLevelComplete", PlayerPrefs.GetInt("LastLevelComplete") + 1);
        }
        SceneManager.LoadScene("LvLManager");
    }

    public void LvLManager() {        
        SceneManager.LoadScene("LvLManager");
    }
}
