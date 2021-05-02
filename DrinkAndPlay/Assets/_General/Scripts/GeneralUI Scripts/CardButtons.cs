using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtons : MonoBehaviour
{
    [SerializeField] private ButtonType buttonType;
    enum ButtonType
    {
        Like,
        AddSentence,
        Share,
        NextTurn,
        Error
    }

    private void Start()
    {
        Button button = GetComponent<Button>();
        TurnsGameManager secMan;
            
        try
        {
            secMan = (TurnsGameManager) (SectionManager.instance);
        }
        catch (InvalidCastException)
        {
            //Must be added manually
            return; 
        }
        
        switch (buttonType)
        {
            case ButtonType.Like:
                button.onClick.AddListener(secMan.Like);
                break;
            case ButtonType.AddSentence:
                button.onClick.AddListener(secMan.AddSentence);
                break;
            case ButtonType.Share:
                button.onClick.AddListener(secMan.Share);
                break;
            case ButtonType.NextTurn:
                button.onClick.AddListener(secMan.NextButton);
                break;
            case ButtonType.Error:
                button.onClick.AddListener(secMan.ErrorButton);
                break;
            default:
                Debug.LogError("Button type of card not set properly for " + gameObject.name, gameObject);
                break;
        }
        
    }
}
