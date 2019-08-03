using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Section", menuName = "Section - Game")]
public class Section : ScriptableObject
{
    [Header("Section information")]
    public string sectionName;
    public string description;
    public Sprite image;
    public bool newness;
    public bool comingSoon;
    public bool premium;

    [Header("Section configuration")]
    public string localizationURL;
    public int minNumberOfPlayers;

    [Header("Start configuration")]
    public bool showNaughyLevel;
    public bool showPlayers;
    public bool showLanguage;

    public static string uiLocalizationURL { get { return "https://docs.google.com/spreadsheets/d/e/2PACX-1vQGs31fwKF9vuUg9uUOvgN8Jr7bVSQvDILQEMPk6xiKkzk3PDYosuOPMhd0FjrnKPzLkMA998tnZfGN/pub?gid=0&single=true&output=csv"; } private set { ; } }

    public Section()
    {
        minNumberOfPlayers = -1;
    }

}
