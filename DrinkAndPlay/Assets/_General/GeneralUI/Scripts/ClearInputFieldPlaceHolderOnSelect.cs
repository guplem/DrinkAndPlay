using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearInputFieldPlaceHolderOnSelect : MonoBehaviour
{
    private string defaultPlaceHolderText = "";
    private TMP_InputField inputField;
    private TextMeshProUGUI placeHolder;

    private void Start()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();
        placeHolder = (TextMeshProUGUI) inputField.placeholder;
    }

    public void OnSelect(string stringTxt)
    {
        if (string.IsNullOrEmpty(stringTxt))
        {
            if (string.IsNullOrEmpty(defaultPlaceHolderText))
                defaultPlaceHolderText = placeHolder.text;
        
            placeHolder.text = "";
        }
    }

    public void OnDeselect(string stringTxt)
    {
        if (!string.IsNullOrEmpty(defaultPlaceHolderText))
            if (string.IsNullOrEmpty(stringTxt))
                placeHolder.text = defaultPlaceHolderText;
    }
}
