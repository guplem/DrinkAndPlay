using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NaughtyLevelMenu : MonoBehaviour
{

    [FormerlySerializedAs("minSliderValueIndicator")]
    [SerializeField] private TextMeshProUGUI sliderValueIndicator;
    [FormerlySerializedAs("minSlider")] [SerializeField] private Slider naughtinessSlider;
    [SerializeField] private Toggle automaticNaughtinessUpdatesToggle;
    private bool setUpDone = false; 

    //[Header("MAX")]
    //[SerializeField] private TextMeshProUGUI maxSliderValueIndicator;
    //[SerializeField] private Slider maxSlider;

    private void Start()
    {
        SetupSlider(naughtinessSlider, sliderValueIndicator, GameManager.instance.dataManager.naughtyLevelExtremes.x, GameManager.instance.dataManager.naughtyLevelExtremes.y, GameManager.instance.dataManager.naughtyLevel);
        //SetupSlider(maxSlider, maxSliderValueIndicator, GameManager.instance.dataManager.naughtyLevelExtremes.min, GameManager.instance.dataManager.naughtyLevelExtremes.max, GameManager.instance.dataManager.GetNaughtyLevelMax());
        automaticNaughtinessUpdatesToggle.isOn = GameManager.instance.dataManager.automaticNaughtyLevel;
        setUpDone = true;
    }

    private void SetupSlider(Slider slider, TextMeshProUGUI textIndicator, int minValue, int maxValue, float currentValue)
    {

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        SetValueSlider(slider, textIndicator, currentValue);
    }

    private void SetValueSlider(Slider slider, TextMeshProUGUI textIndicator, float value)
    {
        slider.value = value;
        SetTextIndicator(textIndicator, value);
    }

    private void SetTextIndicator(TextMeshProUGUI textIndicator, float value)
    {
        int newValueInt = Mathf.RoundToInt(value);
        textIndicator.text = newValueInt.ToString();
    }

    public void NewValue(float newVal)
    {
        if (newVal > GameManager.instance.dataManager.naughtyLevelExtremes.y)
        {
            SetValueSlider(naughtinessSlider, sliderValueIndicator, GameManager.instance.dataManager.naughtyLevelExtremes.y);
        }
        else
        {
            SetTextIndicator(sliderValueIndicator, newVal);
            GameManager.instance.dataManager.SetNaughtyLevel(newVal);
        }
    }

    /*public void NewMaxValue(float newVal)
    {
        if (newVal < GameManager.instance.dataManager.GetNaughtyLevelMin())
        {
            SetValueSlider(maxSlider, maxSliderValueIndicator, GameManager.instance.dataManager.GetNaughtyLevelMin());
        }
        else
        {
            SetTextIndicator(maxSliderValueIndicator, newVal);
            GameManager.instance.dataManager.SetNaughtyLevelMax(Mathf.RoundToInt(newVal));
        }
    }*/
    
    public void SetAutomaticNaughtinessUpdatesTo(bool state)
    {
        if (setUpDone)
            GameManager.instance.dataManager.automaticNaughtyLevel = state;
    }
}
