using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

    public LocalizationManager localizationManager { get; private set; }
    public DataManager dataManager { get; private set; }

    [FormerlySerializedAs("uiSection")] [SerializeField] public LocalizationFile uiLocalizationFile;
    [SerializeField] public GeneralUI generalUi;
    [SerializeField] public Section landingSection;
    [SerializeField] public Language[] languages;
    [SerializeField] public Language defaultLanguage;

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
        
        if (!defaultLanguage.isEnabled)
            Debug.LogError("The default language (" + defaultLanguage + ") is not enabled and it should be.");
        
        localizationManager = new LocalizationManager(dataManager, 25);

        if (uiLocalizationFile == null)
            Debug.LogError("UI Section not set up in the GameManager.");
        //localizationManager.LoadCurrentLanguageFor(uiLocalizationFile);

        if (IsNewSession())
        {
            dataManager.forcePlayersDisplay = true;
            if (dataManager.automaticNaughtyLevel)
                dataManager.SetNaughtyLevel(DataManager.defaultNaughtyLevel);
        }
        else
        {
            dataManager.disclaimerPopupShown = true;
        }
        dataManager.RegisterSession();
        
        /*
        #if UNITY_EDITOR
        #else
            Screen.fullScreen = false;
        #endif
        */
    }
    
    private bool IsNewSession()
    {
        return Mathf.Abs((float)DateTime.Now.Subtract(dataManager.lastSessionDateTime).TotalHours) > 10; // If the time difference is greater than 10h it is considered a new session
    }
    

    public bool PlaySection(Section section)
    {

        if (section == null)
        {
            Debug.LogError("Tying to load a null section.");
            return false;
        }
        
        if (string.Compare(SceneManager.GetActiveScene().name, section.sceneName, StringComparison.OrdinalIgnoreCase) == 0)
        {
            Debug.Log(" Not loading scene '" + section.sceneName + "' because it is the active one.");
            return false;
        }
        
        if ((section.hasMinimumNumberOfPlayers && dataManager.forcePlayersDisplay) || !dataManager.HaveEnoughEnabledPlayersFor(section))
        {
            Debug.Log(" Not loading scene '" + section.sceneName + "' because there are not enough players to play it or the display of the players section has been forced.");
            generalUi.OpenPlayersMenu(section);
            return false;
        }



        if (section.showLocalizationFilesSelectorBeforeLoading && !generalUi.localizationFilesSelectorMenu.isShowing)
        {
            generalUi.OpenLocalizationFilesSelectorMenu(section);
            return false;
        }
        
        Debug.Log(" >>>> Loading scene '" + section.sceneName + "' from section '" + section + "' <<<< ");
        //SceneManager.LoadSceneAsync(section.sceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(section.sceneName, LoadSceneMode.Single);
        
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