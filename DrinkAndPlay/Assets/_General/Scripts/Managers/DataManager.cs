using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private Dictionary<Section, List<string>> GetCloneOfDictionary(Dictionary<Section, List<string>> originalDictionary)
    {
        Dictionary<Section, List<string>> clonedDictionary = new Dictionary<Section, List<string>>();
        foreach (Section section in originalDictionary.Keys)
            clonedDictionary.Add(section, GetCloneOfList(originalDictionary[section]));
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
            GameManager.instance.localizationManager.ReloadCurrentLanguage();
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
            Debug.Log("Players get");
            if (_players == null)
                _players = SaveGame.Load<List<string>>(playersSavename, new List<string>());

            return _players;
        }
        set
        {
            if (_players.SequenceEqual(value)) 
                return;
            
            Debug.Log("Players set");
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
    #endregion


    #region customSentences
    private Dictionary<Section, List<string>> customSentences
    {
        get
        {
            if (_customSentences == null)
                _customSentences = SaveGame.Load(customSentencesSavename, new Dictionary<Section, List<string>>());

            return _customSentences;
        }
        set
        {
            if (_customSentences.SequenceEqual(value)) 
                return;
            
            _customSentences = value;
            SaveGame.Save(customSentencesSavename, value);
        }
    }
    private Dictionary<Section, List<string>> _customSentences;
    private const string customSentencesSavename = "customSentences";

    private void AddCustomSentence(Section section, string sentence)
    {
        Dictionary<Section, List<string>> clonedCs = GetCloneOfDictionary(customSentences);
        clonedCs[section].Add(sentence);
        customSentences = clonedCs;
    }
    private void RemoveCustomSentence(Section section, string sentence)
    {
        Dictionary<Section, List<string>> clonedCs = GetCloneOfDictionary(customSentences);
        clonedCs[section].Remove(sentence);
        customSentences = clonedCs;
    }
    private void RemoveCustomSentence(Section section, int sentence)
    {
        Dictionary<Section, List<string>> clonedCs = GetCloneOfDictionary(customSentences);
        clonedCs[section].RemoveAt(sentence);
        customSentences = clonedCs;
    }
    #endregion


    #region textsRegistered
    private Dictionary<Section, List<string>> textsRegistered
    {
        get
        {
            if (_textsRegistered == null)
                _textsRegistered = SaveGame.Load(textsRegisteredSavename, new Dictionary<Section, List<string>>());

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
    private Dictionary<Section, List<string>> _textsRegistered;
    private const string textsRegisteredSavename = "textsRegistered";

    private void AddTextRegistered(Section section, string textId)
    {
        Dictionary<Section, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        clonedCs[section].Add(textId);
        textsRegistered = clonedCs;
    }
    public void RemoveTextRegistered(Section section, string textId)
    {
        Dictionary<Section, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        clonedCs[section].Remove(textId);
        textsRegistered = clonedCs;
    }
    public bool IsTextRegistered(Section section, string textId)
    {
        return textsRegistered[section].Contains(textId);
    }
    public int GetTextRegisteredQuantity(Section section)
    {
        return textsRegistered[section].Count;
    }
    public void RemoveOldestTextRegistered(Section section)
    {
        RemoveNOldestTextRegistered(section, 1);
    }

    private void RemoveNOldestTextRegistered(Section section, int quantity)
    {
        Dictionary<Section, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        clonedCs[section].RemoveRange(0, quantity);
        textsRegistered = clonedCs;
    }
    #endregion

}