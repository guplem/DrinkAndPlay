using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SectionManager : MonoBehaviour
{
    protected GameManager gm { get; private set; }
    [SerializeField] private GameObject gameManagerPrefab;
    public static SectionManager instance { get; private set; }
    [SerializeField] public Section section;


    private void Awake()
    {
        Debug.Log(" ========= Starting section setup - SectionManager (Awake) ========= ");

        SetupGameManager();

        instance = this;

        if (section == null)
            Debug.LogError("The 'section' is null in the object' " + name, gameObject);

        gm.localizationManager.LoadCurrentLanguageFor(section);

        gm.generalUi.SetupFor(section);

        SetupSectionUi();
        
        Debug.Log(" ========= Finished section setup - SectionManager (Awake) ========= ");
    }


    private void SetupGameManager()
    {
        if (gameManagerPrefab == null)
            Debug.LogError("'GameManagerPrefab' is null in the object " + name, gameObject);

        Instantiate(gameManagerPrefab).GetComponent<GameManager>().Initialize();

        gm = GameManager.instance;
    }
    private void SetupSectionUi()
    {
        Debug.Log("Setting up section UI");
        RectTransform rt = GetComponent<RectTransform>();

        if (section.topBar)
            rt.anchorMax = new Vector2(1, gm.generalUi.topBar.GetComponent<RectTransform>().anchorMin.y);

        if (section.bottomBar)
            rt.anchorMin = new Vector2(0, gm.generalUi.bottomBar.GetComponent<RectTransform>().anchorMax.y);
    }

}