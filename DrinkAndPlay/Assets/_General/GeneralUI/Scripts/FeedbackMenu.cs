using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class FeedbackMenu : MonoBehaviour
{
    private string theme;
    private string message;
    private string author;

    [SerializeField] private Localizer topBarText;
    [SerializeField] private TMP_InputField messageInputField;

    public enum FeedbackTime
    {
        General,
        Section,
        Cocktail
    }


    public void Setup(FeedbackTime feedbackTime)
    {
        switch (feedbackTime)
        {
            case FeedbackTime.General:
                this.theme = "General";
                topBarText.Localize("Feedback");
                break;
            case FeedbackTime.Section:
                topBarText.Localize("SendGameContent");
                this.theme = SectionManager.instance.section.ToString();
                break;
            case FeedbackTime.Cocktail:
                topBarText.Localize("SendCocktail");
                this.theme = "Cocktail";
                break;
        }
        
    }

    public void UpdateMessage(string newMessage)
    {
        this.message = newMessage;
    }
    
    public void UpdateAuthor(string newAuthor)
    {
        this.author = newAuthor;
    }

    public void SendForm()
    {
        if (string.IsNullOrEmpty(theme) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(author))
        {
            Debug.LogWarning("Feedback sending aborted. A field is not correct.");
            return;
        }

        StartCoroutine(Post(theme, message, author));

        //messageInputField.onEndEdit.Invoke(messageInputField.text);
        messageInputField.text = "";
        //UtilsUI.ClearSelectedElement();
    }
    
    IEnumerator Post(string theme, string message, string author) {
        
        string BASE_URL = "https://docs.google.com/forms/d/e/1FAIpQLSdRWP5QAVV6W0bYQtkpAh0co7cK5PA0RzgKBirNidNHfHfiOw/formResponse";
        
        WWWForm form = new WWWForm();
        form.AddField("entry.945393892", theme);
        form.AddField("entry.2037568777", message);
        form.AddField("entry.1144089731", author);
        
        //byte[] rawData = form.data;
        //WWW www = new WWW(BASE_URL, rawData);
        //yield return www;
        
        UnityWebRequest www = UnityWebRequest.Post(BASE_URL, form);
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            Debug.Log("Form upload complete.");
        }
    }

}
