using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MainMenuManager))]
public class MainMenuEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MainMenuManager mainMenuManager = (MainMenuManager)target;

        GUILayout.Space(25);
        if (GUILayout.Button("Generate menu"))
            mainMenuManager.GenerateMenu();
    }

}
