using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

    public LocalizationManager localizationManager { get; private set; }
    public DataManager dataManager { get; private set; }

    [FormerlySerializedAs("uiSection")] [SerializeField] public LocalizationFile uiLocalizationFile;
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
        
        dataManager = new DataManager(false, false);
        
        localizationManager = new LocalizationManager(dataManager, 25);

        if (uiLocalizationFile == null)
            Debug.LogError("UI Section not set up in the GameManager.");
        //localizationManager.LoadCurrentLanguageFor(uiLocalizationFile);

        
        /*
        #if UNITY_EDITOR
        #else
            Screen.fullScreen = false;
        #endif
        */
    }
    

    public bool PlaySection(Section section)
    {

        if (section == null)
        {
            Debug.LogError("Tying to load a null section.");
            return false;
        }
        
        if (SceneManager.GetActiveScene().name.CompareTo(section.sceneName) == 0)
        {
            Debug.Log(" Not loading scene '" + section.sceneName + "' because it is the active one.");
            return false;
        }
        
        if (!dataManager.HaveEnoughPlayersFor(section))
        {
            Debug.Log(" Not loading scene '" + section.sceneName + "' because there are not enough players to play it.");
            generalUi.OpenPlayersMenu(section.minNumberOfPlayers, section);
            return false;
        }

        Debug.Log(" >>>> Loading scene '" + section.sceneName + "' from section '" + section + "' <<<< ");
        //SceneManager.LoadSceneAsync(section.sceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(section.sceneName, LoadSceneMode.Single);

        if (section.forceShowNaughtyLevelConfigurator)
            generalUi.OpenNaughtyLevelMenu();
    
        if (section.forceShowPlayersConfigurator)
            generalUi.OpenPlayersMenu();
    
        if (section.forceShowLanguageConfigurator)
            generalUi.OpenLanguageMenu();
        
        if (section.forceShowSectionSelector)
            generalUi.OpenSectionSelectorMenu();
        
        
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            instance.generalUi.CloseLastOpenUiElement();
        }
    }


    
}