using BayatGames.SaveGameFree;
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

    public List<T> GetCloneList<T>(List<T> originalList)
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
                _players = SaveGame.Load<List<string>>(playersSavename, new List<string>() );
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

    /// Player's methods

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
    public int GetPlayersNumber()
    {
        return players.Count;
    }
    public void AddPlayer(string player)
    {
        List<string> clonePlayers = GetCloneList(players);
        clonePlayers.Add(player);
        players = clonePlayers;
    }
    public void RemovePlayer(int playerIndex)
    {
        List<string> clonePlayers = GetCloneList(players);
        clonePlayers.RemoveAt(playerIndex);
        players = clonePlayers;
    }
    public void RemovePlayer(string player)
    {
        List<string> clonePlayers = GetCloneList(players);
        clonePlayers.Remove(player);
        players = clonePlayers;
    }


    #endregion

    /*
        #region naughtyLevel
        public string naughtyLevel
        {
            get
            {
                if (_naughtyLevel == null)
                {
                    _naughtyLevel = SaveGame.Load(naughtyLevelSavename, "en-us");
                }

                return _naughtyLevel;
            }
            set
            {
                if (_naughtyLevel.CompareTo(value) != 0)
                {
                    _naughtyLevel = value;
                    SaveGame.Save(naughtyLevelSavename, value);
                }
            }
        }
        private string _naughtyLevel;
        private string naughtyLevelSavename = "naughtyLevel";
        #endregion

        #region customSentences
        public string customSentences
        {
            get
            {
                if (_customSentences == null)
                {
                    _customSentences = SaveGame.Load(customSentencesSavename, "en-us");
                }

                return _customSentences;
            }
            set
            {
                if (_customSentences.CompareTo(value) != 0)
                {
                    _customSentences = value;
                    SaveGame.Save(customSentencesSavename, value);
                }
            }
        }
        private string _customSentences;
        private string customSentencesSavename = "customSentences";
        #endregion

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