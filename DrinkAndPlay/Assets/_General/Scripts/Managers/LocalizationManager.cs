using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LocalizationManager
{
    private readonly DataManager dataManager;
    private Dictionary<LocalizationFile, List<LocalizedText>> localizedTexts;
    public int resetPercentage { get; private set; }

    public LocalizationManager(DataManager dataManager, int resetPercentage)
    {
        Debug.Log("Creating localization Manager.");
        this.dataManager = dataManager;
        localizedTexts = new Dictionary<LocalizationFile, List<LocalizedText>>();
        this.resetPercentage = resetPercentage;
    }

    public bool ReloadForCurrentLanguage()
    {
        List<LocalizationFile> localizedFiles = new List<LocalizationFile>(localizedTexts.Keys);
        foreach (LocalizationFile localizedFile in localizedFiles)
        {
            localizedTexts[localizedFile] = new List<LocalizedText>();
            LoadCurrentLanguageFor(localizedFile);
        }

        LocalizeAllLocalizableObjects();
        
        return true;
    }

    public bool LoadCurrentLanguageFor(LocalizationFile localizationFile)
    {
        return LoadLanguageFor(localizationFile, dataManager.language);
    }
    
    public bool LoadLanguageFor(LocalizationFile localizationFile, string language)
    {
        if (localizationFile == null)
        {
            Debug.LogError("Trying to load a null localizationFile");
            return false;
        }
        
        if (string.IsNullOrEmpty(language))
        {
            Debug.LogError("The language to be loaded for " + localizationFile + " is null or empty.");
            return false;
        }
        
        Debug.Log("Loading '" + localizationFile + "' localization file for language '" + language + "'.");
        
        string[][] dataRead = CSVReader.Read(localizationFile).ToArray();
        int idCol = -1;
        int naughtyCol = -1;
        int langCol = -1;

        for (int row = 0; row < dataRead.Length; row++)
        {
            //Check where is everything in the file
            if (row == 0)
            {
                for (int col = 0; col < dataRead[row].Length; col++)
                {
                    
                    if (string.Compare(dataRead[row][col].ToUpper(), "ID", StringComparison.Ordinal) == 0)
                        idCol = col;
                    if (string.Compare(dataRead[row][col].ToUpper(), "NAUGHTINESS", StringComparison.Ordinal) == 0)
                        naughtyCol = col;
                    if (string.Compare(dataRead[row][col].ToUpper(), language.ToUpper(), StringComparison.Ordinal) == 0)
                        langCol = col;

                    if (idCol > -1 && naughtyCol > -1 && langCol > -1)
                        break;
                }

                if (idCol < 0)
                    Debug.LogError("The localizationFile '" + localizationFile + "' is missing the 'ID' column in its localization file");
                else if (langCol < 0)
                {
                    Debug.LogError("The localizationFile '" + localizationFile + "' is missing the column '" + language + "' in its localization file.     ('"  + language + "' is the current language.)");
                }
            }


            //Save the localized text with the proper language
            else
            {
                int.TryParse(dataRead[row][1], out int naughtiness);
                LocalizedText localizedText = new LocalizedText(dataRead[row][0], naughtiness, dataRead[row][langCol]);
                if (!string.IsNullOrEmpty(localizedText.text)) //Only if the text is valid
                    AddLocalizedTextToTextsList(localizationFile, localizedText);
            }

        }

        RandomizeLocalizedTexts(localizationFile);
        Debug.Log("Loaded " + localizedTexts[localizationFile].Count + " texts from the localization file '" + localizationFile + "'.");
        
        /*#if UNITY_EDITOR
            CheckValidityOf(localizedTexts[localizationFile], resetPercentage, localizationFile, language);
        #endif*/
        
        return true;
    }

    public List<LocalizedText> GetLocalizedTextsFrom(LocalizationFile localizationFile)
    {
        return localizedTexts[localizationFile];
    }
    
    public void CheckValidityOf(List<LocalizedText> localizedTexts, int percentage, LocalizationFile localizationFile, string lang)
    {
        if (localizedTexts == null || localizedTexts.Count <= 0)
            Debug.LogError("Checking validity of a null or empty localizedTexts.");
        if (localizationFile == null)
            Debug.LogError("Checking validity of a null localizationFile.");
        
        
        for (int nl = dataManager.naughtyLevelExtremes.min; nl <= dataManager.naughtyLevelExtremes.max; nl++)
        {
            if (nl <= 0)
            {
                Debug.LogWarning("The minimum naughty level shouldn't be below '1'.");
                nl=1;
            }
            
            int counter = 0;
            
            foreach (LocalizedText localizedText in localizedTexts)
                if (localizedText.naughtiness == nl)
                    counter++;

            int minQttOfSentences = 100 / percentage;
            if (counter < minQttOfSentences)
                Debug.LogWarning("Only found " + counter + " texts while searching sentences of naughty level = " + nl + " in the localization file '" + localizationFile + "' with language = '" + lang + "'.\nAt least " + minQttOfSentences + " texts are needed.");
        }

    }

    public void RandomizeLocalizedTexts(LocalizationFile localizationFile)
    {
        List<LocalizedText> duplicatedLocalizedTexts = new List<LocalizedText>(localizedTexts[localizationFile]);
        List<LocalizedText> randomList = new List<LocalizedText>();
        System.Random r = new System.Random();
        while (duplicatedLocalizedTexts.Count > 0)
        {
            int randomIndex = r.Next(0, duplicatedLocalizedTexts.Count);
            randomList.Add(duplicatedLocalizedTexts[randomIndex]); //add it to the new, random list
            duplicatedLocalizedTexts.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        localizedTexts[localizationFile] = randomList;
    }

    private void AddLocalizedTextToTextsList(LocalizationFile localizationFile, LocalizedText localizedText)
    {
        if (localizationFile == null)
        {
            Debug.LogError("Trying to save a localized text of a 'null' localizationFile.");
            return;
        }

        if (localizationFile == null)
        {
            Debug.LogError("Trying to save 'null' localizedText.");
            return;
        }

        //Debug.Log("Saving " + localizedText + " to " + localizationFile.name);

        if (!localizedTexts.ContainsKey(localizationFile))
            localizedTexts.Add(localizationFile, new List<LocalizedText>());

        localizedTexts[localizationFile].Add(localizedText);
    }

    public delegate void LocalizeAllAction();
    public static event LocalizeAllAction OnLocalizeAllAction;

    private void LocalizeAllLocalizableObjects()
    {
        if (OnLocalizeAllAction == null) return;
        
        Debug.Log("   > " + "Localizing all objects");
        OnLocalizeAllAction();
    }
    
    
    public LocalizedText SearchLocalizedText(LocalizationFile localizationFile, string id, bool register)
    {
        if (localizationFile == null)
        {
            Debug.LogError("Trying to get a localized text by id from a null localization file.");
            return null;
        }

        CheckAndLoad(localizationFile);

        foreach (LocalizedText localizedText in localizedTexts[localizationFile])
        {
            if (localizedText.id == id)
            {
                if (register)
                    GameManager.instance.dataManager.AddTextRegistered(localizationFile, id);

                return localizedText;
            }
        }

        return new LocalizedText(id, -1, "The text with id '" + id + "' could not be found in the localizationFile '" + localizationFile + "'");
    }

    private void CheckAndLoad(LocalizationFile localizationFile)
    {
        if (!GameManager.instance.localizationManager.IsSectionLocalized(localizationFile))
                    GameManager.instance.localizationManager.LoadCurrentLanguageFor(localizationFile);
    }


    public LocalizedText SearchLocalizedText(LocalizationFile localizationFile, bool useProperNaughtyLevel, bool register, bool checkNotRegistered, bool trySecondSearchAfterResetRegister)
    {
        if (localizationFile == null)
        {
            Debug.LogError("Trying to get a localized text from a null localization file.");
            return null;
        }
        
        CheckAndLoad(localizationFile);
        
        // Search for it
        foreach (LocalizedText localizedText in localizedTexts[localizationFile])
        {
            if (useProperNaughtyLevel && dataManager.IsValueOfNaughtyLevelCorrect(localizedText.naughtiness) || !useProperNaughtyLevel)
            {
                if (checkNotRegistered && !GameManager.instance.dataManager.IsTextRegistered(localizationFile, localizedText.id) || !checkNotRegistered)
                {
                    if (register)
                        GameManager.instance.dataManager.AddTextRegistered(localizationFile, localizedText.id);

                    return localizedText;
                }
            }
            //else Debug.LogWarning("NL of sentence = " + localizedText.naughtiness + ". Expected " + dataManager.naughtyLevel );
        }
        
        
        //If it is not found probably is because it is because it is looking for a not registered text
        if (checkNotRegistered && trySecondSearchAfterResetRegister)
        {
            Debug.Log("Going to reset registered sentences looking for a text from " + localizationFile + ". Using ProperNaughtyLevel? " + useProperNaughtyLevel + " NL = " + dataManager.naughtyLevel);
            if (ResetRegisteredSentences(localizationFile, useProperNaughtyLevel, resetPercentage))
            {
                Debug.Log("Going to do the second search.");
                
                return SearchLocalizedText(localizationFile, useProperNaughtyLevel, register, checkNotRegistered, false);
            }
                
        }
        
        Debug.LogError("Localized text not found in the file '" + localizationFile + "'. Search filter: Register="+register + ", checkNotRegistered="+checkNotRegistered + ". Using ProperNaughtyLevel? " + useProperNaughtyLevel + ". NL = " + dataManager.naughtyLevel);
        Debug.Break();
        return null;
    }

    private bool ResetRegisteredSentences(LocalizationFile localizationFile, bool useProperNaughtyLevel, int percentage)
    {
        int minQttToApplyUnregistering = 100 / percentage;
        if (GameManager.instance.dataManager.GetTextRegisteredQuantity(localizationFile, useProperNaughtyLevel) >= minQttToApplyUnregistering) // To know if there are enough to remove the register of the 50%
        {
            GameManager.instance.dataManager.RemoveOldestPercentageOfTextsRegistered(localizationFile, percentage, useProperNaughtyLevel);
            Debug.Log("Reset the top 25% of the registered sentences in the localization file '" + localizationFile + "'");
            GameManager.instance.localizationManager.RandomizeLocalizedTexts(localizationFile); // To change the order of the new avaliable texts
            return true;
        }
        else
        {
            Debug.LogError("There are not enough registered texts from the localization file '" + localizationFile + "'. Found only " + GameManager.instance.dataManager.GetTextRegisteredQuantity(localizationFile, useProperNaughtyLevel) + " and needed at least " + minQttToApplyUnregistering + ".\nUsing ProperNaughtyLevel? " + useProperNaughtyLevel + " NL = " + dataManager.naughtyLevel);
            return false;
        }
    }

    public bool IsSectionLocalized(LocalizationFile localizationFile)
    {
        return localizedTexts.ContainsKey(localizationFile);
    }
    
    public LocalizationFile GetRandomLocalizationFile(List<LocalizationFile> lastSelectedLocalizationFiles, bool uniformSentenceProbabilityDistribution)
        //uniformSentenceProbabilityDistribution --> All localizations will finish at the same time (to avoid repetition)
    {
        if (lastSelectedLocalizationFiles.Count <= 0) return null;
        
        if (!uniformSentenceProbabilityDistribution)
            return lastSelectedLocalizationFiles[Random.Range(0, lastSelectedLocalizationFiles.Count)];

        int totalSentences = 0;
        foreach (LocalizationFile lf in lastSelectedLocalizationFiles)
            totalSentences += lf.quantityOfSentences;
        int currentValue = 0;
        int randomValue = Random.Range(0, totalSentences);
        foreach (LocalizationFile lf in lastSelectedLocalizationFiles)
        {
            currentValue += lf.quantityOfSentences;
            if (currentValue >= randomValue)
                return lf;
        }
        
        return null;
    }
}