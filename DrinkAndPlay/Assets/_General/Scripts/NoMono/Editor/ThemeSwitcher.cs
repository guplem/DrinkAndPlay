using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThemeSwitcher : MonoBehaviour
{
    public static LightDarkColor.ColorType currentTheme = LightDarkColor.ColorType.Dark;
    
    [MenuItem("Drink and Play/Switch Theme %&d")]
    public static void SwitchTheme()
    {
        currentTheme = (currentTheme == LightDarkColor.ColorType.Light)
            ? LightDarkColor.ColorType.Dark
            : LightDarkColor.ColorType.Light;
        
        ColorSwitcher[] foundObjects = FindObjectsOfType<ColorSwitcher>();
        foreach (ColorSwitcher obj in foundObjects)
            obj.SetColorTo(currentTheme);
        
    }
    
}
