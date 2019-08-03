using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
    private ConfigurationManager configurationManager;

    public LocalizationManager(ConfigurationManager configurationManager)
    {
        this.configurationManager = configurationManager;
    }

    public bool LoadCurrentLanguage(string textsType)
    {
        return LoadLanguage(configurationManager.language, textsType);
    }

    public bool LoadLanguage(string lang, string textsType)
    {

        /*if (string.IsNullOrEmpty(lang))
        {
            Debug.LogError("The desired language is null or empty");
            return false;
        }*/


        /*
        TextAsset localizationFile = Resources.Load<TextAsset>("LocalizationFile.csv");
        string[] linesFromLocalizationFile = localizationFile.text.Split("\n"[0]);
        foreach (string line in linesFromLocalizationFile)
        {
            Debug.Log("LINE KKK: " + line);
        }
        */

        //Open file
        //Foreach line
        //Save the line as localizated text map


        List<Dictionary<string, object>> data = CSVReader.Read("LocalizationFile");

        for (var i = 0; i < data.Count; i++)
        {
            Debug.Log("Type '" + data[i]["Type"] + "'");
            Debug.Log("Id '" + data[i]["Id"] + "'");
            Debug.Log("NaughtyLevel '" + data[i]["NaughtyLevel"] + "'");
            Debug.Log("es-es '" + data[i]["es-es"] + "'");
            Debug.Log("en-us '" + data[i]["en-us"] + "'");
            Debug.Log("================================");
        }




        return true;
    }

    
}
