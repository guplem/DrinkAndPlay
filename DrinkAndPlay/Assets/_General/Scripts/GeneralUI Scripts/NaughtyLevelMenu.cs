using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NaughtyLevelMenu : MonoBehaviour
{
    
    [SerializeField] private Image sliderImageIndicator;
    [FormerlySerializedAs("minSlider")] [SerializeField] private Slider naughtinessSlider;
    [SerializeField] private Toggle automaticNaughtinessUpdatesToggle;
    private bool setUpDone = false;
    [SerializeField] private List<Sprite> sliderSprites;

    //[Header("MAX")]
    //[SerializeField] private TextMeshProUGUI maxSliderValueIndicator;
    //[SerializeField] private Slider maxSlider;

    private void Start()
    {
        SetupSlider(naughtinessSlider, sliderImageIndicator, GameManager.instance.dataManager.naughtyLevelExtremes.x, GameManager.instance.dataManager.naughtyLevelExtremes.y, GameManager.instance.dataManager.naughtyLevel);
        //SetupSlider(maxSlider, maxSliderValueIndicator, GameManager.instance.dataManager.naughtyLevelExtremes.min, GameManager.instance.dataManager.naughtyLevelExtremes.max, GameManager.instance.dataManager.GetNaughtyLevelMax());
        automaticNaughtinessUpdatesToggle.isOn = GameManager.instance.dataManager.automaticNaughtyLevel;
        setUpDone = true;
    }

    private void SetupSlider(Slider slider, Image imageIndicator, int minValue, int maxValue, float currentValue)
    {

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        SetValueSlider(slider, imageIndicator, currentValue);
    }

    private void SetValueSlider(Slider slider, Image imageIndicator, float value)
    {
        slider.value = value;
        SetSpriteIndicator(imageIndicator, value);
    }

    private void SetSpriteIndicator(Image imageIndicator, float value)
    {
        int newValueInt = Mathf.RoundToInt(value);
        imageIndicator.sprite = sliderSprites[(int)(Mathf.RoundToInt(value) - 1)];
    }

    public void NewValue(float newVal)
    {
        if (newVal > GameManager.instance.dataManager.naughtyLevelExtremes.y)
        {
            SetValueSlider(naughtinessSlider, sliderImageIndicator, GameManager.instance.dataManager.naughtyLevelExtremes.y);
        }
        else
        {
            SetSpriteIndicator(sliderImageIndicator, newVal);
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
            SetSpriteIndicator(maxSliderValueIndicator, newVal);
            GameManager.instance.dataManager.SetNaughtyLevelMax(Mathf.RoundToInt(newVal));
        }
    }*/
    
    public void SetAutomaticNaughtinessUpdatesTo(bool state)
    {
        if (setUpDone)
            GameManager.instance.dataManager.automaticNaughtyLevel = state;
    }

    public void UpdateValues()
    {
        Start();
    }
}
