using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Language", menuName = "Language")]
public class Language : ScriptableObject, IComparable<Language>
{
    [FormerlySerializedAs("languageId")] public string id;
    [FormerlySerializedAs("languageName")] public string titleName;
    public bool isEnabled = true;

    public int CompareTo(Language other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(titleName, other.titleName, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString()
    {
        return id;
    }
}
