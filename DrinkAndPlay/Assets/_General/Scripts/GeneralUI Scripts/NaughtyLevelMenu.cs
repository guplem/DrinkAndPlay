using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NaughtyLevelMenu : MonoBehaviour
{

    [Header("MIN")]
    [SerializeField] private TextMeshProUGUI minSliderValueIndicator;
    [SerializeField] private Slider minSlider;

    [Header("MAX")]
    [SerializeField] private TextMeshProUGUI maxSliderValueIndicator;
    [SerializeField] private Slider maxSlider;

    private void Start()
    {
        SetupSlider(minSlider, minSliderValueIndicator, GameManager.instance.dataManager.naughtyLevelExtremes.min, GameManager.instance.dataManager.naughtyLevelExtremes.max, GameManager.instance.dataManager.GetNaughtyLevelMin());
        SetupSlider(maxSlider, maxSliderValueIndicator, GameManager.instance.dataManager.naughtyLevelExtremes.min, GameManager.instance.dataManager.naughtyLevelExtremes.max, GameManager.instance.dataManager.GetNaughtyLevelMax());
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

    public void NewMinValue(float newVal)
    {
        if (newVal > GameManager.instance.dataManager.GetNaughtyLevelMax())
        {
            SetValueSlider(minSlider, minSliderValueIndicator, GameManager.instance.dataManager.GetNaughtyLevelMax());
        }
        else
        {
            SetTextIndicator(minSliderValueIndicator, newVal);
            GameManager.instance.dataManager.SetNaughtyLevelMin(Mathf.RoundToInt(newVal));
        }
    }

    public void NewMaxValue(float newVal)
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
    }
}
