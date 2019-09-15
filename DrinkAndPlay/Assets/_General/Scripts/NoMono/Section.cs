using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Section", menuName = "Section - Game")]
public class Section : ScriptableObject
{
    [Header("Section information")]
    public string nameId;
    public string descriptionId;
    public Sprite image;
    public bool newness;
    public bool comingSoon;
    public bool premium;

    [Header("Section configuration")]
    public LocalizationFile[] localizationFiles;
    public int minNumberOfPlayers;
    public string sceneName;

    [Header("General UI configuration")]
    [Tooltip("If the value es false, the 'Back Button' and 'Configuration Button' will have false values too")]
    public bool topBar = true;
    public bool backButton = true;
    public bool sectionTitle = true;
    public bool gameTitle = false;
    public bool configButton = true;
    /*[Space(5)]
    [Tooltip("If the value es false, the 'Back Button' and 'Configuration Button' will have false values too")]
    public bool bottomBar = true;
    public bool likeButton = true;
    public bool addButton = true;
    public bool shareButton = true;*/

    [Header("Start configuration")]
    public bool showNaughtyLevelConfigurator;
    public bool showPlayersConfigurator;
    public bool showLanguageConfigurator;

    public Section()
    {
        minNumberOfPlayers = -1;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return other != null && GetHashCode() == ((Section)other).GetHashCode();
    }

    public override string ToString()
    {
        return name;
    }

}
