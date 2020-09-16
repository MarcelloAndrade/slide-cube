﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour {

    public GameManager gameManager;

    public Gradient gradient;
    public Image fill;

    private Slider slider;
    
    void Start() {
        slider = gameObject.GetComponent<Slider>();
        slider.maxValue = gameManager.GetTimeToCompleteLevel();
        slider.value = gameManager.GetTimeToCompleteLevel();

        fill.color = gradient.Evaluate(1f);
    }

    void Update() {
        if (slider.value > 0) {
            slider.value -= Time.deltaTime;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        } else if (slider.value <= 0) {
            gameManager.EndGame();
        }
    }
}
