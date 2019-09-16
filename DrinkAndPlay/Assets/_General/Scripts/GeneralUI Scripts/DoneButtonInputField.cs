using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoneButtonInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void ClickDoneButton()
    {
        inputField.onEndEdit.Invoke(inputField.text);
    }
}
