using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
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
            #if UNITY_EDITOR
                Undo.RecordObject(img, "Changing the component 'Image' on " + gameObject.name);
            #endif
            
            img.color = GetColor(colorType);
            AfterColorSwitchCallMethod(colorType);
            return;
        }

        TextMeshProUGUI txt = GetComponent<TextMeshProUGUI>();
        if (txt != null)
        {
            #if UNITY_EDITOR
                Undo.RecordObject(txt, "Changing the component 'TextMeshProUGUI' on " + gameObject.name);
            #endif
            
            txt.color = GetColor(colorType);
            AfterColorSwitchCallMethod(colorType);
            return;
        }
        
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            #if UNITY_EDITOR
                Undo.RecordObject(cam, "Changing the component 'Camera' on " + gameObject.name);
            #endif
            
            cam.backgroundColor = GetColor(colorType);
            AfterColorSwitchCallMethod(colorType);
            return;
        }

        
    }

    private void AfterColorSwitchCallMethod(LightDarkColor.ColorType colorType)
    {
        if (afterColorSwitchCall != null)
            afterColorSwitchCall(colorType);
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
    
    public Action<LightDarkColor.ColorType> afterColorSwitchCall;
}
