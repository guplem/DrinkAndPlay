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
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.GetType() != this.GetType()) return false;
        return Equals((LocalizationFile) other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (localizationUrl != null ? localizationUrl.GetHashCode() : 0);
        }
    }
}