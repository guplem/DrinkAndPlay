﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[Serializable]
public struct TextsToLocalize {
    public string stringTag;
    public TextMeshProUGUI tmProGui;

    public TextsToLocalize(string stringTag, TextMeshProUGUI tmProGui)
    {
        this.stringTag = stringTag;
        this.tmProGui = tmProGui;
    }

    public TextsToLocalize(TextMeshProUGUI tmProGui) : this()
    {
        this.tmProGui = tmProGui;
    }

    public bool IsReady()
    {
        return tmProGui != null;
    }
}

public class Localizer : MonoBehaviour
{

    [SerializeField] public string id;
    [SerializeField] public LocalizationFile localizationFile;
    [SerializeField] public bool automaticallyLocalize = false;
    [SerializeField] public bool registerTimestampAtLocalize;

    private string currentLanguage = "";
    private bool previouslyStarted;
    private bool subscribed = false;

    public TextsToLocalize[] textsToLocalize;



    private void OnEnable()
    {
        if (textsToLocalize.Length == 0 || textsToLocalize == null)
        {
            textsToLocalize = new TextsToLocalize[1];
            textsToLocalize[0] = new TextsToLocalize(GetComponent<TextMeshProUGUI>() );

            if (!textsToLocalize[0].IsReady())
                Debug.LogError("Any 'TextMeshProUGUI' component could be found in the object " + name, gameObject);
        }

        if (!subscribed)
        {
            LocalizationManager.OnLocalizeAllAction += Localize;
            subscribed = true;
        }

        if (previouslyStarted)
            Start();

    }

    private void OnDisable()
    {
        if (subscribed)
        {
            LocalizationManager.OnLocalizeAllAction -= Localize;
            subscribed = false;
        }
    }

    private void Start()
    {
        previouslyStarted = true;

        if (automaticallyLocalize)
            if (currentLanguage != GameManager.instance.dataManager.language)
                Localize();
    }

    public void SetId(string id)
    {
        this.id = id;
    }
    
    public void SetLocalizationFile(LocalizationFile localizationFile)
    {
        this.localizationFile = localizationFile;
    }
    
    public void Localize(string id)
    {
        SetId(id);
        Localize();
    }
    
    public void Localize(LocalizedText lt)
    {
        SetId(lt.id);
        Localize();
    }

    public void Localize(string id, LocalizationFile localizationFile)
    {
        SetId(id);
        SetLocalizationFile(localizationFile);
        Localize();
    }


    public void Localize()
    {
        if (!Application.isPlaying)
            return;

        if (string.IsNullOrEmpty(id))
        {
            GameObject go = gameObject;
            Debug.LogWarning("Trying to localize the object '" + go.name + "' but the 'id' in the 'Localizer' component is null or empty.", go);
            return;
        }
        
        if (localizationFile == null)
        {
            GameObject go = gameObject;
            Debug.LogWarning("Trying to localize the object '" + go.name + "' but the 'localizationFile' in the 'Localizer' component is null.", go);
            return;
        }
        
        currentLanguage = GameManager.instance.dataManager.language;
        LocalizedText localizedText = GameManager.instance.localizationManager.GetLocalizedText(localizationFile, id, registerTimestampAtLocalize);
        SetText(localizedText);
    }

    public void SetText(LocalizedText localizedText)
    {
        SetId(localizedText.id);
        
        string text = localizedText.text;

        List<TextsToLocalize> savedForLater = new List<TextsToLocalize>(); 
        foreach (TextsToLocalize txt in textsToLocalize)
        {
            if (txt.tmProGui == null)
                Debug.LogWarning("A TMProGUI is null in a text to localize " + name, gameObject);
            
            if (string.IsNullOrEmpty(txt.stringTag))
            {
                //ApplyText(txt.tmProGui, text);
                savedForLater.Add(txt);
            }
            else
            {
                int pFrom = text.IndexOf(">"+txt.stringTag+">", StringComparison.OrdinalIgnoreCase) + (">"+txt.stringTag+">").Length;
                int pTo = text.LastIndexOf("<"+txt.stringTag+"<", StringComparison.OrdinalIgnoreCase);
            
                if (pFrom >= pTo || pFrom < 0 || pTo < 0)
                {
                    GameObject o = gameObject;
                    //Debug.LogWarning(o.name +  " is trying to localize an object but the text between the designed stringTag (" + txt.stringTag + ") was not found in the text with id = '" + id + "' from localization file '" + localizationFile + "'", o);
                    ApplyText(txt.tmProGui, "");
                }
                else
                {
                    ApplyText(txt.tmProGui, text.Substring(pFrom, pTo - pFrom));
                }
            }
        }

        foreach (TextsToLocalize txt in savedForLater)
        {
            ApplyText(txt.tmProGui, text);
        }
    }

    private void ApplyText(TextMeshProUGUI tmProGui, string text)
    {
        tmProGui.richText = true;
        tmProGui.text = text;
    }

}