using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUiButtons : MonoBehaviour
{
    public void ClickBackButton()
    {
        GameManager.instance.generalUi.CloseLastOpenUiElement();
    }

    public void ClickConfigurationButton()
    {
        GameManager.instance.generalUi.OpenConfigMenu();
    }

}
