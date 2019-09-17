using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class ImprovedInputField : MonoBehaviour
{
    private string defaultPlaceHolderText = "";
    private string editText = "";
    private TMP_InputField inputField;
    private TextMeshProUGUI placeHolder;
    private TouchScreenKeyboard kb;

    private void Start()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();
        placeHolder = (TextMeshProUGUI) inputField.placeholder;
        defaultPlaceHolderText = placeHolder.text;
        
        inputField.onSelect.AddListener(RemovePlaceHolder);
        inputField.onDeselect.AddListener(SetPlaceHolder);
        inputField.onEndEdit.AddListener(OnEndEdit);
        inputField.onValueChanged.AddListener(OnEditting);
    }

    private void RemovePlaceHolder(string currentText)
    {
        Debug.Log("******************* RemovePlaceHolder called with '" + currentText + "'");
        
        /*if (string.IsNullOrEmpty(currentText))
        {
            if (string.IsNullOrEmpty(defaultPlaceHolderText))
                defaultPlaceHolderText = placeHolder.text;

            placeHolder.text = "";
        }*/
        
        placeHolder.text = "";
    }

    private void SetPlaceHolder(string currentText)
    {
        Debug.Log("******************* SetPlaceHolder called with '" + currentText + "'");
        
        if (!string.IsNullOrEmpty(defaultPlaceHolderText))
            if (string.IsNullOrEmpty(currentText))
                placeHolder.text = defaultPlaceHolderText;
    }
    
    
  
    public void OnEditting(string currentText)
    {
        Debug.Log("******************* OnEditting called with '" + currentText + "'");
        
        /*Debug.Log("******************* OnEditting with '" + currentText + "'");
        
        if (!Input.GetKey(KeyCode.Escape))
        {
            if (!string.IsNullOrEmpty(currentText))
            {
                editText = currentText;
                Debug.Log("###################### Updated text to: " + editText);
            }
        }*/
    }
  
    private void OnEndEdit(string currentText)
    {
        Debug.Log("******************* OnEndEdit called with '" + currentText + "'");
        
        /*if (Input.GetKey(KeyCode.Escape))
        {
            inputField.text = editText;
            Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$ Setted input field text to: " + editText);
            
            if (string.IsNullOrEmpty(editText))
                SetPlaceHolder(editText);
        }*/
    }
    
    
}
