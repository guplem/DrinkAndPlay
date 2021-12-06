using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedText
{
    public string id { get; }
    public int naughtiness { get; }
    public string text { get; }
    
    public string author { get; }

    public string specialOptions { get; }

    public LocalizedText(string id, int naughtiness, string text, string author, string specialOptions)
    {
        this.id = id;
        this.naughtiness = naughtiness;
        this.text = text;
        this.author = author;
        this.specialOptions = specialOptions;
    }
    public override int GetHashCode()
    {
        return id.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return other != null && GetHashCode() == ((LocalizedText) other).GetHashCode();
    }

    public override string ToString()
    {
        return $"Id: '{id}', Naughtiness: '{naughtiness}', Text: '{text}', Author: '{author}', SpecialOptions: '{specialOptions}'";
    }

}
