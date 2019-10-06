using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public partial class ColorSwitcher : MonoBehaviour
{
    [SerializeField] public LightDarkColor lightDarkColor;

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

        UpdateColor();
    }
    
    private void OnDisable()
    {
        if (subscribed)
        {
            GameManager.instance.dataManager.changedVisualMode -= SetColorTo;
            subscribed = false;
        }
    }


    public void SetColorTo(LightDarkColor.ColorType colorType)
    {
        Image img = GetComponent<Image>();
        if (img != null)
        {
            Undo.RecordObject(img, "Changing the component 'Image' on " + gameObject.name);
            
            img.color = GetColor(colorType);
            return;
        }

        TextMeshProUGUI txt = GetComponent<TextMeshProUGUI>();
        if (txt != null)
        {
            Undo.RecordObject(txt, "Changing the component 'TextMeshProUGUI' on " + gameObject.name);

            txt.color = GetColor(colorType);
            return;
        }
        
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            Undo.RecordObject(cam, "Changing the component 'Camera' on " + gameObject.name);

            cam.backgroundColor = GetColor(colorType);
            return;
        }

    }

    public void UpdateColor()
    {
        SetColorTo(GameManager.instance.dataManager.GetVisualMode());
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
