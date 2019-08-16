using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainMenuManager : SectionManager
{
    [Header("Sections - Games")]
    [SerializeField] private GameObject verticalScrollContentHolder;
    [SerializeField] private GameObject mainMenuSectionPrefab;
    [SerializeField] private Section[] sectionsToDisplay;

    [Header("Coctels - Recipes")]
    [SerializeField] private GameObject horizontalMenu;
    [SerializeField] private GameObject horizontalScrollContentHolder;
    [SerializeField] private GameObject mainMenuCocktailPrefab;
    [SerializeField] private Cocktail[] cocktailsToDisplay;


    void Start()
    {
        //All localization (UI and MainMenu) already loaded

        GenerateMenu();
        Debug.Log("Started MainMenu' SectionManager.");
    }

    private static void DestroyContentsOf(Transform parentToClean, List<Transform> exceptions)
    {
        while (parentToClean.childCount > exceptions.Count)
            foreach (Transform child in parentToClean)
                if (!exceptions.Contains(child))
                    DestroyImmediate(child.gameObject);

        //Check that the elimination is ok
        foreach (Transform child in parentToClean)
            if (!exceptions.Contains(child))
            {
                Debug.LogError("Error destroying the contents of " + parentToClean.name + ". Maybe some of the exceptions is no inside the parentGameObject. The exceptions were the following ones:");
                foreach (Transform exception in exceptions)
                    Debug.LogError("  --> " + exception.name);
            }
    }

    public void GenerateMenu()
    {
        List<Transform> destroyExceptions = new List<Transform>();

        //Games
        destroyExceptions.Add(horizontalMenu.transform);
        DestroyContentsOf(verticalScrollContentHolder.transform, destroyExceptions);

        for (int s = 0; s < sectionsToDisplay.Length; s++)
        {
            // Keeping the prefab connection
            /*
            GameObject game = PrefabUtility.InstantiatePrefab(mainMenuGamePrefab) as GameObject;
            game.transform.SetParent(verticalScrollContentHolder.transform);
            */

            // Destroying the prefab connection
            GameObject game = Instantiate(mainMenuSectionPrefab, verticalScrollContentHolder.transform);

            game.transform.SetSiblingIndex(s);
            game.GetComponent<MainMenuSection>().Setup(sectionsToDisplay[s]);
        }

        //Coctels
        destroyExceptions.Clear();
        DestroyContentsOf(horizontalScrollContentHolder.transform, destroyExceptions);

        for (int c = 0; c < cocktailsToDisplay.Length; c++)
        {
            GameObject coctel = Instantiate(mainMenuCocktailPrefab, horizontalScrollContentHolder.transform);
            coctel.transform.SetSiblingIndex(c);
            coctel.GetComponent<MainMenuCoctel>().Setup(cocktailsToDisplay[c]);
        }
    }

    public void OpenSectionDescription(Section section)
    {
        Debug.Log("Opening description of " + section);
        GameManager.LoadSection(section); //TODO: remove
    }

}
