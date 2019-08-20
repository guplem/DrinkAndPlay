using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    
    public static void SetAnchorsAroundObject(GameObject objToModify)
    {
        GameObject o = objToModify;
        
        if (o == null || o.GetComponent<RectTransform>() == null) 
            return;
        
        RectTransform r = o.GetComponent<RectTransform>();
        RectTransform p = o.transform.parent.GetComponent<RectTransform>();

        if (o.GetComponent<Canvas>() != null)
        {
            Debug.LogError("The object to work with should not be the canvas.");
            return;
        }
           
        Vector2 offsetMin = r.offsetMin;
        Vector2 offsetMax = r.offsetMax;
        Vector2 _anchorMin = r.anchorMin;
        Vector2 _anchorMax = r.anchorMax;
           
        float parent_width = p.rect.width;      
        float parent_height = p.rect.height;  
           
        Vector2 anchorMin = new Vector2(_anchorMin.x + (offsetMin.x / parent_width),
            _anchorMin.y + (offsetMin.y / parent_height));
        Vector2 anchorMax = new Vector2(_anchorMax.x + (offsetMax.x / parent_width),
            _anchorMax.y + (offsetMax.y / parent_height));
           
        r.anchorMin = anchorMin;
        r.anchorMax = anchorMax;
           
        r.offsetMin = new Vector2(0f, 0f);
        r.offsetMax = new Vector2(0f, 0f);
        r.pivot = new Vector2(0.5f, 0.5f);
    }
    
}
