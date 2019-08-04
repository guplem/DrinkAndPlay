using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public LocalizationManager localizationManager { get; private set; }
    public SavesManager savesManager { get; private set; }
    public ConfigurationManager configurationManager { get; private set; }
    public Section uiSection;


    public static GameManager Instance { get; private set; }
    public void Initialize()
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
        Setup();
    }


    private void Setup()
    {
        savesManager = new SavesManager();
        configurationManager = new ConfigurationManager(savesManager);
        localizationManager = new LocalizationManager(configurationManager);

        if (uiSection == null)
            Debug.LogError("UI Section not set up in the GameManager");
        localizationManager.LoadCurrentLanguage(uiSection);

        Debug.Log("Game Manager setup completed");
    }
}
