﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUiButtons : MonoBehaviour
{
    public void ClickBackButton()
    {
        Debug.Log("Clicked 'Back' button.");
        GameManager.Instance.generalUI.CloseLastOpenUiElement();
    }

    public void ClickConfigurationButton()
    {
        Debug.Log("Clicked 'Configuration' button.");
        GameManager.Instance.generalUI.OpenConfigMenu();
    }

    public void ClickLikeButton()
    {
        Debug.Log("Clicked 'Like' button.");
    }

    public void ClickAddButton()
    {
        Debug.Log("Clicked 'Add' button.");
    }

    public void ClickShareButton()
    {
        Debug.Log("Clicked 'Share' button.");
    }

}
