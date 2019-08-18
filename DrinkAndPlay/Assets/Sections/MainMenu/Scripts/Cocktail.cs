using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cocktail", menuName = "Cocktail - Recipe")]
public class Cocktail : ScriptableObject
{
    [Header("Cocktail information")]
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
        return other != null && GetHashCode() == ((Cocktail)other).GetHashCode();
    }

    public override string ToString()
    {
        return name;
    }
}
