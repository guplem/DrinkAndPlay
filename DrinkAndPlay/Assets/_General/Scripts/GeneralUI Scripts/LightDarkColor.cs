using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Light and Dark color", menuName = "Color Light and Dark")]
public class LightDarkColor : ScriptableObject
{
    public Color lightColor;
    public Color darkColor;

    public enum ColorType
    {
        Light, 
        Dark
    }
    
    public LightDarkColor()
    {
        this.lightColor = Color.white;
        this.darkColor = Color.black;
    }
}
