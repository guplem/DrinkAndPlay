using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
public class ForcedDynamicAspectRatio : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Start()
    {
        AspectRatioFitter ar = GetComponent<AspectRatioFitter>();
        Sprite sprite = image.sprite;
        ar.aspectRatio = sprite.rect.width / sprite.rect.height;
    }


}
