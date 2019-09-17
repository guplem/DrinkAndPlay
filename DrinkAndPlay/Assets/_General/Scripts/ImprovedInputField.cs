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
        defaultPlaceHolderText = placeHolder.text;
        inputField.onSelect.AddListener(RemovePlaceHolder);
        inputField.onDeselect.AddListener(CheckAndSetPlaceHolder);
        
        //CloseKeyboardHandeling
        inputField.onEndEdit.AddListener(EndEdit);
        inputField.onValueChanged.AddListener(Editting);
        inputField.onTouchScreenKeyboardStatusChanged.AddListener(ReportChangeStatus);
        
    }


    #region PlaceHolder

    private void RemovePlaceHolder(string currentText)
    {
        Debug.Log("**** @@@ RemovePlaceHolder called with '" + currentText + "'");
        
        /*if (string.IsNullOrEmpty(currentText))
        {
            if (string.IsNullOrEmpty(defaultPlaceHolderText))
                defaultPlaceHolderText = placeHolder.text;

            placeHolder.text = "";
        }*/
        
        placeHolder.text = "";
    }

    private void CheckAndSetPlaceHolder(string currentText)
    {
        Debug.Log("**** @@@ CheckAndSetPlaceHolder called with '" + currentText + "'");
        
        if (!string.IsNullOrEmpty(defaultPlaceHolderText))
            if (string.IsNullOrEmpty(currentText))
                placeHolder.text = defaultPlaceHolderText;
    }

    #endregion


    #region CloseKeyboardHandeling

    private void ReportChangeStatus(TouchScreenKeyboard.Status newStatus)
    {
        Debug.Log("cdbf /// New keyboard status: " + newStatus);
        if (newStatus == TouchScreenKeyboard.Status.Canceled)
            keepOldTextInField = true;
    }
  
    public void Editting(string currentText)
    {
        Debug.Log("cdbf @@@ Editting called with '" + currentText + "'");
        
        oldEditText = editText;
        Debug.Log("cdbf ### EDIT1 OLDtxt: " + oldEditText + ", NEWtxt: " + editText);
        editText = currentText;
        
        Debug.Log("cdbf ### EDIT2f OLDtxt: " + oldEditText + ", NEWtxt: " + editText);
    }
  
    private void EndEdit(string currentText)
    {
        Debug.Log("cdbf @@@ EndEdit called with '" + currentText + "'. Keep text = " + keepOldTextInField);
        
        if (keepOldTextInField && !string.IsNullOrEmpty(oldEditText))
        {
            //IMPORTANT ORDER
            editText = oldEditText;
            Debug.Log("cdbf ### END1 OLDtxt: " + oldEditText + ", NEWtxt: " + editText);
            inputField.text = oldEditText;
            Debug.Log("cdbf ### END2 OLDtxt: " + oldEditText + ", NEWtxt: " + editText);
            
            keepOldTextInField = false;
            Debug.Log("cdbf ### END3f OLDtxt: " + oldEditText + ", NEWtxt: " + editText);
        }
    }

    #endregion
    
}