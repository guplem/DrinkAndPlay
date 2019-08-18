using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageButton : MonoBehaviour
{
    private Language language;
    [SerializeField] private TextMeshProUGUI textName;
    
    public void Setup(Language language)
    {
        this.language = language;
        textName.text = language.languageName;
    }

    public void SelectLanguage()
    {
        GameManager.instance.dataManager.language = language.languageId;
    }
}