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
        SetVolume();
        completeLevelUI.SetActive(false);
        settingsUI.SetActive(false);
        startTouchlUI.SetActive(true);
    }

    private void Update() {
        if (!activeTouchlUI) {
            startTouchlUI.SetActive(false);            
        }
    }

    public void CompleteLevel() {
        lvlComplete = true;
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
