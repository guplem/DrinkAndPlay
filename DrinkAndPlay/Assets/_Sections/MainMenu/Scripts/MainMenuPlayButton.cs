using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayButton : MonoBehaviour
{
    public void LoadSelectedSection()
    {
        GetComponent<ButtonAnimation>().MidAnimEvent += LoadSelectedSectionAtEvent;
    }

    private void LoadSelectedSectionAtEvent()
    {
        ((MainMenuManager)SectionManager.instance).LoadSelectedSection();
        GetComponent<ButtonAnimation>().MidAnimEvent -= LoadSelectedSectionAtEvent;
    }
}
