using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Transform[] exceptionsInVerticalMenu; 

    [Header("Coctels - Recipes")]
    [SerializeField] private GameObject horizontalMenu;
    [SerializeField] private GameObject horizontalScrollContentHolder;
    [SerializeField] private GameObject mainMenuCocktailPrefab;
    [SerializeField] private Cocktail[] cocktailsToDisplay;
    [SerializeField] private Description cocktailDescription;
    private Section currentSelectedSection;
    [SerializeField] private Transform[] exceptionsInHorizontalMenu; 


    private void Start()
    {
        //All localization (UI and MainMenu) already loaded

        GenerateMenu();
        
        //Debug.Log("Started MainMenu' SectionManager.");
    }



    public void GenerateMenu()
    {
        List<Transform> destroyExceptions = new List<Transform>();

        //Games
        destroyExceptions.Add(horizontalMenu.transform);
        destroyExceptions.AddRange(exceptionsInVerticalMenu.ToList());
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

            game.transform.SetSiblingIndex(s+1);
            game.GetComponent<MainMenuSection>().Setup(sectionsToDisplay[s]);
        }

        //Cocktails
        destroyExceptions.Clear();
        destroyExceptions.AddRange(exceptionsInHorizontalMenu.ToList());
        UtilsUI.DestroyContentsOf(horizontalScrollContentHolder.transform, destroyExceptions);

        for (int c = 0; c < cocktailsToDisplay.Length; c++)
        {
            GameObject cocktail = Instantiate(mainMenuCocktailPrefab, horizontalScrollContentHolder.transform);
            cocktail.transform.SetSiblingIndex(c+1);
            cocktail.GetComponent<MainMenuCoctel>().Setup(cocktailsToDisplay[c]);
        }
    }

    public void OpenSectionDescription(Section selectedSection, GameObject originalImage)
    {
        sectionDescription.PlayOpenAnimationOf(selectedSection.nameId, selectedSection.descriptionId, originalImage);
        currentSelectedSection = selectedSection;
    }
    
    public void OpenCocktailDescription(string nameId, string descriptionId, GameObject originalImage)
    {
        cocktailDescription.PlayOpenAnimationOf(nameId, descriptionId, originalImage);
    }

    public void LoadSelectedSection()
    {   
        GameManager.LoadSection(currentSelectedSection);
    }

    public void OpenConfigMenu()
    {
        GameManager.instance.generalUi.OpenConfigMenu();
    }

    public void OpenFeedbackMenu()
    {
        GameManager.instance.generalUi.OpenFeedbackMenu();
    }

    public void OpenSendNewCocktailFeedbakc()
    {
        GameManager.instance.generalUi.OpenFeedbackMenuCocktails();
    }

}
