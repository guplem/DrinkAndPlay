using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
    private ConfigurationManager configurationManager;
    Dictionary<Section, List<LocalizedText>> localizedTexts;

    public LocalizationManager(ConfigurationManager configurationManager)
    {
        this.configurationManager = configurationManager;
        localizedTexts = new Dictionary<Section, List<LocalizedText>>();
    }

    public bool SetLanguage(string newLang)
    {
        configurationManager.language = newLang;
        Debug.LogWarning("LoadLanguage() not implemented yet"); //TODO: Foreach loaded section, load the language
        return false;
    }

    public bool LoadCurrentLanguage(Section section)
    {
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

        string[][] dataReaded = CSVReader.Read(section).ToArray();
        int languageColumn = -1;

        for (int row = 0; row < dataReaded.Length; row++)
        {

            //Check where is everything in the file
            if (row == 0)
            {
                for (int col = 0; col < dataReaded[row].Length; col++)
                {

                    // Check that the format is correct
                    if (col == 0 && dataReaded[row][col].ToUpper().CompareTo("ID") != 0)
                        Debug.LogError("The localizatioin file for " + section + " does not have the 1st colum setted as 'ID'.");
                    if (col == 1 && dataReaded[row][col].ToUpper().CompareTo("NAUGHTINESS") != 0)
                        Debug.LogError("The localizatioin file for " + section + " does not have the 2nd colum setted as 'NAUGHTINESS'.");

                    //Check if the column is the language
                    if (col > 1)
                        if (dataReaded[row][col].ToUpper().CompareTo(lang.ToUpper()) == 0)
                        {
                            languageColumn = col;
                            break;
                        }
                }
            }


            //Save the localized text with the proper language
            else
            {
                int.TryParse(dataReaded[row][1], out int naughtiness);
                LocalizedText localizedText = new LocalizedText(dataReaded[row][0], naughtiness, dataReaded[row][languageColumn]);
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
        //TODO: Registration of the querry to get the text if "register == true"

        foreach (LocalizedText localizedText in localizedTexts[section])
        {
            if (localizedText.id == id)
                return localizedText;
        }
        return new LocalizedText(id, -1, "The text with id " + id + " could not be found in the section " + section);
    }

    public delegate void LocalizeAllAction();
    public static event LocalizeAllAction OnLocalizeAllAction;
    public void LocalizeAllLocalizableObjects()
    {
        if (OnLocalizeAllAction != null)
        {
            Debug.Log("   > " + "Localizing all objects");
            OnLocalizeAllAction();
        }
    }
}
