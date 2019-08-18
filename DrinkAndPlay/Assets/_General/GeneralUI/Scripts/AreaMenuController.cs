using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMenuController : MonoBehaviour
{
    [SerializeField] private GameObject topBar;

    private void Start()
    {
        GetComponent<RectTransform>().anchorMax = new Vector2(1, topBar.GetComponent<RectTransform>().anchorMin.y);
    }
}
