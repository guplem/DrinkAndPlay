using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController
{

    [MenuItem("Drink and Play/Adjust canvas")]
    public static void AdjustCanvas()
    {

        var canvasCol = Object.FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvasCol)
            AdjustCanvasComponent(canvas);
        if (canvasCol.Length <= 0)
            Debug.LogError("The 'Canvas' component was not found in the scene.");
        //if (canvasCol.Length > 1)
        //    Debug.LogWarning("More than one 'Canvas' component was found in the scene. There are multiple 'Canvas' objects in the scene? Shouln't be.");

        var canvasScalerCol = Object.FindObjectsOfType<CanvasScaler>();
        foreach (CanvasScaler canvasScaler in canvasScalerCol)
            AdjustCanvasScaler(canvasScaler);
        if (canvasScalerCol.Length <= 0)
            Debug.LogError("The 'CanvasScaler' component was not found in the scene.");
        //if (canvasScalerCol.Length > 1)
        //    Debug.LogWarning("More than one 'CanvasScaler' component was found in the scene. There are multiple 'Canvas' objects in the scene? Shouln't be.");


        var graphicRaycasterCol = Object.FindObjectsOfType<GraphicRaycaster>();
        foreach (GraphicRaycaster graphicRaycaster in graphicRaycasterCol)
            AdjustGraphicRaycaster(graphicRaycaster);
        if (graphicRaycasterCol.Length <= 0)
            Debug.LogError("The 'GraphicRaycaster' component was not found in the scene.");
        //if (graphicRaycasterCol.Length > 1)
        //    Debug.LogWarning("More than one 'GraphicRaycaster' component was found in the scene. There are multiple 'Canvas' objects in the scene? Shouln't be.");


        Debug.Log("Canvas configuration applied to the found canvas components.");
    }


    public static void AdjustCanvasComponent(Canvas canvasComponent) {
        if (canvasComponent == null)
        {
            Debug.LogError("The component 'Canvas' inside the 'Canvas' GameObject was not found.");
            return;
        }

        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasComponent.pixelPerfect = false;
        canvasComponent.sortingOrder = (canvasComponent.GetComponent<GameManager>() != null)? 50 : 0;
        canvasComponent.targetDisplay = 0;
        canvasComponent.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;


    }

    public static void AdjustCanvasScaler(CanvasScaler canvasScaler)
    {
        if (canvasScaler == null)
        {
            Debug.LogError("The component 'CanvasScaler' inside the 'Canvas' GameObject was not found.");
            return;
        }

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080f, 1920f);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 0.5f;
        canvasScaler.referencePixelsPerUnit = 100f;

    }

    public static void AdjustGraphicRaycaster(GraphicRaycaster graphicRaycaster)
    {
        if (graphicRaycaster == null)
        {
            Debug.LogError("The component 'GraphicRaycaster' inside the 'Canvas' GameObject was not found.");
            return;
        }

        graphicRaycaster.ignoreReversedGraphics = true;
        graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
        //graphicRaycaster.blockingMask = GraphicRaycaster.BlockingMask.All; // Not accessible via script

    }


        
}
