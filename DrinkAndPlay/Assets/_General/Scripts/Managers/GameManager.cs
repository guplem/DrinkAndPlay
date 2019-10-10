using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public bool saveUserAsBetaTester;

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

        if (uiLocalizationFile == null)
            Debug.LogError("UI Section not set up in the GameManager.");
        localizationManager.LoadCurrentLanguageFor(uiLocalizationFile);

        dataManager.betaTester = saveUserAsBetaTester;
        
        #if UNITY_EDITOR
        #elif UNITY_ANDROID
            Screen.fullScreen = false;
        #endif
    }
    

    public bool PlaySection(Section section)
    {
        if (SceneManager.GetActiveScene().name.CompareTo(section.sceneName) == 0)
        {
            Debug.Log(" Not loading scene '" + section.sceneName + "' because it is the active one.");
            return false;
        }

        
        if (!HaveEnoughPlayersFor(section))
        {
            Debug.Log(" Not loading scene '" + section.sceneName + "' because there are not enough players to play it.");
            generalUi.OpenPlayersMenu(section.minNumberOfPlayers, section);
            return false;
        }

        Debug.Log(" >>>> Loading scene '" + section.sceneName + "' from section '" + section + "' <<<< ");
        //SceneManager.LoadSceneAsync(section.sceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(section.sceneName, LoadSceneMode.Single);

        return true;
    }

    public bool HaveEnoughPlayersFor(Section section)
    {
        if (section != null)
            return ! (section.minNumberOfPlayers > 0 && section.minNumberOfPlayers > dataManager.GetPlayersQuantity());

        Debug.LogWarning("Checking if there are enough players to play a NULL section.");
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
