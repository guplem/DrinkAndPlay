using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LocalizationManager
{
    private readonly DataManager configurationManager;
    private Dictionary<LocalizationFile, List<LocalizedText>> localizedTexts;

    public LocalizationManager(DataManager dataManager)
    {
        Debug.Log("Creating localization Manager");
        this.configurationManager = dataManager;
        localizedTexts = new Dictionary<LocalizationFile, List<LocalizedText>>();
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
        Debug.Log("Loading '" + localizationFile + "' localization file");
        
        if (localizationFile == null)
        {
            Debug.LogError("Trying to load a 'null' localizationFile's localized texts");
            return false;
        }

        string lang = configurationManager.language;

        if (string.IsNullOrEmpty(lang))
        {
            Debug.LogError("The language to be loaded is null or empty");
            return false;
        }

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
                    if (string.Compare(dataRead[row][col].ToUpper(), lang.ToUpper(), StringComparison.Ordinal) == 0)
                        langCol = col;

                    if (idCol > -1 && naughtyCol > -1 && langCol > -1)
                        break;
                }

                if (idCol < 0)
                    Debug.LogError("The localizationFile '" + localizationFile + "' is missing the 'ID' column in its localization file");
                else if (langCol < 0)
                {
                    Debug.LogError("The localizationFile '" + localizationFile + "' is missing the column '" + lang + "' in its localization file.     ('"  + lang + "' is the current language.)");
                    Debug.LogError("The ID COLUMN IS: " + idCol);
                    
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
        
        return true;
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
    
    
    public LocalizedText GetLocalizedText(LocalizationFile localizationFile, string id, bool register)
    {
        if (localizationFile == null)
        {
            Debug.LogError("Trying to get a localized text from a null localization file.");
            return null;
        }

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

    
    public LocalizedText GetLocalizedText(LocalizationFile localizationFile, int naughtyLevel, bool register, bool checkNotRegistered)
    {
        // Search for it
        foreach (LocalizedText localizedText in localizedTexts[localizationFile])
        {
            if (localizedText.naughtiness == naughtyLevel )
            {
                if (checkNotRegistered && !GameManager.instance.dataManager.IsTextRegistered(localizationFile, localizedText.id) || !checkNotRegistered)
                {
                    if (register)
                            GameManager.instance.dataManager.AddTextRegistered(localizationFile, localizedText.id);

                    return localizedText;
                }
            }
        }

        return null;
    }
    
    
    public LocalizedText GetLocalizedText(LocalizationFile localizationFile, bool register, bool checkNotRegistered)
    {
        // Search for it
        foreach (LocalizedText localizedText in localizedTexts[localizationFile])
        {
            if (checkNotRegistered && !GameManager.instance.dataManager.IsTextRegistered(localizationFile, localizedText.id) || !checkNotRegistered)
            {
                if (register)
                    GameManager.instance.dataManager.AddTextRegistered(localizationFile, localizedText.id);

                return localizedText;
            }
        }

        return null;
    }

    public bool IsSectionLocalized(LocalizationFile localizationFile)
    {
        return localizedTexts.ContainsKey(localizationFile);
    }


}
