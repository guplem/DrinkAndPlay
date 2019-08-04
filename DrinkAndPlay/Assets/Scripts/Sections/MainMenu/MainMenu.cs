using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : SectionManager
{

    [SerializeField] private Section[] sectionsToDisplay;

    void Start()
    {
        Debug.Log("Started MainMenu' SectionManager.");
    }

}
