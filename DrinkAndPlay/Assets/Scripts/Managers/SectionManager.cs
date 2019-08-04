using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public abstract class SectionManager : MonoBehaviour
{
    public GameManager gm { get; private set; }
    [SerializeField] private GameObject GameManagerPrefab;
    public static SectionManager Instance { get; private set; }
    [SerializeField] public Section section;


    private void Awake()
    {
        GameManagerManagement();

        Instance = this;

        if (section == null)
            Debug.LogError("The 'section' is null in the object' " + name, gameObject);

        gm.localizationManager.LoadCurrentLanguageFor(section);
    }

    private void GameManagerManagement()
    {
        if (GameManagerPrefab == null)
            Debug.LogError("'GameManagerPrefab' is null in the object " + name, gameObject);
        Instantiate(GameManagerPrefab).GetComponent<GameManager>().Initialize();
        gm = GameManager.Instance;
    }


}