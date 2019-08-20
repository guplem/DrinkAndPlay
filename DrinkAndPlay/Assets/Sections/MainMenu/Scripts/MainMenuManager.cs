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

        //Coctels
        destroyExceptions.Clear();
        UtilsUI.DestroyContentsOf(horizontalScrollContentHolder.transform, destroyExceptions);

        for (int c = 0; c < cocktailsToDisplay.Length; c++)
        {
            GameObject coctel = Instantiate(mainMenuCocktailPrefab, horizontalScrollContentHolder.transform);
            coctel.transform.SetSiblingIndex(c);
            coctel.GetComponent<MainMenuCoctel>().Setup(cocktailsToDisplay[c]);
        }
    }

    public void OpenSectionDescription(Section section, GameObject originalImage)
    {
        sectionDescription.PlayOpenAnimationOf(section, originalImage);
    }

}
