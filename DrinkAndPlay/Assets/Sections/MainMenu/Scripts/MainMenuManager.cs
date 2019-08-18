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
    [SerializeField] private GameObject sectionDescription;

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

    public void OpenSectionDescription(Section section, GameObject originalImage, Vector2 imageSize)
    {
        Debug.Log("Opening description of " + section);
        
        RectTransform rect = sectionDescription.GetComponent<RectTransform>();
        //rect.anchorMin = new Vector2(0.5f, 0.5f);
        //rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.position = originalImage.GetComponent<RectTransform>().position;
        //rect.sizeDelta = imageSize;
        //Vector2 sizeRect = new Vector2(rect.rect.width, rect.rect.height);
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(1f, 1f);
        //rect.sizeDelta = sizeRect;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imageSize.x);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imageSize.y);
//        float rrw = rect.rect.width;
 //       rect.anchorMin = new Vector2(0f, rect.anchorMin.y);
  //      rect.anchorMax = new Vector2(0f, rect.anchorMax.y);
   //     rect.sizeDelta = new Vector2(rrw, rect.sizeDelta.y);
    }

}
