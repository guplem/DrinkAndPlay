
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
    [SerializeField] private GameObject mainMenuSectionDoublePrefab;
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

        GameObject doubleSection = null;
        for (int s = 0; s < sectionsToDisplay.Length; s++)
        {
            int childIndex = Math.Abs(((float) s) % 2f) < 0.01f ? 0 : 1;

            if (childIndex == 0)
            {
                // OPTION A Keeping the prefab connection
                #if UNITY_EDITOR
                doubleSection = PrefabUtility.InstantiatePrefab(mainMenuSectionDoublePrefab) as GameObject;
                doubleSection.transform.SetParent(verticalScrollContentHolder.transform);
                doubleSection.transform.localScale = Vector3.one;
                
                #else
                
                // OPTION A Destroying the prefab connection
                doubleSection = Instantiate(mainMenuSectionDoublePrefab, verticalScrollContentHolder.transform);
                doubleSection.transform.localScale = Vector3.one;
                #endif
                doubleSection.transform.SetSiblingIndex(s/2+1);
            }

            GameObject game = doubleSection.transform.GetChild(childIndex).gameObject;
            game.GetComponent<MainMenuSection>().Setup(sectionsToDisplay[s]);
        }

        if (sectionsToDisplay.Length % 2 != 0)
            doubleSection.transform.GetChild(1).gameObject.SetActive(false);
            
            

        //Cocktails
        destroyExceptions.Clear();
        destroyExceptions.AddRange(exceptionsInHorizontalMenu.ToList());
        UtilsUI.DestroyContentsOf(horizontalScrollContentHolder.transform, destroyExceptions);

        for (int c = 0; c < cocktailsToDisplay.Length; c++)
        {
            // OPTION A Keeping the prefab connection
            GameObject cocktail = null;
            
            #if UNITY_EDITOR
            cocktail = PrefabUtility.InstantiatePrefab(mainMenuCocktailPrefab) as GameObject;
            cocktail.transform.SetParent(horizontalScrollContentHolder.transform);
            cocktail.transform.localScale = Vector3.one;
            
            #else

            // OPTION A Destroying the prefab connectioz
            cocktail = Instantiate(mainMenuCocktailPrefab, horizontalScrollContentHolder.transform);
            #endif
            
            cocktail.transform.SetSiblingIndex(c+1);
            cocktail.GetComponent<MainMenuCoctel>().Setup(cocktailsToDisplay[c]);
        }
    }

    public void OpenSectionDescription(Section selectedSection, GameObject originalImage)
    {
        sectionDescription.SetupAnimationOf(selectedSection.nameId, selectedSection.descriptionId, originalImage, selectedSection);
        gm.generalUi.Show(sectionDescription);
        currentSelectedSection = selectedSection;
    }
    
    public void OpenCocktailDescription(string nameId, string descriptionId, GameObject originalImage, Cocktail selectedCocktail)
    {
        cocktailDescription.SetupAnimationOf(nameId, descriptionId, originalImage, selectedCocktail);
        gm.generalUi.Show(cocktailDescription);
    }

    public void PlaySelectedSection()
    {
        gm.PlaySection(currentSelectedSection);
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
