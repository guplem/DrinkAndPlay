using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedText
{
    public string id { get; private set; }
    public int naughtiness { get; private set; }
    public string text { get; private set; }

    public LocalizedText(string id, int naughtiness, string text)
    {
        this.id = id;
        this.naughtiness = naughtiness;
        this.text = text;
    }
    public override int GetHashCode()
    {
        return id.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return GetHashCode() == ((LocalizedText)other).GetHashCode();
    }

    public override string ToString()
    {
        return "Id: " + id + ", Naughtiness: " + naughtiness + ", Text: " + text /*+ ". Hash code = " + GetHashCode()*/;
    }

}
