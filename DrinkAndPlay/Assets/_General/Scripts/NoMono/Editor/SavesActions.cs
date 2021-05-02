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

    // I do not know why it does not work
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
    
    [MenuItem("Drink and Play/Saves/Print 'Application persistent data path'")]
    public static void PrintAppDataPath()
    {
        Debug.Log(Application.persistentDataPath);
    }
    
    [MenuItem("Drink and Play/Saves/Open 'Application persistent data path'")]
    public static void OpenAppDataPath()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }
}
