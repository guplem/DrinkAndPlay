using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataManager
{
    public DataManager(bool encode)
    {
        Debug.Log("Creating DataManager");
        SaveGame.Encode = encode;
    }

    private static List<T> GetCloneOfList<T>(List<T> originalList)
    {
        return new List<T>(originalList);
    }
    private Dictionary<string, List<string>> GetCloneOfDictionary(Dictionary<string, List<string>> originalDictionary)
    {
        Dictionary<string, List<string>> clonedDictionary = new Dictionary<string, List<string>>();
        foreach (string key in originalDictionary.Keys)
            clonedDictionary.Add(key, GetCloneOfList(originalDictionary[key]));
        return clonedDictionary;
    }


    #region language
    public string language
    {
        get
        {
            if (_language == null)
                _language = SaveGame.Load(languageSavename, "es-es");

            return _language;
        }
        set
        {
            if (string.Compare(_language, value, StringComparison.Ordinal) == 0) 
                return;
            
            Debug.Log("New language: " + value);
            _language = value;
            GameManager.instance.localizationManager.ReloadForCurrentLanguage();
            SaveGame.Save(languageSavename, value);
        }
    }
    private string _language;
    private const string languageSavename = "language";

    #endregion


    #region players
    private List<string> players
    {
        get
        {
            if (_players == null)
                _players = SaveGame.Load<List<string>>(playersSavename, new List<string>());

            return _players;
        }
        set
        {
            if (_players.SequenceEqual(value)) 
                return;
             
            _players = value;
            SaveGame.Save(playersSavename, value);
        }
    }
    private List<string> _players;
    private const string playersSavename = "players";

    public string GetPlayer(int playerNumber)
    {
        return players[playerNumber];
    }    
    public List<string> GetPlayers()
    {
        return GetCloneOfList(players);
    }
    public int GetPlayerNumber(string player)
    {
        if (!string.IsNullOrEmpty(player)) 
            return players.IndexOf(player);
        
        Debug.LogWarning("Searching the index of a null player");
        return -1;
    }
    public int GetPlayersQuantity()
    {
        return players.Count;
    }
    public bool CanAddPlayer(string player)
    {
        return !(players.Contains(player) || string.IsNullOrEmpty(player) || string.IsNullOrWhiteSpace(player));
    }
    public void AddPlayer(string player)
    {
        List<string> clonePlayers = GetCloneOfList(players);
        clonePlayers.Add(player);
        players = clonePlayers;
    }
    public void RemovePlayer(int playerIndex)
    {
        List<string> clonePlayers = GetCloneOfList(players);
        clonePlayers.RemoveAt(playerIndex);
        players = clonePlayers;
    }
    public void RemovePlayer(string player)
    {
        List<string> clonePlayers = GetCloneOfList(players);
        clonePlayers.Remove(player);
        players = clonePlayers;
    }
    #endregion


    #region naughtyLevel
    public class NaughtyLevel
    {
        public readonly int min;
        public readonly int max;

        public NaughtyLevel(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public override bool Equals(object obj)
        {
            return obj is NaughtyLevel level &&
                   min == level.min &&
                   max == level.max;
        }

        public override int GetHashCode()
        {
            int hashCode = -897720056;
            hashCode = hashCode * -1521134295 + min.GetHashCode();
            hashCode = hashCode * -1521134295 + max.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return "(" + min + "," + max + ")";
        }
    }
    private NaughtyLevel naughtyLevel // naughtyLevel.x = min, naughtyLevel.y = max
    {
        get
        {
            if (_naughtyLevel == null)
            {
                _naughtyLevel = SaveGame.Load<NaughtyLevel>(naughtyLevelSavename, new NaughtyLevel(1, 10));
            }

            return _naughtyLevel;
        }
        set
        {
            if (!_naughtyLevel.Equals(value))
            {
                _naughtyLevel = value;
                SaveGame.Save<NaughtyLevel>(naughtyLevelSavename, value);
            }
        }
    }
    private NaughtyLevel _naughtyLevel;
    public NaughtyLevel naughtyLevelExtremes { get { return _naughtyLevelExtremes; } private set { /*Intended not possible set*/ } }
    private readonly NaughtyLevel _naughtyLevelExtremes = new NaughtyLevel(1, 10);
    private const string naughtyLevelSavename = "naughtyLevel";

    public void SetNaughtyLevelMin(int value)
    {
        naughtyLevel = new NaughtyLevel(value, naughtyLevel.max);
    }
    public void SetNaughtyLevelMax(int value)
    {
        naughtyLevel = new NaughtyLevel(naughtyLevel.min, value);
    }
    public int GetNaughtyLevelMin()
    {
        return naughtyLevel.min;
    }
    public int GetNaughtyLevelMax()
    {
        return naughtyLevel.max;
    }
    public int GetRandomNaughtyLevel()
    {
        return Random.Range(GetNaughtyLevelMin(), GetNaughtyLevelMax() + 1);
    }
    #endregion



    #region textsRegistered
    private Dictionary<string, List<string>> textsRegistered
    {
        get
        {
            if (_textsRegistered == null)
                _textsRegistered = SaveGame.Load(textsRegisteredSavename, new Dictionary<string, List<string>>());

            return _textsRegistered;
        }
        set
        {
            if (_textsRegistered.SequenceEqual(value)) 
                return;
            
            _textsRegistered = value;
            SaveGame.Save(textsRegisteredSavename, value);
        }
    }
    private Dictionary<string, List<string>> _textsRegistered;
    private const string textsRegisteredSavename = "textsRegistered";

    public void AddTextRegistered(Section section, string textId)
    {
        try { if (textsRegistered[section.ToString()].Contains(textId)) return; } catch (KeyNotFoundException) { }
        
        Dictionary<string, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        try
        {
            clonedCs[section.ToString()].Add(textId);
        }
        catch (KeyNotFoundException)
        {
            clonedCs.Add(section.ToString(), new List<string>());
        }

        textsRegistered = clonedCs;
    }
    public void RemoveTextRegistered(Section section, string textId)
    {
        Dictionary<string, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        clonedCs[section.ToString()].Remove(textId);
        textsRegistered = clonedCs;
    }
    public bool IsTextRegistered(Section section, string textId)
    {
        try
        {
            return textsRegistered[section.ToString()].Contains(textId);
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }
    public int GetTextRegisteredQuantity(Section section)
    {
        try
        {
            return textsRegistered[section.ToString()].Count;
        }
        catch (KeyNotFoundException)
        {
            return 0;
        }
    }
    public int GetTextRegisteredQuantity(Section section, int naughtyLevel)
    {
        return GetRegisteredTexts(section, naughtyLevel).Count;
    }
    
    
    public void RemoveOldestTextRegistered(Section section)
    {
        RemoveNOldestTextRegistered(section, 1);
    }

    private void RemoveNOldestTextRegistered(Section section, int quantity)
    {
        Dictionary<string, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        clonedCs[section.ToString()].RemoveRange(0, quantity);
        textsRegistered = clonedCs;
    }

    public void RemoveOldestPercentageOfTextsRegistered(Section section, float percentage, int naughtyLevel)
    {
        List<string> regTexts = new List<string>();
        
        // List all the registered texts in the section with the selected Naughty Level
        regTexts = naughtyLevel == -1 ? GetRegisteredTexts(section) : GetRegisteredTexts(section, naughtyLevel);

        // Remove the desired quantity of the registered texts
        int quantityToRemove = (int) (percentage * regTexts.Count / 100);
        regTexts.RemoveRange(0, quantityToRemove);

        // Apply the changes to a clone
        Dictionary<string, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        clonedCs[section.ToString()] = regTexts;
        
        textsRegistered = clonedCs;
    }
    
    public void RemoveOldestPercentageOfTextsRegistered(Section section, float percentage)
    {
        RemoveOldestPercentageOfTextsRegistered(section, percentage, -1);
    }

    private List<string> GetRegisteredTexts(Section section, int naughtyLevel)
    {
        List<string> regTextsWithProperNl = new List<string>();
        foreach (string textId in textsRegistered[section.ToString()])
        {
            LocalizedText curr = GameManager.instance.localizationManager.GetLocalizedText(section, textId, false);
            if (curr.naughtiness == naughtyLevel || naughtyLevel == -1)
                regTextsWithProperNl.Add(textId);
        }

        return regTextsWithProperNl;
    }
    
    private List<string> GetRegisteredTexts(Section section)
    {
        return textsRegistered[section.ToString()];
    }
        
    #endregion


    #region RandomChallenges

    public bool randomChallenges
    {
        get
        {
            //if (_randomChallenges == true)
                _randomChallenges = SaveGame.Load(randomChallengesSavename, true);

            return _randomChallenges;
        }
        set
        {
            if (_randomChallenges == value) 
                return;
            
            Debug.Log("New random challenges state: " + value);
            _randomChallenges = value;
            SaveGame.Save(randomChallengesSavename, value);
        }
    }
    private bool _randomChallenges;
    private const string randomChallengesSavename = "randomChallenges";

    #endregion

    
    #region RatePopup
    public bool ratePopupShown;
    public bool ratedApp
    {
        get
        {
            //if (_ratedApp == true)
            _ratedApp = SaveGame.Load(ratedAppSavename, false);

            return _ratedApp;
        }
        set
        {
            if (_ratedApp == value) 
                return;
            
            Debug.Log("New ratedApp state: " + value);
            _ratedApp = value;
            SaveGame.Save(ratedAppSavename, value);
        }
    }
    private bool _ratedApp;
    private const string ratedAppSavename = "ratedApp";

    #endregion
    
}