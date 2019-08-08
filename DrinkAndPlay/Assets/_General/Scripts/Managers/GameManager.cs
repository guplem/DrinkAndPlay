using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public LocalizationManager localizationManager { get; private set; }
    public SavesManager savesManager { get; private set; }
    public ConfigurationManager configurationManager { get; private set; }

    [SerializeField] public Section uiSection;
    [SerializeField] public GeneralUI generalUI;

    public static GameManager Instance { get; private set; }
    public void Initialize()
    {
        if (Instance != null)
        {
            Debug.Log("Two game managers have been created (expected if comming from other section). Destroying the game object with the newest 'GameManager' before initialization");
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
            Debug.LogError("UI Section not set up in the GameManager.");
        localizationManager.LoadCurrentLanguageFor(uiSection);

        Debug.Log("Game Manager setup completed.");
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading scene '" + sceneName + "'");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
