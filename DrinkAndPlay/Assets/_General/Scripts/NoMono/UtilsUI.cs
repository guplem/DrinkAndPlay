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

    public static void SetAnchors(RectTransform rt, Vector2 min, Vector2 max, bool keepSize)
    {
        Vector2 size = Vector2.zero;
        if (keepSize)
        {
            Rect rect = rt.rect;
            size = new Vector2(rect.width,rect.height);
        }

        rt.anchorMin = min;
        rt.anchorMax = max;

        if (keepSize)
            SetSize(rt, size);
        
    }

    public static void SetSize(RectTransform rt, Vector2 size)
    {
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
    }
}
