using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class MainMenu : SectionManager
{
    [Header("Sections - Games")]
    [SerializeField] private Section[] sectionsToDisplay;
    [SerializeField] private GameObject verticalScrollContentHolder;
    [SerializeField] private GameObject mainMenuGamePrefab;
    [Header("Coctels - Recipes")]
    [SerializeField] private Coctel[] coctelsToDisplay;
    [SerializeField] private GameObject HorizontalMenu;
    [SerializeField] private GameObject horizontalScrollContentHolder;
    [SerializeField] private GameObject mainMenuCoctelPrefab;


    void Start()
    {
        Debug.Log("Started MainMenu' SectionManager.");

        List<Transform> exceptions = new List<Transform>();
        exceptions.Add(HorizontalMenu.transform);
        DestroyContentsOf(verticalScrollContentHolder.transform, exceptions);

        for (int s = 0; s < sectionsToDisplay.Length; s++)
        {
            GameObject game = Instantiate(mainMenuGamePrefab, verticalScrollContentHolder.transform);
            game.transform.SetSiblingIndex(s);
            game.GetComponent<MainMenuGame>().Setup(sectionsToDisplay[s]);
        }

    }

    private void DestroyContentsOf(Transform ParentToClean, List<Transform> exceptions)
    {
        while (ParentToClean.childCount > exceptions.Count)
            foreach (Transform child in ParentToClean)
                if (!exceptions.Contains(child))
                    DestroyImmediate(child.gameObject);

        //Check that the elimination is ok
        foreach (Transform child in ParentToClean)
            if (!exceptions.Contains(child))
            {
                Debug.LogError("Error destroying the contents of " + ParentToClean.name + ". Maybe some of the exceptions is no inside the parentGameObject. The exceptions were the following ones:");
                foreach (Transform exception in exceptions)
                    Debug.LogError("  --> " + exception.name);
            }
    }
}
