using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LocalizationManager
{
    private readonly DataManager configurationManager;
    private Dictionary<Section, List<LocalizedText>> localizedTexts;

    public LocalizationManager(DataManager dataManager)
    {
        Debug.Log("Creating localization Manager");
        this.configurationManager = dataManager;
        localizedTexts = new Dictionary<Section, List<LocalizedText>>();
    }

    public bool ReloadForCurrentLanguage()
    {
        List<Section> localizedSections = new List<Section>(localizedTexts.Keys);
        foreach (Section localizedSection in localizedSections)
        {
            localizedTexts[localizedSection] = new List<LocalizedText>();
            LoadCurrentLanguageFor(localizedSection);
        }

        LocalizeAllLocalizableObjects();
        
        return true;
    }

    public bool LoadCurrentLanguageFor(Section section)
    {
        Debug.Log("Loading language for '" + section + "'");
        
        if (section == null)
        {
            Debug.LogError("Trying to load a 'null' section's localized texts");
            return false;
        }

        string lang = configurationManager.language;

        if (string.IsNullOrEmpty(lang))
        {
            Debug.LogError("The language to be loaded is null or empty");
            return false;
        }

        string[][] dataRead = CSVReader.Read(section).ToArray();
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

                    if (string.Compare(dataRead[row][col].ToUpper(), "ID", StringComparison.Ordinal) != 0)
                        idCol = col;
                    if (string.Compare(dataRead[row][col].ToUpper(), "NAUGHTINESS", StringComparison.Ordinal) != 0)
                        naughtyCol = col;
                    if (string.Compare(dataRead[row][col].ToUpper(), lang.ToUpper(), StringComparison.Ordinal) == 0)
                        langCol = col;

                    if (idCol > -1 && naughtyCol > -1 && langCol > -1)
                        break;
                }

                if (idCol < 0)
                    Debug.LogError("The section '" + section + "' is missing the 'ID' column in its localization file");
                else if (langCol < 0)
                    Debug.LogError("The section '" + section + "' is missing the column '" + lang + "' in its localization file.     ('"  + lang + "' is the current language.)");
            }


            //Save the localized text with the proper language
            else
            {
                int.TryParse(dataRead[row][1], out int naughtiness);
                LocalizedText localizedText = new LocalizedText(dataRead[row][0], naughtiness, dataRead[row][langCol]);
                AddLocalizedTextToTextsList(section, localizedText);
            }

        }
        
        return true;
    }

    private void AddLocalizedTextToTextsList(Section section, LocalizedText localizedText)
    {
        if (section == null)
        {
            Debug.LogError("Trying to save a localized text of a 'null' section.");
            return;
        }

        if (section == null)
        {
            Debug.LogError("Trying to save 'null' localizedText.");
            return;
        }

        //Debug.Log("Saving " + localizedText + " to " + section.name);

        if (!localizedTexts.ContainsKey(section))
            localizedTexts.Add(section, new List<LocalizedText>());

        localizedTexts[section].Add(localizedText);
    }

    public delegate void LocalizeAllAction();
    public static event LocalizeAllAction OnLocalizeAllAction;

    private void LocalizeAllLocalizableObjects()
    {
        if (OnLocalizeAllAction == null) return;
        
        Debug.Log("   > " + "Localizing all objects");
        OnLocalizeAllAction();
    }
    
   
    public LocalizedText GetLocalizedText(Section section, string id, bool register)
    {
        
        foreach (LocalizedText localizedText in localizedTexts[section])
        {
            if (localizedText.id == id)
            {
                if (register)
                    GameManager.instance.dataManager.AddTextRegistered(section, id);

                return localizedText;
            }
        }

        return new LocalizedText(id, -1, "The text with id '" + id + "' could not be found in the section '" + section + "'");
    }

    public LocalizedText GetLocalizedText(Section section, int naughtyLevel, bool register, bool checkNotRegistered)
    {
        // Randomize the list
        /*List<LocalizedText> duplicatedLocalizedTexts = new List<LocalizedText>(localizedTexts[section]);
        List<LocalizedText> randomList = new List<LocalizedText>();
        System.Random r = new System.Random();
        while (duplicatedLocalizedTexts.Count > 0)
        {
            int randomIndex = r.Next(0, duplicatedLocalizedTexts.Count);
            randomList.Add(duplicatedLocalizedTexts[randomIndex]); //add it to the new, random list
            duplicatedLocalizedTexts.RemoveAt(randomIndex); //remove to avoid duplicates
        }*/

        // Search for it
        foreach (var localizedText in localizedTexts[section])
        {
            if (localizedText.naughtiness == naughtyLevel )
            {
                if (checkNotRegistered && !GameManager.instance.dataManager.IsTextRegistered(section, localizedText.id) || !checkNotRegistered)
                {
                    if (register)
                            GameManager.instance.dataManager.AddTextRegistered(section, localizedText.id);

                    return localizedText;
                }
            }
        }
        
        /*
        string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";
        string randomID = "";
        int charAmount = Random.Range(5, 15);
        for(int i=0; i<charAmount; i++) randomID += glyphs[Random.Range(0, glyphs.Length)];
        return new LocalizedText(randomID, -1, "Any text with naughty level '" + naughtyLevel + "' could not be found in the section '" + section + "'");
        */

        return null;
    }


}
