using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
#pragma warning disable 0649
public class EnableScrollRectAtPlay : MonoBehaviour
{
    [SerializeField] private bool horizontalScroll;
    [SerializeField] private bool verticalScroll;

    void Start()
    {
        ScrollRect sr = GetComponent<ScrollRect>();
        sr.horizontal = horizontalScroll;
        sr.vertical = verticalScroll;
    }

}
