using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Coctel", menuName = "Coctel - Recipe")]
public class Coctel : ScriptableObject
{
    [Header("Coctel information")]
    public string nameId;
    public string descriptionId;
    public Sprite image;
    public bool newness;
    public bool top;

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return GetHashCode() == ((Coctel)other).GetHashCode();
    }

    public override string ToString()
    {
        return name;
    }
}
