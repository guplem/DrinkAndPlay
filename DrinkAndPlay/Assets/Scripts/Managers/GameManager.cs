using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public LocalizationManager localizationManager { get; private set; }
    public SavesManager savesManager { get; private set; }
    public ConfigurationManager configurationManager { get; private set; }


    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Two game managers have been created. Destroying the game object with the newest 'GameManager' before initialization");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    void Start()
    {
        savesManager = new SavesManager();
        configurationManager = new ConfigurationManager(savesManager);
        localizationManager = new LocalizationManager(configurationManager);

        localizationManager.LoadCurrentLanguage("UI");
    }
}
