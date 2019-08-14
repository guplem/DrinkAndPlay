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
        SetupSlider(minSlider, minSliderValueIndicator, GameManager.Instance.dataManager.naughtyLevelExtremes.min, GameManager.Instance.dataManager.naughtyLevelExtremes.max, GameManager.Instance.dataManager.GetNaughtyLevelMin());
        SetupSlider(maxSlider, maxSliderValueIndicator, GameManager.Instance.dataManager.naughtyLevelExtremes.min, GameManager.Instance.dataManager.naughtyLevelExtremes.max, GameManager.Instance.dataManager.GetNaughtyLevelMax());
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
        if (newVal > GameManager.Instance.dataManager.GetNaughtyLevelMax())
        {
            SetValueSlider(minSlider, minSliderValueIndicator, GameManager.Instance.dataManager.GetNaughtyLevelMax());
        }
        else
        {
            SetTextIndicator(minSliderValueIndicator, newVal);
            GameManager.Instance.dataManager.SetNaughtyLevelMin(Mathf.RoundToInt(newVal));
        }
    }

    public void NewMaxValue(float newVal)
    {
        if (newVal < GameManager.Instance.dataManager.GetNaughtyLevelMin())
        {
            SetValueSlider(maxSlider, maxSliderValueIndicator, GameManager.Instance.dataManager.GetNaughtyLevelMin());
        }
        else
        {
            SetTextIndicator(maxSliderValueIndicator, newVal);
            GameManager.Instance.dataManager.SetNaughtyLevelMax(Mathf.RoundToInt(newVal));
        }
    }
}
