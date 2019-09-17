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
    private string oldEditText = "";
    private TMP_InputField inputField;
    private TextMeshProUGUI placeHolder;
    private bool keepOldTextInField = false;

    private void Start()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();
        placeHolder = (TextMeshProUGUI) inputField.placeholder;
        defaultPlaceHolderText = placeHolder.text;
        
        inputField.onSelect.AddListener(RemovePlaceHolder);
        inputField.onDeselect.AddListener(SetPlaceHolder);
        inputField.onEndEdit.AddListener(OnEndEdit);
        inputField.onValueChanged.AddListener(OnEditting);
        
        inputField.onTouchScreenKeyboardStatusChanged.AddListener(ReportChangeStatus);
        
    }

    private void ReportChangeStatus(TouchScreenKeyboard.Status newStatus)
    {
        Debug.Log("/////////////////////////////////////////////// New keyboard status: " + newStatus);
        if (newStatus == TouchScreenKeyboard.Status.Canceled)
            keepOldTextInField = true;
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
        
        oldEditText = editText;
        editText = currentText;
    }
  
    private void OnEndEdit(string currentText)
    {
        Debug.Log("******************* OnEndEdit called with '" + currentText + "'. Keep text = " + keepOldTextInField);
        
        if (keepOldTextInField && !string.IsNullOrEmpty(oldEditText))
        {
            inputField.text = oldEditText;
            keepOldTextInField = false;
        }
    }
    
    
}
