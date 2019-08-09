using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SectionManager : MonoBehaviour
{
    public GameManager gm { get; private set; }
    [SerializeField] private GameObject GameManagerPrefab;
    public static SectionManager Instance { get; private set; }
    [SerializeField] public Section section;


    private void Awake()
    {
        Debug.Log("Starting preparation process");

        GameManagerManagement();

        Instance = this;

        if (section == null)
            Debug.LogError("The 'section' is null in the object' " + name, gameObject);

        gm.localizationManager.LoadCurrentLanguageFor(section);

        gm.generalUI.SetupFor(section);

        SetupSectionUI();
    }


    private void GameManagerManagement()
    {
        if (GameManagerPrefab == null)
            Debug.LogError("'GameManagerPrefab' is null in the object " + name, gameObject);

        Instantiate(GameManagerPrefab).GetComponent<GameManager>().Initialize();

        gm = GameManager.Instance;
    }
    private void SetupSectionUI()
    {
        RectTransform rt = GetComponent<RectTransform>();

        if (section.topBar)
            rt.anchorMax = new Vector2(1, gm.generalUI.topBar.GetComponent<RectTransform>().anchorMin.y);

        if (section.bottomBar)
            rt.anchorMin = new Vector2(0, gm.generalUI.bottomBar.GetComponent<RectTransform>().anchorMax.y);
    }

}