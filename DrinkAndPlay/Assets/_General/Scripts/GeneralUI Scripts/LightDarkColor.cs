using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Light and Dark color", menuName = "Color Light and Dark")]
public class LightDarkColor : ScriptableObject
{

    [Header("Inheritance of colors")]
    [Tooltip("If set, the colors of the attached 'LightDarkColor' will be used instead of the manually selected colors.")]
    public LightDarkColor useTheColorsOf;

    [Header("Custom colors")]
    [Tooltip("Only used if there is no LightDarkColor assigned in 'Use TheColors Of'.")]
    [SerializeField] private Color _lightColor;
    [Tooltip("Only used if there is no LightDarkColor assigned in 'Use TheColors Of'.")]
    [SerializeField] private Color _darkColor;

    public Color lightColor
    {
        get { return useTheColorsOf == null ? _lightColor : useTheColorsOf.lightColor; }
        private set { }
    }
    public Color darkColor    
    {
        get { return useTheColorsOf == null ? _darkColor : useTheColorsOf.darkColor; }
        private set { }
    }

    public enum ColorType
    {
        Light, 
        Dark
    }
    
    public LightDarkColor()
    {
        this._lightColor = Color.white;
        this._darkColor = Color.black;
    }
}
