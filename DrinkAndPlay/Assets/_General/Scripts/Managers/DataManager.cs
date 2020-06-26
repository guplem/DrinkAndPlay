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
        Debug.Log("Creating DataManager.");
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

    public Section lastSelectedSection;

    #region selectedLocalizationFiles
    public List<LocalizationFile> lastSelectedLocalizationFiles { get; private set; }

    public void SetSelectedLocalizationFiles(LocalizationFile[] allLocalizationFiles, bool[] activatedLocFiles)
    {
        List<LocalizationFile> selectedLocFiles = new List<LocalizationFile>();
        for (int i = 0; i < allLocalizationFiles.Length; i++)
        {
            if (activatedLocFiles[i])
                selectedLocFiles.Add(allLocalizationFiles[i]);
        }
        SetSelectedLocalizationFiles(selectedLocFiles);
    }
    
    public void SetSelectedLocalizationFiles(List<LocalizationFile> selectedLocalizationFiles)
    {
        string stringOfSelected = " >>> List of selected localization files: ";
        foreach (LocalizationFile localizationFile in selectedLocalizationFiles)
            stringOfSelected += localizationFile.ToString() + ", ";
        Debug.Log(stringOfSelected);

        lastSelectedLocalizationFiles = new List<LocalizationFile>();
        foreach (LocalizationFile locFile in selectedLocalizationFiles)
            lastSelectedLocalizationFiles.Add(locFile);
    }
    
    /*public LocalizationFile GetRandomSelectedLocalizationFiles(bool uniformSentenceProbabilityDistribution)
    //uniformSentenceProbabilityDistribution --> All localizations will finish at the same time (to avoid repetition)
    {
        if (lastSelectedLocalizationFiles.Count <= 0) return null;
        
        if (!uniformSentenceProbabilityDistribution)
            return lastSelectedLocalizationFiles[Random.Range(0, GetSelectedLocalizationFilesQuantity())];

        int totalSentences = 0;
        foreach (LocalizationFile lf in lastSelectedLocalizationFiles)
            totalSentences += lf.quantityOfSentences;
        int currentValue = 0;
        int randomValue = Random.Range(0, totalSentences);
        foreach (LocalizationFile lf in lastSelectedLocalizationFiles)
        {
            currentValue += lf.quantityOfSentences;
            if (currentValue >= randomValue)
                return lf;
        }
        
        return null;
    }*/
    
    public int GetSelectedLocalizationFilesQuantity()
    {
        return lastSelectedLocalizationFiles.Count;
    }

    public bool IsSelectedLocalizationFilesListInitialized()
    {
        return lastSelectedLocalizationFiles != null;
    }

    #endregion
    

    #region language
    public Language language
    {
        get
        {
            if (_language == null)
                _language = SaveGame.Load(languageSavename, GetSystemLanguage() );

            return _language;
        }
        set
        {
            if (string.Compare(_language.id, value.id, StringComparison.Ordinal) == 0) 
                return;
            
            Debug.Log("New language: " + value);
            _language = value;
            GameManager.instance.localizationManager.ReloadForCurrentLanguage();
            SaveGame.Save(languageSavename, value);
        }
    }
    private Language _language;
    private const string languageSavename = "language";

    private Language GetSystemLanguage()
    {
        Language lang;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Spanish:
                lang = GetEnabledLanguageWithId("es-es");
                return lang != null ? lang : GetEnabledLanguageWithId(GameManager.instance.defaultLanguage.id);

            case SystemLanguage.Basque:
                lang = GetEnabledLanguageWithId("es-es");
                return lang != null ? lang : GetEnabledLanguageWithId(GameManager.instance.defaultLanguage.id);
            
            case SystemLanguage.Catalan:
                lang = GetEnabledLanguageWithId("ca");
                return lang != null ? lang : GetEnabledLanguageWithId(GameManager.instance.defaultLanguage.id);
            
            case SystemLanguage.English:
                lang = GetEnabledLanguageWithId("en-us");
                return lang != null ? lang : GetEnabledLanguageWithId(GameManager.instance.defaultLanguage.id);
            
            default:
                return GetEnabledLanguageWithId(GameManager.instance.defaultLanguage.id);
        }
    }

    private Language GetEnabledLanguageWithId(string id)
    {
        foreach (Language language in GameManager.instance.languages)
            if (id.Equals(language.id, StringComparison.InvariantCultureIgnoreCase))
                return language.isEnabled ? language : null;
        return null;
    }
    
    #endregion


    #region players
    private List<Player> players
    {
        get
        {
            if (_players == null)
            {
                _players = SaveGame.Load<List<Player>>(playersSavename, new List<Player>());
                DebugPro.LogEnumerable(_players, " - ", "Loaded Players: ");
            }

            return _players;
        }
        set
        {
            //if (_players.SequenceEqual(value)) 
            //    return;
             
            _players = value;
            SaveGame.Save(playersSavename, value);
            DebugPro.LogEnumerable(_players, " - ", "Saved Players: ");
            GenerateNewRandomRoundOrder();
        }
    }
    private List<Player> _players;
    private const string playersSavename = "players";

    public Player GetPlayer(int playerNumber)
    {
        if (GetPlayersQuantity() <= 0)
        {
            string fakeName = GameManager.instance.localizationManager.SearchLocalizedText(GameManager.instance.uiLocalizationFile, "ChoosePlayer", false).text;
            return new Player(fakeName);
        }

        if (playerNumber > GetPlayersQuantity() - 1 || playerNumber < 0)
        {
            string fakeName = "PLAYER '" + playerNumber + "' NOT EXISTENT";
            return new Player(fakeName);
        }
        
        return players[playerNumber];
    }    
    public List<Player> GetPlayers()
    {
        return GetCloneOfList(players);
    }
    public int GetPlayersQuantity()
    {
        return players.Count;
    }
    public int GetEnabledPlayersQuantity()
    {
        return GetPlayers().Count(p => p.enabled);
    }
    public bool CanAddPlayer(Player player)
    {
        return !(players.Contains(player) || player==null || string.IsNullOrEmpty(player.name) || string.IsNullOrWhiteSpace(player.name));
    }
    public void AddPlayer(Player player)
    {
        List<Player> clonedPlayers = GetCloneOfList(players);
        clonedPlayers.Add(player);
        players = clonedPlayers;
    }
    public void RemovePlayer(Player player)
    {
        List<Player> clonedPlayers = GetCloneOfList(players);
        clonedPlayers.Remove(player);
        players = clonedPlayers;
    }

    public void SetPlayerEnabled(Player player, bool state)
    {
        List<Player> clonedPlayers = GetCloneOfList(players);
        foreach (Player p in clonedPlayers)
            if (p == player)
                p.enabled = state;
        
        players = clonedPlayers;
    }

    private int playerTurn = 0;

    public void SetTurnForNextPlayer()
    {
        playerTurn ++;
        if (playerTurn >= GetPlayersQuantity())
        {
            playerTurn = 0;
            GenerateNewRandomRoundOrder();
        }

        if (!GetCurrentPlayer().enabled)
            SetTurnForNextPlayer();
    }
    
    public void PreviousPlayerTurn()
    {
        playerTurn --;
        if (playerTurn < 0)
            playerTurn = GetPlayersQuantity()-1;
        
        if (!GetCurrentPlayer().enabled)
            PreviousPlayerTurn();
    }

    private int[] randomRoundOrder;
    private void GenerateNewRandomRoundOrder()
    {
        randomRoundOrder = new int[GetPlayersQuantity()];
        
        List<int> numbersToOrder = new List<int>();
        for (int i = 0; i < GetPlayersQuantity(); i++)
            //if (GetPlayer(i).enabled)
                numbersToOrder.Add(i);

        for (int r = 0; r < randomRoundOrder.Length; r++)
        {
            randomRoundOrder[r] = numbersToOrder[Random.Range(0, numbersToOrder.Count)];
            numbersToOrder.Remove(randomRoundOrder[r]);
        }
    }



    public Player GetCurrentPlayer()
    {
        if (!randomPlayerOrder)
            return GetPlayer(playerTurn);
        else
            return GetPlayer(GetCurrentRandomPlayerNumber());
    }

    public Player GetRandomPlayer()
    {
        return GetPlayer(Random.Range(0, GetPlayersQuantity()));
    }

    private int GetCurrentRandomPlayerNumber()
    {
        if (randomRoundOrder == null || randomRoundOrder.Length <= 0)
        {
            GenerateNewRandomRoundOrder();
            if (randomRoundOrder == null)
                Debug.LogError("The random round order of the players has not been generated.");

            if (randomRoundOrder.Length <= 0 || randomRoundOrder.Length > GetPlayersQuantity())
                Debug.LogError($"Error generating the new random round order of the players. Length: {randomRoundOrder.Length}");
        }

        return randomRoundOrder[playerTurn];
    }

    public Player GetRandomPlayer(List<Player> exclusions)
    {
        Player player = null;
        if (exclusions.Count <= 0)
            player = GetRandomPlayer();
        else
        {
            List<Player> clonedPlayers = GetCloneOfList(players);

            foreach (Player excluded in exclusions)
                clonedPlayers.Remove(excluded);

            if (players.Count > 0)
            {
                player = clonedPlayers[Random.Range(0, clonedPlayers.Count)];
            }
            else
                player = GetRandomPlayer();
        }

        if (!player.enabled)
        {
            exclusions.Add(player);
            return GetRandomPlayer(exclusions);
        }
        
        return player;
    }
    
    public bool HaveEnougheNABLEDPlayersFor(Section section)
    {
        if (section != null)
            return ! (section.minNumberOfPlayers > 0 && section.minNumberOfPlayers > GetEnabledPlayersQuantity());

        Debug.LogWarning("Checking if there are enough players to play a NULL section.");
        return true;
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
    public NaughtyLevel naughtyLevelExtremes { get { return _naughtyLevelExtremes; } private set { /*Intended not possible set*/ } }
    private readonly NaughtyLevel _naughtyLevelExtremes = new NaughtyLevel(1, 5);
    public NaughtyLevel naughtyLevel // naughtyLevel.x = min, naughtyLevel.y = max
    {
        get
        {
            if (_naughtyLevel == null)
            {
                _naughtyLevel = SaveGame.Load<NaughtyLevel>(naughtyLevelSavename, naughtyLevelExtremes);

                if (naughtyLevel.max > naughtyLevelExtremes.max) naughtyLevel = new NaughtyLevel(naughtyLevel.min, naughtyLevelExtremes.max);
                if (naughtyLevel.min < naughtyLevelExtremes.min) naughtyLevel = new NaughtyLevel(naughtyLevelExtremes.min, naughtyLevel.max);
            }

            return _naughtyLevel;
        }
        private set
        {
            if (!_naughtyLevel.Equals(value))
            {
                _naughtyLevel = value;
                SaveGame.Save<NaughtyLevel>(naughtyLevelSavename, value);
            }
        }
    }
    private NaughtyLevel _naughtyLevel;

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

    public bool IsValueOfNaughtyLevelCorrect(int valueToCheck)
    {
        return (naughtyLevel.min <= valueToCheck) && (valueToCheck <= naughtyLevel.max);
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

    public void AddTextRegistered(LocalizationFile localizationFile, string textId)
    {
        try { if (textsRegistered[localizationFile.ToString()].Contains(textId)) return; } catch (KeyNotFoundException) { }
        
        Dictionary<string, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        
        if (!textsRegistered.ContainsKey(localizationFile.ToString()))
            clonedCs.Add(localizationFile.ToString(), new List<string>());
        
        clonedCs[localizationFile.ToString()].Add(textId);

        textsRegistered = clonedCs;
        
        /*Debug.Log("Text registered.  LF: " + localizationFile + " ID: " + textId);
        foreach (KeyValuePair<string, List<string>> kvp in textsRegistered)
        {
            string str = "";
            foreach (string ids in kvp.Value)
                str += ids + ", ";
            Debug.Log ("DataManager information:\n LocalizationFile: " + kvp.Key + ". + Registered: " + str);
        }*/
    }
    
    public void RemoveTextRegistered(Section section, string textId)
    {
        Dictionary<string, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        clonedCs[section.ToString()].Remove(textId);
        textsRegistered = clonedCs;
    }
    public bool IsTextRegistered(LocalizationFile localizationFile, string textId)
    {
        try
        {
            return textsRegistered[localizationFile.ToString()].Contains(textId);
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }
    /*public int GetTextRegisteredQuantity(LocalizationFile localizationFile)
    {
        try
        {
            return textsRegistered[localizationFile.ToString()].Count;
        }
        catch (KeyNotFoundException)
        {
            return 0;
        }
    }*/
    public int GetTextRegisteredQuantity(LocalizationFile localizationFile, bool properNaughtyLevel)
    {
        return GetRegisteredTexts(localizationFile, properNaughtyLevel).Count;
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

    public void RemoveOldestPercentageOfTextsRegistered(LocalizationFile localizationFile, float percentage,  bool properNaughtyLevel)
    {
        List<string> regTexts = new List<string>();
        
        // List all the registered texts in the localizationFile with the selected Naughty Level
        regTexts = GetRegisteredTexts(localizationFile, properNaughtyLevel);
        regTexts.DebugLog(" >>> Sentences found to reset: ");
        
        // Remove the desired quantity of the registered texts
        int quantityToRemove = (int) (percentage * regTexts.Count / 100);
        regTexts.RemoveRange(0, quantityToRemove);

        // Apply the changes to a clone
        Dictionary<string, List<string>> clonedCs = GetCloneOfDictionary(textsRegistered);
        clonedCs[localizationFile.ToString()] = regTexts;
        
        textsRegistered = clonedCs;
    }

    private List<string> GetRegisteredTexts(LocalizationFile localizationFile, bool properNaughtyLevel)
    {
        if (!properNaughtyLevel) return textsRegistered[localizationFile.ToString()];
        
        List<string> regTextsWithProperNl = new List<string>();
        foreach (string textId in textsRegistered[localizationFile.ToString()])
        {
            LocalizedText curr = GameManager.instance.localizationManager.SearchLocalizedText(localizationFile, textId, false);
            if (IsValueOfNaughtyLevelCorrect(curr.naughtiness))
                regTextsWithProperNl.Add(textId);
        }

        return regTextsWithProperNl;
    }
    
    /*private List<string> GetRegisteredTexts(LocalizationFile localizationFile)
    {
        return textsRegistered[localizationFile.ToString()];
    }*/
        
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
            
            Debug.Log("New randomChallenges state: " + value);
            _randomChallenges = value;
            SaveGame.Save(randomChallengesSavename, value);
        }
    }
    private bool _randomChallenges;
    private const string randomChallengesSavename = "randomChallenges";

    #endregion

    
    #region RandomPlayerOrder

    public bool randomPlayerOrder
    {
        get
        {
            _randomPlayerOrder = SaveGame.Load(randomPlayerOrderSavename, false);
            return _randomPlayerOrder;
        }
        set
        {
            if (_randomPlayerOrder == value) 
                return;
            
            Debug.Log("New randomPlayerOrder state: " + value);
            _randomPlayerOrder = value;
            SaveGame.Save(randomPlayerOrderSavename, value);
        }
    }
    private bool _randomPlayerOrder;
    private const string randomPlayerOrderSavename = "randomPlayerOrder";

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


    #region DarkMode

    public bool darkMode
    {
        get
        {
            //TODO: default value depending on time of the time or depending on device configuration
            _darkMode = SaveGame.Load(darkModeSavename, true);
            return _darkMode;
        }
        set
        {
            if (_darkMode == value) 
                return;
            
            Debug.Log("New dark mode state: " + value);
            _darkMode = value;
            
            SaveGame.Save(darkModeSavename, value);
            
            if (changedVisualMode != null)
                changedVisualMode(GetVisualMode());
        }
    }

    public Action<LightDarkColor.ColorType> changedVisualMode;
    private bool _darkMode;
    private const string darkModeSavename = "darkMode";

    public LightDarkColor.ColorType GetVisualMode()
    {
        return darkMode? LightDarkColor.ColorType.Dark : LightDarkColor.ColorType.Light;
    }
    
    #endregion


    #region BetaTester

    public bool betaTester
    {
        get
        {
            _betaTester = SaveGame.Load(betaTesterSavename, false);
            return _betaTester;
        }
        private set
        {
            if (_betaTester == value || value == false) 
                return;
            
            _betaTester = value;
            
            SaveGame.Save(betaTesterSavename, value);
        }
    }
    private bool _betaTester = false;
    private const string betaTesterSavename = "betaTester";

    #endregion
    
    
    #region author
    public string author
    {
        get
        {
            if (_author == null)
                _author = SaveGame.Load(authorSavename, "" );

            return _author;
        }
        set
        {
            if (string.Compare(_author, value, StringComparison.Ordinal) == 0) 
                return;
            
            Debug.Log("New author: " + value);
            
            _author = value;
            
            SaveGame.Save(authorSavename, value);
        }
    }
    private string _author;
    private const string authorSavename = "author";

    #endregion


}