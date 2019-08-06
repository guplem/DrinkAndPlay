using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCreation : MonoBehaviour
{
    [MenuItem("Drink and Play/New customized scene")]
    public static void CreateScene()
    {
        //Create the scene
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        //Instantiate the Camera prefab in the scene
        string[] guids2 = AssetDatabase.FindAssets("Camera", new[] { "Assets/General/Prefabs" });
        if (guids2.Length > 1)
            Debug.LogError("There are more than one cameras found inside the path Assets/General/Prefabs");
        string path = AssetDatabase.GUIDToAssetPath(guids2[0]);
        GameObject camPrefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        PrefabUtility.InstantiatePrefab(camPrefab, newScene);


        //Create the SectionManager object
        GameObject sectionManager = new GameObject("SectionManager");

        //Create the canvas object
        GameObject canvasGo = new GameObject("Canvas");
        canvasGo.transform.parent = sectionManager.transform;
        canvasGo.AddComponent<Canvas>();
        canvasGo.AddComponent<CanvasScaler>();
        canvasGo.AddComponent<GraphicRaycaster>();

        Debug.Log("Scene created");

        //Configure the canvas
        CanvasController.AdjustCanvas();
    }

    
}
