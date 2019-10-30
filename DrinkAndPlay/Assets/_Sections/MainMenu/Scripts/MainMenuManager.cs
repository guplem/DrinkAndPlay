
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
    [SerializeField] private SectionDescription sectionDescription;
    [SerializeField] private Transform[] exceptionsInVerticalMenu; 

    [Header("Cocktails - Recipes")]
    //[SerializeField] private GameObject horizontalMenuCocktails;
    [SerializeField] private GameObject horizontalScrollContentHolderCocktails;
    [SerializeField] private Cocktail[] cocktailsToDisplay;
    [SerializeField] private Transform[] exceptionsInHorizontalMenuCocktails; 
    
    [Header("Cubatas - Recipes")]
    //[SerializeField] private GameObject horizontalMenuCubatas;
    [SerializeField] private GameObject horizontalScrollContentHolderCubatas;
    [SerializeField] private Cocktail[] cubatasToDisplay;
    [SerializeField] private Transform[] exceptionsInHorizontalMenuCubatas; 
    
    [Space(20)]
    [SerializeField] private GameObject mainMenuCocktailPrefab;
    [SerializeField] private Description cocktailDescription;
    
    private Section currentSelectedSection;
    private Cocktail currentSelectedCocktail;

    private void Start()
    {
        //All localization (UI and MainMenu) already loaded

        GenerateMenu();
        
        //Debug.Log("Started MainMenu' SectionManager.");
    }



    public void GenerateMenu()
    {
        List<Transform> destroyExceptions = new List<Transform>();

        #region GAMES

        //Games
        //destroyExceptions.Add(horizontalMenuCocktails.transform);
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
                // OPTION B Destroying the prefab connection
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

        #endregion
        
        
        //Cocktails
        SpawnCocktails(cocktailsToDisplay, exceptionsInHorizontalMenuCocktails, horizontalScrollContentHolderCocktails);
        SpawnCocktails(cubatasToDisplay, exceptionsInHorizontalMenuCubatas, horizontalScrollContentHolderCubatas);
    }

    public void SpawnCocktails(Cocktail[] drinkToDisplay, Transform[] exceptionsInHorizontalMenu,
        GameObject scrollContentHolder)
    {
        List<Transform> destroyExceptions = new List<Transform>();
        destroyExceptions.Clear();
        destroyExceptions.AddRange(exceptionsInHorizontalMenu.ToList());
        UtilsUI.DestroyContentsOf(scrollContentHolder.transform, destroyExceptions);

        for (int c = 0; c < drinkToDisplay.Length; c++)
        {
            // OPTION A Keeping the prefab connection
            GameObject drink = null;
            
#if UNITY_EDITOR
            drink = PrefabUtility.InstantiatePrefab(mainMenuCocktailPrefab) as GameObject;
            drink.transform.SetParent(scrollContentHolder.transform);
            drink.transform.localScale = Vector3.one;
            
#else

            // OPTION A Destroying the prefab connectioz
            drink = Instantiate(mainMenuCocktailPrefab, scrollContentHolder.transform);
#endif
            
            drink.transform.SetSiblingIndex(c+1);
            drink.GetComponent<MainMenuCoctel>().Setup(drinkToDisplay[c]);
        }
    }

    public void OpenSectionDescription(Section selectedSection, GameObject originalImage)
    {
        if (isDescOpen())
            return;
        
        //First step
        gm.generalUi.Show(sectionDescription);
        sectionDescription.SetupAnimationOf(selectedSection.nameId, selectedSection.descriptionId, originalImage, selectedSection);
        
        currentSelectedSection = selectedSection;
    }
    
    public void OpenCocktailDescription(string nameId, string descriptionId, GameObject originalImage, Cocktail selectedCocktail)
    {
        if (isDescOpen())
            return;
        
        //First step
        gm.generalUi.Show(cocktailDescription);
        cocktailDescription.SetupAnimationOf(nameId, descriptionId, originalImage, selectedCocktail);

        currentSelectedCocktail = selectedCocktail;
    }

    public bool isDescOpen()
    {
        return currentSelectedCocktail != null || currentSelectedSection != null;
    }
    
    public void setDescIsNotOpen()
    {
        currentSelectedCocktail = null;
        currentSelectedSection = null;
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
    
    public void OpenSendNewCubataFeedbakc()
    {
        GameManager.instance.generalUi.OpenFeedbackMenuCubata();
    }

}
