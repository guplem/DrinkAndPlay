using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class HeightAsMinHeight : MonoBehaviour
{
    private void Start()
    {
        GetComponent<LayoutElement>().minHeight = GetComponent<RectTransform>().sizeDelta.y;
        GetComponent<Canvas>().sortingOrder += transform.GetSiblingIndex();// Debug.Log();
        Debug.Log("si: " + transform.GetSiblingIndex());
    }

}
