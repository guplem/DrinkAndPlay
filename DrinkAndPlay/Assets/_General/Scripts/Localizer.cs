using System;
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
    [SerializeField] public bool isUi;
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
                Debug.LogError("Aby 'TextMeshProUGUI' component could be found in the object " + name, gameObject);
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
    
    public void Localize(string id)
    {
        SetId(id);
        Localize();
    }


    public void Localize()
    {
        if (!Application.isPlaying)
            return;

        if (string.IsNullOrEmpty(id))
        {
            GameObject go = gameObject;
            Debug.LogWarning("Trying to localize the object '" + go.name + "' but the 'id' in the 'Localizer' component is null or empty", go);
            return;
        }

        Section section = isUi ? GameManager.instance.uiSection : SectionManager.instance.section;
        currentLanguage = GameManager.instance.dataManager.language;
        string localizedText = GameManager.instance.localizationManager.GetLocalizedText(section, id, registerTimestampAtLocalize).text;
        
        foreach (TextsToLocalize txt in textsToLocalize)
        {
            if (txt.tmProGui == null)
                Debug.LogWarning("A TMProGUI is null in " + name, gameObject);
            

            int pFrom = localizedText.IndexOf(">"+txt.stringTag+">", StringComparison.OrdinalIgnoreCase) + (">"+txt.stringTag+">").Length;
            int pTo = localizedText.LastIndexOf("<"+txt.stringTag+"<", StringComparison.OrdinalIgnoreCase);

            try
            {
                txt.tmProGui.text = localizedText.Substring(pFrom, pTo - pFrom);
            }
            catch (ArgumentOutOfRangeException)
            {
                txt.tmProGui.text = localizedText;
            }
        }

    }

}