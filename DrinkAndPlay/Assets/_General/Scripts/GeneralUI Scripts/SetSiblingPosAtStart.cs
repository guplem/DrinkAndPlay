using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSiblingPosAtStart : MonoBehaviour
{
    [SerializeField] private int siblingIndex = -1;
    void Start()
    {
        if (siblingIndex >= 0)
            transform.SetSiblingIndex(siblingIndex);
    }
    
}
