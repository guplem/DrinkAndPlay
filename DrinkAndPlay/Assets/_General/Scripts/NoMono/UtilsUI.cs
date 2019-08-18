using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsUI : MonoBehaviour
{
    public static void DestroyContentsOf(Transform parentToClean, List<Transform> exceptions)
    {
        while (parentToClean.childCount > exceptions.Count)
            foreach (Transform child in parentToClean)
                if (!exceptions.Contains(child))
                    DestroyImmediate(child.gameObject);

        //Check that the elimination is ok
        foreach (Transform child in parentToClean)
            if (!exceptions.Contains(child))
            {
                Debug.LogError("Error destroying the contents of " + parentToClean.name + ". Maybe some of the exceptions is no inside the parentGameObject. The exceptions were the following ones:");
                foreach (Transform exception in exceptions)
                    Debug.LogError("  --> " + exception.name);
            }
    }
}
