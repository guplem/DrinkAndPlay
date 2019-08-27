using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        Debug.Log("Message updated to : '" + message+"'");
    }
    
    public void UpdateAuthor(string newAuthor)
    {
        this.author = newAuthor;
    }

    public void SendForm()
    {
        Debug.Log("THEME: " + theme);
        Debug.Log("MESSAGE: " + message);
        Debug.Log("AUTHOR: " + author);

        if (string.IsNullOrEmpty(theme) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(author))
        {
            Debug.Log("Feedback sending aborted.");
            return;
        }

        
        //messageInputField.onEndEdit.Invoke(messageInputField.text);
        messageInputField.text = "";
        //UtilsUI.ClearSelectedElement();
        
        // TODO: Send it
    }

}
