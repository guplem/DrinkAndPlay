using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public struct TextsToLocalize {
    public string stringTag;
    [Tooltip("Should the text be punctuated at the end if it is not?")]
    public bool forcePunctuation;
    public TextMeshProUGUI tmProGui;
    public UnityEvent methodToCallAfter;


    public TextsToLocalize(string stringTag, TextMeshProUGUI tmProGui, UnityEvent methodToCallAfter, bool forcePunctuation)
    {
        this.stringTag = stringTag;
        this.tmProGui = tmProGui;
        this.methodToCallAfter = methodToCallAfter;
        this.forcePunctuation = forcePunctuation;
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

    private Language currentLanguage = null;
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
        if (localizationFile == null)
            Debug.LogWarning("Null localization file in " + gameObject.name, gameObject);
        
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
        if (string.IsNullOrEmpty(id))
            Debug.LogError("Trying to set a null 'id' into the Localizer in object " + gameObject.name, gameObject);
        
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

    public LocalizedText GetLocalizedText()
    {
        if (!Application.isPlaying)
            return null;

        if (string.IsNullOrEmpty(id))
        {
            GameObject go = gameObject;
            Debug.LogWarning("Trying to localize the object '" + go.name + "' but the 'id' in the 'Localizer' component is null or empty.", go);
            return null;
        }
        
        if (localizationFile == null)
        {
            GameObject go = gameObject;
            Debug.LogWarning("Trying to localize the object '" + go.name + "' but the 'localizationFile' in the 'Localizer' component is null.", go);
            return null;
        }
        
        return GameManager.instance.localizationManager.SearchLocalizedText(localizationFile, id, registerTimestampAtLocalize);
    }
    
    public void Localize()
    {
        LocalizedText lt = GetLocalizedText();

        if (lt != null)
        {
            currentLanguage = GameManager.instance.dataManager.language;
            SetText(lt);
        }
    }

    public void SetText(LocalizedText localizedText)
    {
        SetId(localizedText.id);
        
        string text = CleanTags(localizedText.text);

        

        List<TextsToLocalize> savedForLater = new List<TextsToLocalize>(); 
        foreach (TextsToLocalize txt in textsToLocalize)
        {
            if (txt.tmProGui == null)
                Debug.LogWarning("A TMProGUI is null in a text to localize " + name, gameObject);
            
            if (string.IsNullOrEmpty(txt.stringTag))
            {
                savedForLater.Add(txt);
            }
            else
            {
                int tagLength = (">" + txt.stringTag + ">").Length;
                int pFrom = text.IndexOf(">"+txt.stringTag+">", StringComparison.OrdinalIgnoreCase) + tagLength;
                int pTo = text.LastIndexOf("<"+txt.stringTag+"<", StringComparison.OrdinalIgnoreCase);
            
                if (pFrom >= pTo || pFrom < 0 || pTo < 0) //Error handling
                {
                    ApplyText(txt.tmProGui, "", false);
                    if (txt.methodToCallAfter != null) txt.methodToCallAfter.Invoke();
                }
                else
                {
                    int searchedTextLength = pTo - pFrom;
                    ApplyText(txt.tmProGui, text.Substring(pFrom, searchedTextLength), txt.forcePunctuation);
                    text = text.Remove(pFrom - tagLength,  searchedTextLength + tagLength*2);
                    if (txt.methodToCallAfter != null) txt.methodToCallAfter.Invoke();
                }
            }
        }

        if (savedForLater.Count > 1)
            Debug.LogWarning("The text with id " + localizedText.id + " have multiple separated strings to ad in the default container of the localizer in " + gameObject.name, gameObject);
        
        foreach (TextsToLocalize txt in savedForLater)
        {
            ApplyText(txt.tmProGui, text, txt.forcePunctuation);
            if (txt.methodToCallAfter != null) txt.methodToCallAfter.Invoke();
        }
    }

    private string CleanTags(string text)
    {
        MatchCollection matchList = Regex.Matches(text, @"<\s*\w+\s*>|>\s*\w+\s*>|<\s*\w+\s*<"); // Test web: https://regexr.com/
        List<string> stringList = matchList.Cast<Match>().Select(match => match.Value).ToList();
        
        foreach (string match in stringList)
            text = text.Replace(match, Regex.Replace(match, @"\s+", ""));

        return text;
    }

    private static void ApplyText(TextMeshProUGUI tmProGui, string text, bool forcePunctuation)
    {
        text = ApplyPlayersTo(text);
        text = text.Trim();
        if (forcePunctuation)
            text = ApplyPunctuation(text);
        
        tmProGui.richText = true;
        tmProGui.text = text;
    }

    private static string ApplyPunctuation(string text)
    {
        if (!char.IsPunctuation(text.Last()))
            text += ".";
        return text;
    }

    private static string ApplyPlayersTo(string text)
    {
        HashSet<Player> alreadyIncludedPlayers = new HashSet<Player>();

        if (text.IndexOf("<p>", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            Regex rgx = new Regex("<p>", RegexOptions.IgnoreCase); 
            do
            {
                Player currentPlayer = GameManager.instance.dataManager.GetCurrentPlayer();
                text = rgx.Replace(text, currentPlayer.nameTrimmed, 1);
                alreadyIncludedPlayers.Add(currentPlayer);
            } while (text.IndexOf("<p>", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        if (text.IndexOf("<pr>", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            Regex rgx = new Regex("<pr>", RegexOptions.IgnoreCase); 
            do
            {
                Player randPlayer = GameManager.instance.dataManager.GetRandomEnabledPlayer(alreadyIncludedPlayers.ToList());
                text = rgx.Replace(text, randPlayer.nameTrimmed, 1);
                alreadyIncludedPlayers.Add(randPlayer);
            } while (text.IndexOf("<pr>", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        return text;
    }
}