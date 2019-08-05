using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MainMenu))]
public class MainMenuEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MainMenu mainMenuManager = (MainMenu)target;

        GUILayout.Space(25);
        if (GUILayout.Button("Generate menu"))
            mainMenuManager.GenerateMenu();
    }

}
