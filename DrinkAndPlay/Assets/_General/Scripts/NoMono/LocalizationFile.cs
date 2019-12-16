using System;
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

    public override bool Equals(object other)
    {
        LocalizationFile lf;
        try
        {
            lf = (LocalizationFile) other;
        }
        catch (InvalidCastException) {return false;}

        return lf != null && String.Compare(lf.localizationUrl, this.localizationUrl, StringComparison.Ordinal) == 0;
    }
    
}