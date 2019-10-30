using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class ImprovedInputField : MonoBehaviour
{
    private TMP_InputField inputField;
    
    //PlaceHolder
    private TextMeshProUGUI placeHolder;
    private string defaultPlaceHolderText = "";
    
    //CloseKeyboardHandeling
    private bool keepOldTextInField = false;
    private string editText = "";
    private string oldEditText = "";

    private void Start()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();
        
        //PlaceHolder
        placeHolder = (TextMeshProUGUI) inputField.placeholder;
        defaultPlaceHolderText = GetDefaultPlaceHolderText();
        inputField.onSelect.AddListener(RemovePlaceHolder);
        inputField.onDeselect.AddListener(CheckAndSetPlaceHolder);
        
        //CloseKeyboardHandeling
        placeHolder = (TextMeshProUGUI) inputField.placeholder;
        inputField.onTouchScreenKeyboardStatusChanged.AddListener(ReportHidedKeyboard);
        inputField.onValueChanged.AddListener(Editing);
        inputField.onEndEdit.AddListener(EndEdit);
    }




    #region PlaceHolder
    
    private string GetDefaultPlaceHolderText()
    {
        Localizer placeHolderLocalizer = placeHolder.GetComponent<Localizer>();
        
        if (placeHolderLocalizer != null)
            return placeHolderLocalizer.GetLocalizedText().text;
        
        return placeHolder.text;
    }
    
    private void RemovePlaceHolder(string currentText)
    {
        placeHolder.text = "";
    }

    private void CheckAndSetPlaceHolder(string currentText)
    {
        if (!string.IsNullOrEmpty(defaultPlaceHolderText))
            if (string.IsNullOrEmpty(currentText))
                placeHolder.text = defaultPlaceHolderText;
    }

    #endregion


    #region CloseKeyboardHandeling

    private void ReportHidedKeyboard(TouchScreenKeyboard.Status newStatus)
    {
        if (newStatus == TouchScreenKeyboard.Status.Canceled)
            keepOldTextInField = true;
    }

    private void Editing(string currentText)
    {
        oldEditText = editText;
        editText = currentText;
    }
  
    private void EndEdit(string currentText)
    {
        if (keepOldTextInField && !string.IsNullOrEmpty(oldEditText))
        {
            //IMPORTANT ORDER
            editText = oldEditText;
            inputField.text = oldEditText;
            
            keepOldTextInField = false;
        }
    }

    #endregion
    
}