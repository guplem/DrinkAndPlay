using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{

    public DataManager(bool encode)
    {
        Debug.Log("Creating DataManager");
        SaveGame.Encode = encode;
    }

    #region Language
    public string language
    {
        get
        {
            if (_language == null)
            {
                _language = SaveGame.Load(languageSavename, "en-us");
            }

            return _language;
        }
        set
        {
            if (_language.CompareTo(value) != 0)
            {
                _language = value;
                SaveGame.Save(languageSavename, value);
            }
        }
    }
    private string _language;
    private string languageSavename = "language";
    #endregion


}