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

    public int quantityOfSentences
    {
        get
        {
            if (_quantityOfSentences < 0) 
                _quantityOfSentences = CalculateQuantityOfSentences();
            return _quantityOfSentences;
        }
        private set { _quantityOfSentences = value; }
    }

    private int _quantityOfSentences = -1;
    
    private int CalculateQuantityOfSentences()
    {
        return CSVReader.Read(this).ToArray().Length;
    }

    public override string ToString()
    {
        return name;
    }

    
    // AUTO-GENERATED
    private sealed class LocalizationUrlEqualityComparer : IEqualityComparer<LocalizationFile>
    {
        public bool Equals(LocalizationFile x, LocalizationFile y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.localizationUrl == y.localizationUrl;
        }

        public int GetHashCode(LocalizationFile obj)
        {
            return (obj.localizationUrl != null ? obj.localizationUrl.GetHashCode() : 0);
        }
    }
    public static IEqualityComparer<LocalizationFile> localizationUrlComparer { get; } = new LocalizationUrlEqualityComparer();
}