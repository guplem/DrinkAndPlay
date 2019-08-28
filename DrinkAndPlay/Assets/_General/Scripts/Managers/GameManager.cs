using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public LocalizationManager localizationManager { get; private set; }
    public DataManager dataManager { get; private set; }

    [SerializeField] public Section uiSection;
    [SerializeField] public GeneralUI generalUi;
    [SerializeField] public Section landingSection;

    public static GameManager instance { get; private set; }
    public void Initialize()
    {
        Debug.Log("Initializing GameManager");
        
        if (instance != null)
        {
            Debug.Log("Two game managers have been created (expected if comming from other section). Destroying the game object with the newest 'GameManager' before initialization");
            Destroy(gameObject);
            return; // If the return is not not here, the method is fully-run and after that, the object is destroyed. This is not the desired behaviour.
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // SETUP
        
        dataManager = new DataManager(false);
        localizationManager = new LocalizationManager(dataManager);

        if (uiSection == null)
            Debug.LogError("UI Section not set up in the GameManager.");
        localizationManager.LoadCurrentLanguageFor(uiSection);
    }
    

    public static void LoadSection(Section section)
    {
        Debug.Log(" >>>> Loading scene '" + section.sceneName + "' from section '" + section + "' <<<< ");
        SceneManager.LoadScene(section.sceneName, LoadSceneMode.Single);
    }

}
