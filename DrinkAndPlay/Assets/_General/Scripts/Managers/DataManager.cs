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

    public List<T> GetCloneOfList<T>(List<T> originalList)
    {
        return new List<T>(originalList);
    }

    #region language
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


    #region players
    private List<string> players
    {
        get
        {
            Debug.Log("Players get");
            if (_players == null)
            {
                _players = SaveGame.Load<List<string>>(playersSavename, new List<string>());
            }

            return _players;
        }
        set
        {
            if (!_players.SequenceEqual(value))
            {
                Debug.Log("Players set");
                _players = value;
                SaveGame.Save(playersSavename, value);
            }
        }
    }
    private List<string> _players;
    private string playersSavename = "players";
    public string GetPlayer(int playerNumber)
    {
        return players[playerNumber];
    }
    public int GetPlayerNumber(string player)
    {
        if (string.IsNullOrEmpty(player))
        {
            Debug.LogWarning("Searching the index of a null player");
            return -1;
        }

        return players.IndexOf(player);
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
    public Vector2Int naughtyLevel // naughtyLevel.x = min, naughtyLevel.y = max
    {
        get
        {
            if (_naughtyLevel == null)
            {
                _naughtyLevel = SaveGame.Load(naughtyLevelSavename, new Vector2Int(1, 10));
            }

            return _naughtyLevel;
        }
        set
        {
            if (_naughtyLevel != value)
            {
                _naughtyLevel = value;
                SaveGame.Save(naughtyLevelSavename, value);
            }
        }
    }
    private Vector2Int _naughtyLevel;
    private string naughtyLevelSavename = "naughtyLevel";
    #endregion


    #region customSentences
    private Dictionary<Section, List<string>> customSentences
    {
        get
        {
            if (_customSentences == null)
            {
                _customSentences = SaveGame.Load(customSentencesSavename, new Dictionary<Section, List<string>>());
            }

            return _customSentences;
        }
        set
        {
            if (!_customSentences.SequenceEqual(value))
            {
                _customSentences = value;
                SaveGame.Save(customSentencesSavename, value);
            }
        }
    }
    private Dictionary<Section, List<string>> _customSentences;
    private string customSentencesSavename = "customSentences";
    private Dictionary<Section, List<string>> GetCloneOfCustomSentences()
    {
        Dictionary<Section, List<string>> clonedDictionary = new Dictionary<Section, List<string>>();
        foreach (Section section in customSentences.Keys)
            clonedDictionary.Add(section, GetCloneOfList(customSentences[section]) );
        return clonedDictionary;
    }
    private void AddCustomSentence(Section section, string sentence)
    {
        Dictionary<Section, List<string>> clonedCS = GetCloneOfCustomSentences();
        clonedCS[section].Add(sentence);
        customSentences = clonedCS;
    }
    private void RemoveCustomSentence(Section section, string sentence)
    {
        Dictionary<Section, List<string>> clonedCS = GetCloneOfCustomSentences();
        clonedCS[section].Remove(sentence);
        customSentences = clonedCS;
    }
    private void RemoveCustomSentence(Section section, int sentence)
    {
        Dictionary<Section, List<string>> clonedCS = GetCloneOfCustomSentences();
        clonedCS[section].RemoveAt(sentence);
        customSentences = clonedCS;
    }
    #endregion


    /*
        #region textsRegistered
        public string textsRegistered
        {
            get
            {
                if (_textsRegistered == null)
                {
                    _textsRegistered = SaveGame.Load(textsRegisteredSavename, "en-us");
                }

                return _textsRegistered;
            }
            set
            {
                if (_textsRegistered.CompareTo(value) != 0)
                {
                    _textsRegistered = value;
                    SaveGame.Save(textsRegisteredSavename, value);
                }
            }
        }
        private string _textsRegistered;
        private string textsRegisteredSavename = "textsRegistered";
        #endregion

        #region Premium
        public string Premium
        {
            get
            {
                if (_Premium == null)
                {
                    _Premium = SaveGame.Load(PremiumSavename, "en-us");
                }

                return _Premium;
            }
            set
            {
                if (_Premium.CompareTo(value) != 0)
                {
                    _Premium = value;
                    SaveGame.Save(PremiumSavename, value);
                }
            }
        }
        private string _Premium;
        private string PremiumSavename = "Premium";
        #endregion
    */

}