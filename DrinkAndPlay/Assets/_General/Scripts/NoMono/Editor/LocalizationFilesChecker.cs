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

        LocalizationFile[] localizationFiles = Downloader.GetAllLocalizationFiles();
        Language[] languages = GetAllLanguages();

        foreach (Language language in languages)
        {
            Debug.Log("Starting check of language '" + language.id +"'.");
            if (!language.isEnabled)
            {
                Debug.Log("Disabled language, skipping.");
                continue;
            }

            int sentencesByLanguage = 0;
            
            foreach (LocalizationFile localizationFile in localizationFiles)
            {
                Debug.Log("Starting check of localizationFile '" + localizationFile+"'.");
                if (!localizationFile.checkForEnoughSentencesOfAllNaughtyLevels)
                {
                    Debug.Log("Disabled localization file's check of texts' naughty level, skipping.");
                    continue;
                }
                
                // Load
                localizationManager.LoadLanguageFor(localizationFile, language);
                // Check
                List<LocalizedText> lt = localizationManager.GetLocalizedTextsFrom(localizationFile);
                localizationManager.CheckValidityOf(lt, localizationManager.resetPercentage, localizationFile, language.id);
                sentencesByLanguage += lt.Count;
            }
            
            Debug.Log("Checked " + sentencesByLanguage + " in-game texts for the language " + language);
        }

        Debug.Log("Localization files checking finished.");
    }

    public static Language[] GetAllLanguages()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(Language).Name);
        Language[] languages = new Language[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            languages[i] = AssetDatabase.LoadAssetAtPath<Language>(path);
        }

        return languages;
    }
    


}
