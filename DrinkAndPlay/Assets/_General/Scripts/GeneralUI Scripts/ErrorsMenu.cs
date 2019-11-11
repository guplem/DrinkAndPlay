using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ErrorsMenu : MonoBehaviour
{
    private bool typo;
    private bool offensive;
    private bool nonInclusive;

    public void Setup(TextInTurnsGame currentTextInCard)
    {
        // TODO save it
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

    private void SendForm()
    {
        ForceSendForm("ERROR", GetMessage(), SectionManager.instance.section.ToString());
    }

    private string GetMessage()
    {
        
        
        return "MESSAGE - TODO";
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
