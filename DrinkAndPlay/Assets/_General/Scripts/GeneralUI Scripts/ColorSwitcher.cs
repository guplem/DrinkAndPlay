using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class ColorSwitcher : MonoBehaviour
{
    [SerializeField] private LightDarkColor lightDarkColor;

    private bool subscribed = false;

    private void Start()
    {
        if (!subscribed)
            OnEnable();
        
        if (lightDarkColor == null)
            Debug.LogError("No 'LightDarkColor' set in " + gameObject.name, gameObject);
    }

    private void OnEnable()
    {
        if (GameManager.instance == null)
            return;
        
        if (!subscribed)
        {
            GameManager.instance.dataManager.changedVisualMode += SetColorTo;
            subscribed = true;
        }
        SetColorTo(GameManager.instance.dataManager.GetVisualMode());
    }
    
    private void OnDisable()
    {
        if (subscribed)
        {
            GameManager.instance.dataManager.changedVisualMode += SetColorTo;
            subscribed = false;
        }
    }


    public void SetColorTo(LightDarkColor.ColorType colorType)
    {
        Image img = GetComponent<Image>();
        if (img != null)
        {
            img.color = GetColor(colorType);
            return;
        }

        TextMeshProUGUI txt = GetComponent<TextMeshProUGUI>();
        if (txt != null)
        {
            txt.color = GetColor(colorType);
            return;
        }
        
        //TODO: Buttons
        
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            cam.backgroundColor = GetColor(colorType);
            return;
        }
            



    }

    public Color GetColor(LightDarkColor.ColorType colorType)
    {
        switch (colorType)
        {
            case LightDarkColor.ColorType.Light:
                return lightDarkColor.lightColor;
            case LightDarkColor.ColorType.Dark:
                return lightDarkColor.darkColor;
            default:
                return Color.magenta;
        }
    }
}
