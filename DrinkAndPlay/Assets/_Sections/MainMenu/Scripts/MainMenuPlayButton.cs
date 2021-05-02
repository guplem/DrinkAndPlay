using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayButton : MonoBehaviour
{
    public void PlaySelectedSection()
    {
        GetComponent<ButtonAnimation>().MidAnimEvent += LoadSelectedSectionAtEvent;
    }

    private void LoadSelectedSectionAtEvent()
    {
        ((MainMenuManager)SectionManager.instance).PlaySelectedSection();
        GetComponent<ButtonAnimation>().MidAnimEvent -= LoadSelectedSectionAtEvent;
    }
}
