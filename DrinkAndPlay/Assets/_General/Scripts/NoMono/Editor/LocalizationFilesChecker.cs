using System.Collections;
using System.Collections.Generic;
using System.IO;
using BayatGames.SaveGameFree;
using UnityEditor;
using UnityEngine;


public class LocalizationFilesChecker : MonoBehaviour
{
    [MenuItem("Drink and Play/Localization files/Check localization files health")]
    public static bool CheckLocalizationFiles()
    {
        LocalizationManager localizationManager = new LocalizationManager(new DataManager(false), 25);

        LocalizationFile[] localizationFiles = Downloader.GetAllLocalizationFiles();
        List<Language> languages = GetAllEnabledLanguages();

        foreach (Language language in languages)
        {
            Debug.Log("Starting check of language '" + language +"'.");

            int sentencesByLanguage = 0;
            
            foreach (LocalizationFile localizationFile in localizationFiles)
            {
                Debug.Log("Starting check of localizationFile '" + localizationFile+"'.");
                
                // Load and check health of document
                if (!localizationManager.LoadLanguageFor(localizationFile, language))
                    return false;
                
                if (!localizationFile.checkForEnoughSentencesOfAllNaughtyLevels)
                {
                    Debug.Log("Disabled localization file's check of texts' naughty level, skipping.");
                    continue;
                }
                
                // Check 25% reset
                List<LocalizedText> lt = localizationManager.GetLocalizedTextsFrom(localizationFile);
                localizationManager.CheckValidityOf(lt, localizationManager.resetPercentage, localizationFile, language);
                sentencesByLanguage += lt.Count;
            }

            SaveGame.Save("LocalizedTexts for " + language + " - Check Result" + ".json", localizationManager.localizedTexts);
            
            Debug.Log("Checked " + sentencesByLanguage + " in-game texts for the language " + language);
        }

        Debug.Log("Localization files checking finished successfully.");
        return true;
    }

    public static List<Language> GetAllEnabledLanguages()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(Language).Name);
        List<Language> languages = new List<Language>();
        foreach (string assetLang in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetLang);
            Language lang = AssetDatabase.LoadAssetAtPath<Language>(path);
            if (lang.isEnabled)
                languages.Add(lang);
        }

        return languages;
    }
    


}
