using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ColorSwitcher))]
public class ColorSwitcherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ColorSwitcher colorSwitcher = (ColorSwitcher)target;

        GUILayout.Space(25);
        if (GUILayout.Button("Set light color"))
            colorSwitcher.SetColorTo(LightDarkColor.ColorType.Light);
        if (GUILayout.Button("Set dark color"))
            colorSwitcher.SetColorTo(LightDarkColor.ColorType.Dark);
    }
}
