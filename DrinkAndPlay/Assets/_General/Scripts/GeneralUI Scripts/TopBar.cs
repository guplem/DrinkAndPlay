using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
[RequireComponent(typeof(Canvas))]
public class TopBar : MonoBehaviour
{
    public static int lastTopBarLayer;
    [SerializeField] private Transform menuOfTopBar;
    private void Start()
    {
        GetComponent<LayoutElement>().minHeight = GetComponent<RectTransform>().sizeDelta.y;
        
        if (menuOfTopBar != null)
            GetComponent<Canvas>().sortingOrder += menuOfTopBar.GetSiblingIndex();
        
        //else
        //    GetComponent<Canvas>().sortingOrder += transform.GetSiblingIndex();
    }
}
