using System.Collections;
using System.Collections.Generic;
using System.IO;
using BayatGames.SaveGameFree;
using UnityEditor;
using UnityEngine;

public class LocalizationFilesChecker : MonoBehaviour
{
    [MenuItem("Drink and Play/Localization files/Check for 25% reset")]
    public static void CheckLocalizationFiles()
    {
        LocalizationManager localizationManager = new LocalizationManager(new DataManager(false, true), 25);

        LocalizationFile[] localizationFiles = GameManager.GetAllLocalizationFiles();
        Language[] languages = GameManager.GetAllLanguages();

        foreach (Language language in languages)
        {
            Debug.Log("Starting check of language '" + language.id +"'.");
            if (!language.isEnabled)
            {
                Debug.Log("Disabled language, skipping.");
                continue;
            }
                

            foreach (LocalizationFile localizationFile in localizationFiles)
            {
                Debug.Log("Starting check of localizationFile '" + localizationFile+"'.");
                if (!localizationFile.checkForEnoughSentencesOfAllNaughtyLevels)
                {
                    Debug.Log("Disabled localization file's check of texts' naughty level, skipping.");
                    continue;
                }
                
                // Load
                localizationManager.LoadLanguageFor(localizationFile, language.id);
                // Check
                localizationManager.CheckValidityOf(localizationManager.GetLocalizedTextsFrom(localizationFile), localizationManager.resetPercentage, localizationFile, language.id);
            }
            
        }

        Debug.Log("Localization files checked.");
    }


}
