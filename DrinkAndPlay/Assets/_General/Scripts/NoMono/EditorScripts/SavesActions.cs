using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BayatGames.SaveGameFree;
using System.IO;

public class SavesActions : MonoBehaviour
{
    [MenuItem("Drink and Play/Saves/Clear data")]
    public static void EraseData()
    {
        SaveGame.Clear();
        Debug.Log("Data erased");
    }

    [MenuItem("Drink and Play/Saves/Open folder")]
    public static void OpenFolder()
    {
        DirectoryInfo[] directories = SaveGame.GetDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            string path = directory.Parent.FullName;
            System.Diagnostics.Process.Start(@path);
            Debug.Log("Folder " + path + " opened."); 
        }
        if (directories.Length == 0)
            Debug.Log("No directories found.");
    }
}
