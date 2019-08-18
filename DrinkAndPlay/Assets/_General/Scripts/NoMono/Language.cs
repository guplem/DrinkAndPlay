using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Language", menuName = "Language")]
public class Language : ScriptableObject, IComparable<Language>
{
    public string languageId;
    public string languageName;

    public int CompareTo(Language other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(languageName, other.languageName, StringComparison.OrdinalIgnoreCase);
    }
}
