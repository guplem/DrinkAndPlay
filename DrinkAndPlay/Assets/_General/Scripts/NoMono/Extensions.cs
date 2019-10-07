using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class Extensions
{
    public static List<T> CloneToList<T>(this IEnumerable<T> listToClone) where T: ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }
    
    public static void DebugLog<T>(this IEnumerable<T> listToDebug, string message) where T: IEnumerable
    {
        Debug.Log(message + string.Join(", ",
                      new List<T>(listToDebug)
                          .ConvertAll(i => i.ToString())
                          .ToArray()));
    }
}

public static class RectTransformExtensions
{
    // Setters
    
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }
 
    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }
 
    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }
 
    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
    
    // Getters

    public static float GetHeightAnchors(this RectTransform rt)
    {
        return rt.sizeDelta.y;
    }
    
    public static float GetWidthAnchors(this RectTransform rt)
    {
        return rt.sizeDelta.x;
    }
    
    public static float GetHeightTransform(this RectTransform rt)
    {
        return rt.rect.y;
    }
    
    public static float GetWidthTransform(this RectTransform rt)
    {
        return rt.rect.x;
    }

}