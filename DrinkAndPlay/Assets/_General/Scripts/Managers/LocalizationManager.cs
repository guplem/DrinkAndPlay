using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
    private readonly DataManager configurationManager;
    private Dictionary<Section, List<LocalizedText>> localizedTexts;

    public LocalizationManager(DataManager dataManager)
    {
        this.configurationManager = dataManager;
        localizedTexts = new Dictionary<Section, List<LocalizedText>>();
    }

    public bool ReloadCurrentLanguage()
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
        if (section == null)
        {
            Debug.LogError("Trying to load a 'null' section's localized texts");
            return false;
        }

        Debug.Log("Loading language for " + section);

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
                SaveLocalizedText(section, localizedText);
            }

        }

        Debug.Log("Success loading the localized texts for the section '" + section + "'.");
        return true;
    }

    private void SaveLocalizedText(Section section, LocalizedText localizedText)
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

    public LocalizedText GetLocalizedText(Section section, string id, bool register)
    {
        //TODO: Registration of the quarry to get the text if "register == true"

        foreach (LocalizedText localizedText in localizedTexts[section])
            if (localizedText.id == id)
                return localizedText;
        
        return new LocalizedText(id, -1, "The text with id '" + id + "' could not be found in the section '" + section + "'");
    }

    public delegate void LocalizeAllAction();
    public static event LocalizeAllAction OnLocalizeAllAction;

    private void LocalizeAllLocalizableObjects()
    {
        if (OnLocalizeAllAction == null) return;
        
        Debug.Log("   > " + "Localizing all objects");
        OnLocalizeAllAction();
    }
}
