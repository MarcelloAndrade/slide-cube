using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvLManager : MonoBehaviour {

    public Sprite spriteUnlock;
    public Sprite spriteLock;    

    private Button[] levelButtons;
    private void Awake() {
        //PlayerPrefs.DeleteAll();
        int lastLevelComplete = PlayerPrefs.GetInt("LastLevelComplete", 1);
        levelButtons = new Button[transform.childCount];
        for(int i = 0; i < levelButtons.Length; i++) {
            int curret = i + 1;
            levelButtons[i] = transform.GetChild(i).GetComponent<Button>();
            levelButtons[i].onClick.AddListener(() => this.LoadScene(curret));
            if (i+1 <= lastLevelComplete) {
                transform.GetChild(i).GetComponent<Button>().image.sprite = spriteUnlock;
                levelButtons[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
                levelButtons[i].interactable = true;
            } else {
                transform.GetChild(i).GetComponent<Button>().image.sprite = spriteLock;
                levelButtons[i].GetComponentInChildren<Text>().enabled = false;
                levelButtons[i].interactable = false;                
            }
        }
    }

    public void LoadScene(int level) {
        PlayerPrefs.SetInt("LevelSelect", level);        
        SceneManager.LoadScene("LvL" + level.ToString());        
    }
}
