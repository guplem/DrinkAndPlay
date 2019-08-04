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
    [TextArea]
    public string localizationURL;
    public int minNumberOfPlayers;
    public string sceneName;

    [Header("Start configuration")]
    public bool showNaughyLevelConfigurator;
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
        return GetHashCode() == ((Section)other).GetHashCode();
    }

    public override string ToString()
    {
        return name;
    }
}
