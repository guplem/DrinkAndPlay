using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Localization File", menuName = "Localization File")]
public class LocalizationFile : ScriptableObject
{
    [TextArea]
    public string localizationUrl;
    public bool checkForEnoughSentencesOfAllNaughtyLevels = true;
    
    public override string ToString()
    {
        return name;
    }
}
