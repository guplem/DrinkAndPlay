using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ErrorsMenu : MonoBehaviour
{
    private bool typo;
    private bool offensive;
    private bool nonInclusive;
    private bool other;
    private TextInTurnsGame currentTextOfError;
    [SerializeField] private Toggle typoToggle;
    [SerializeField] private Toggle offensiveToggle;
    [SerializeField] private Toggle nonInclusiveToggle;
    [SerializeField] private Toggle otherToggle;

    public void Setup(TextInTurnsGame currentTextInCard)
    {
        currentTextOfError = currentTextInCard;
        ClearToggles();
    }

    private void ClearToggles()
    {
        typoToggle.isOn = false;
        offensiveToggle.isOn = false;
        nonInclusiveToggle.isOn = false;
        otherToggle.isOn = false;
    }

    public void SetTypo(bool newValue)
    {
        typo = newValue;
    }
    
    public void SetOffensive(bool newValue)
    {
        offensive = newValue;
    }
    
    public void SetNonInclusive(bool newValue)
    {
        nonInclusive = newValue;
    }
    
    public void SetOther(bool newValue)
    {
        other = newValue;
    }

    public void SendForm()
    {
        ForceSendForm("ERROR", GetMessage(), SectionManager.instance.section.ToString());
        
        ClearToggles();
        
        GameManager.instance.generalUi.CloseLastOpenUiElement();
    }

    private string GetMessage()
    {
        string message = "";

        if (typo) message += "TYPO, ";
        if (offensive) message += "OFFENSIVE, ";
        if (nonInclusive) message += "NON-INCLUSIVE, ";
        if (other) message += "OTHER, ";
        if (currentTextOfError != null) message += "Localized Text: " + currentTextOfError.localizedText + ", Localization File: " + currentTextOfError.localizationFile.ToString() + ".";

        return message;
    }

    public void ForceSendForm(string theme, string message, string author)
    {
        StartCoroutine(Post(theme, message, author));
    }
    
    IEnumerator Post(string theme, string message, string author) {
        
        string BASE_URL = "https://docs.google.com/forms/d/e/1FAIpQLSdRWP5QAVV6W0bYQtkpAh0co7cK5PA0RzgKBirNidNHfHfiOw/formResponse";
        
        WWWForm form = new WWWForm();
        form.AddField("entry.945393892", theme);
        form.AddField("entry.2037568777", message);
        form.AddField("entry.1144089731", author);

        UnityWebRequest www = UnityWebRequest.Post(BASE_URL, form);
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            Debug.Log("'Error form' upload complete.");
        }
    }
}
