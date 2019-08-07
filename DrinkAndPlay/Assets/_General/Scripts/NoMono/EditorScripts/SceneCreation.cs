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
        //Avoid discarting changes without intention
        if (SceneManager.GetActiveScene().isDirty)
        {
            Debug.LogWarning("The current scene is not saved. Save or discard the changes before creating a new one");
            return;
        }

        //Create the scene
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        //Instantiate the Camera prefab in the scene
        string[] guids2 = AssetDatabase.FindAssets("Camera", new[] { "Assets/_General/Prefabs" });
        if (guids2.Length > 1)
            Debug.LogError("There are more than one 'Camera' found inside the path 'Assets/General/Prefabs'");
        string path = AssetDatabase.GUIDToAssetPath(guids2[0]);
        GameObject prefabLoaded = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        PrefabUtility.InstantiatePrefab(prefabLoaded, newScene);

        /*
        //Create the SectionManager object
        GameObject sectionManager = new GameObject("SectionManager");

        //Create the canvas object
        GameObject canvasGo = new GameObject("Canvas");
        canvasGo.transform.parent = sectionManager.transform;
        canvasGo.AddComponent<Canvas>();
        canvasGo.AddComponent<CanvasScaler>();
        canvasGo.AddComponent<GraphicRaycaster>();
        */

        

        guids2 = AssetDatabase.FindAssets("Canvas - For Section", new[] { "Assets/_General/Prefabs" });
        if (guids2.Length > 1)
            Debug.LogError("There are more than one 'Canvas - For Section' found inside the path 'Assets/General/Prefabs'");
        path = AssetDatabase.GUIDToAssetPath(guids2[0]);
        prefabLoaded = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        Instantiate(prefabLoaded).name = prefabLoaded.name;

        Debug.Log("Scene created");

        //Configure the canvas
        CanvasController.AdjustCanvas();
    }

    
}
