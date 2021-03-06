﻿using System.Collections;
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
    
    public void ClickHelpButton()
    {
        GameManager.instance.generalUi.OpenHelpMenu(SectionManager.instance.section);
    }
        
    public void ClickPlayersButton()
    {
        GameManager.instance.generalUi.OpenPlayersMenu();
    }

}
