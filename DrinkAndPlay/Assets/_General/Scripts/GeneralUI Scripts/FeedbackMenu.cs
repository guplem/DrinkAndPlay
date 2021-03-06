﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FeedbackMenu : MonoBehaviour
{
    private string theme;
    private string message;
    //private string author;

    [SerializeField] private Localizer topBarText;
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private TMP_InputField authorInputField;
    
    
    [SerializeField] private TextMeshProUGUI sendTextButton;
    [SerializeField] private Image sendIconButton;
    [SerializeField] private Button sendButton;

    public enum FeedbackType
    {
        General,
        Section,
        Game,
        Drink
    }
    
    private bool subscribed = false;
    private void OnEnable()
    {
        if (GameManager.instance == null)
            return;
        
        if (!subscribed)
        {
            sendIconButton.GetComponent<ColorSwitcher>().afterColorSwitchCall += SetColorsAfterSwitch;
            sendTextButton.GetComponent<ColorSwitcher>().afterColorSwitchCall += SetColorsAfterSwitch;
            subscribed = true;
        }
    }
    
    private void OnDisable()
    {
        if (subscribed)
        {
            sendIconButton.GetComponent<ColorSwitcher>().afterColorSwitchCall += SetColorsAfterSwitch;
            sendTextButton.GetComponent<ColorSwitcher>().afterColorSwitchCall += SetColorsAfterSwitch;
            subscribed = false;
        }
    }

    private void SetColorsAfterSwitch(LightDarkColor.ColorType obj)
    {
        UpdateVisuals();
    }


    public void Setup(FeedbackType feedbackType)
    {
        OnEnable();
        
        switch (feedbackType)
        {
            case FeedbackType.General:
                this.theme = "General";
                topBarText.Localize("Feedback");
                break;
            case FeedbackType.Section:
                topBarText.Localize("SendGameContent");
                this.theme = SectionManager.instance.section.ToString();
                break;
            case FeedbackType.Game:
                topBarText.Localize("SendGame");
                this.theme = "Game";
                break;
            case FeedbackType.Drink:
                topBarText.Localize("SendDrink");
                this.theme = "Drink";
                break;
        }

        authorInputField.text = GameManager.instance.dataManager.author;
        
        UpdateVisuals();
    }

    public void UpdateMessage(string newMessage)
    {
        this.message = newMessage;
        UpdateVisuals();
    }
    
    public void UpdateAuthor(string newAuthor)
    {
        GameManager.instance.dataManager.author = newAuthor;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        //TODO: Show warning if there is no internet connection
        
        ColorBlock buttonColors = sendButton.colors;
        
        if (AreContentsCorrect() /*&& Application.internetReachability != NetworkReachability.NotReachable*/) //This test only checks the connectivity,not if the internet is actually reachable
        {
            sendTextButton.color = new Color(sendTextButton.color.r, sendTextButton.color.g, sendTextButton.color.b, buttonColors.normalColor.a);
            sendIconButton.color = new Color(sendIconButton.color.r, sendIconButton.color.g, sendIconButton.color.b, buttonColors.normalColor.a);
            sendButton.interactable = true;
        }
        else
        {
            sendTextButton.color = new Color(sendTextButton.color.r, sendTextButton.color.g, sendTextButton.color.b, buttonColors.disabledColor.a);
            sendIconButton.color = new Color(sendIconButton.color.r, sendIconButton.color.g, sendIconButton.color.b, buttonColors.disabledColor.a);
            sendButton.interactable = false;
        }
    }
    
    private bool AreContentsCorrect()
    {
        return !(   string.IsNullOrEmpty(theme) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(GameManager.instance.dataManager.author)   /*TODO: Or there is no internet connection*/    );
    }

    public void SendForm()
    {
        if (!AreContentsCorrect())
        {
            Debug.LogWarning("Feedback sending aborted. A field is not correct.");
            return;
        }

        //StartCoroutine(Post(theme, message, GameManager.instance.dataManager.author));
        ForceSendForm(theme + " (" + GameManager.instance.dataManager.language + ")", message, GameManager.instance.dataManager.author);
        
        //messageInputField.onEndEdit.Invoke(messageInputField.text);
        messageInputField.text = "";
        //UtilsUI.ClearSelectedElement();

        UpdateVisuals();
        
        GameManager.instance.generalUi.CloseLastOpenUiElement();
    }
    
    public void ForceSendForm(string theme, string message, string author)
    {
        StartCoroutine(Post(theme, message, GameManager.instance.dataManager.author));
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
