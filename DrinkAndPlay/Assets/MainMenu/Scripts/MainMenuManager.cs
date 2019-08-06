using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#pragma warning disable 0649
public class MainMenuManager : SectionManager
{
    [Header("Sections - Games")]
    [SerializeField] private GameObject verticalScrollContentHolder;
    [SerializeField] private GameObject mainMenuGamePrefab;
    [SerializeField] private Section[] sectionsToDisplay;

    [Header("Coctels - Recipes")]
    [SerializeField] private GameObject HorizontalMenu;
    [SerializeField] private GameObject horizontalScrollContentHolder;
    [SerializeField] private GameObject mainMenuCoctelPrefab;
    [SerializeField] private Coctel[] coctelsToDisplay;


    void Start()
    {
        GenerateMenu();
        Debug.Log("Started MainMenu' SectionManager.");
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

    public void GenerateMenu()
    {
        List<Transform> exceptions = new List<Transform>();

        //Games
        exceptions.Add(HorizontalMenu.transform);
        DestroyContentsOf(verticalScrollContentHolder.transform, exceptions);

        for (int s = 0; s < sectionsToDisplay.Length; s++)
        {
            // Keeping the prefab connection
            /*
            GameObject game = PrefabUtility.InstantiatePrefab(mainMenuGamePrefab) as GameObject;
            game.transform.SetParent(verticalScrollContentHolder.transform);
            */

            // Destroying the prefab connection
            GameObject game = Instantiate(mainMenuGamePrefab, verticalScrollContentHolder.transform);

            game.transform.SetSiblingIndex(s);
            game.GetComponent<MainMenuSection>().Setup(sectionsToDisplay[s]);
        }

        //Coctels
        exceptions.Clear();
        DestroyContentsOf(horizontalScrollContentHolder.transform, exceptions);

        for (int c = 0; c < coctelsToDisplay.Length; c++)
        {
            GameObject coctel = Instantiate(mainMenuCoctelPrefab, horizontalScrollContentHolder.transform);
            coctel.transform.SetSiblingIndex(c);
            coctel.GetComponent<MainMenuCoctel>().Setup(coctelsToDisplay[c]);
        }
    }

    public void OpenSectionDescription(Section section)
    {
        Debug.Log("Opening description of " + section);
    }

    //Run before build

}
