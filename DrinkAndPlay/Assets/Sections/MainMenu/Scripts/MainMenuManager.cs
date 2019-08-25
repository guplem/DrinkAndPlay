using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class MainMenuManager : SectionManager
{
    [Header("Sections - Games")]
    [SerializeField] private GameObject verticalScrollContentHolder;
    [SerializeField] private GameObject mainMenuSectionPrefab;
    [SerializeField] private Section[] sectionsToDisplay;
    [SerializeField] private Description sectionDescription;

    [Header("Coctels - Recipes")]
    [SerializeField] private GameObject horizontalMenu;
    [SerializeField] private GameObject horizontalScrollContentHolder;
    [SerializeField] private GameObject mainMenuCocktailPrefab;
    [SerializeField] private Cocktail[] cocktailsToDisplay;
    [SerializeField] private Description cocktailDescription;
    private Section currentSelectedSection;


    private void Start()
    {
        //All localization (UI and MainMenu) already loaded

        GenerateMenu();
        Debug.Log("Started MainMenu' SectionManager.");
    }



    public void GenerateMenu()
    {
        List<Transform> destroyExceptions = new List<Transform>();

        //Games
        destroyExceptions.Add(horizontalMenu.transform);
        UtilsUI.DestroyContentsOf(verticalScrollContentHolder.transform, destroyExceptions);

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

        //Cocktails
        Debug.Log("Cocktail");
        destroyExceptions.Clear();
        Debug.Log("Cleared");
        UtilsUI.DestroyContentsOf(horizontalScrollContentHolder.transform, destroyExceptions);
        Debug.Log("Destroyed");

        for (int c = 0; c < cocktailsToDisplay.Length; c++)
        {
            Debug.Log("Creation " + c);
            GameObject cocktail = Instantiate(mainMenuCocktailPrefab, horizontalScrollContentHolder.transform);
            cocktail.transform.SetSiblingIndex(c);
            cocktail.GetComponent<MainMenuCoctel>().Setup(cocktailsToDisplay[c]);
            Debug.Log("END Creation " + c);
        }
    }

    public void OpenSectionDescription(string nameId, string descriptionId, GameObject originalImage)
    {
        sectionDescription.PlayOpenAnimationOf(nameId, descriptionId, originalImage);
        currentSelectedSection = section;
    }
    
    public void OpenCocktailDescription(string nameId, string descriptionId, GameObject originalImage)
    {
        cocktailDescription.PlayOpenAnimationOf(nameId, descriptionId, originalImage);
    }

    public void LoadSelectedSection()
    {
        GameManager.LoadSection(currentSelectedSection);
    }

}
