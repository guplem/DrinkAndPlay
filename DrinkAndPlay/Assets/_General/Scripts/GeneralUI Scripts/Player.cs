using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name; // kept public instead of replaced by nameTrimmed to ensure compatibility with the data stored in the user's devices
    
    public bool enabled;
    public string nameTrimmed => name.IsNullOrEmpty()? "" : name.Trim();

    public Player(string name, bool enabled = true)
    {
        this.name = name;
        this.enabled = enabled;
    }

    protected bool Equals(Player other)
    {
        return string.Equals(nameTrimmed, other.nameTrimmed, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Player) obj);
    }

    public override int GetHashCode()
    {
        return (nameTrimmed != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(nameTrimmed) : 0);
    }

    public static bool operator ==(Player left, Player right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Player left, Player right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"Player '{nameTrimmed}'. Enabled: {nameTrimmed}.";
    }
}
