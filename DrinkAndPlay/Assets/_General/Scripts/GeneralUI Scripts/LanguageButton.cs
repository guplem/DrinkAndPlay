using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    private Language language;
    [SerializeField] private TextMeshProUGUI textName;
    private Button btn; 
    private bool subscribed = false;
    
    private void OnEnable()
    {
        btn = GetComponent<Button>();
        
        if (GameManager.instance == null)
            return;
        
        if (!subscribed)
        {
            textName.GetComponent<ColorSwitcher>().afterColorSwitchCall += SetColors;
            subscribed = true;
        }
    }
    
    private void OnDisable()
    {
        if (subscribed)
        {
            textName.GetComponent<ColorSwitcher>().afterColorSwitchCall -= SetColors;
            subscribed = false;
        }
    }
    
    public void Setup(Language language)
    {
        OnEnable();
        
        this.language = language;
        textName.text = language.titleName;
        
        btn.interactable = language.isEnabled;

        SetColors(GameManager.instance.dataManager.GetVisualMode());
    }

    public void SelectLanguage()
    {
        GameManager.instance.dataManager.language = language;
    }

    public void SetColors(LightDarkColor.ColorType colorType)
    {
        if (language == null)
            return;
        
        if (!this.language.isEnabled)
        {
            ColorBlock buttonColors = btn.colors;
            textName.color = new Color(textName.color.r, textName.color.g, textName.color.b, buttonColors.disabledColor.a);
        }
    }
}