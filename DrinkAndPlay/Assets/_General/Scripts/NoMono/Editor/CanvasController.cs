using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController
{

    [MenuItem("Drink and Play/Adjust in-scene canvases")]
    public static void AdjustCanvas()
    {

        Canvas[] canvasCol = Object.FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvasCol)
            AdjustCanvasComponent(canvas);
        if (canvasCol.Length <= 0)
            Debug.LogError("The 'Canvas' component was not found in the scene.");

        CanvasScaler[] canvasScalerCol = Object.FindObjectsOfType<CanvasScaler>();
        foreach (CanvasScaler canvasScaler in canvasScalerCol)
            AdjustCanvasScaler(canvasScaler);
        if (canvasScalerCol.Length <= 0)
            Debug.LogError("The 'CanvasScaler' component was not found in the scene.");

        var graphicRaycasterCol = Object.FindObjectsOfType<GraphicRaycaster>();
        foreach (GraphicRaycaster graphicRaycaster in graphicRaycasterCol)
            AdjustGraphicRaycaster(graphicRaycaster);
        if (graphicRaycasterCol.Length <= 0)
            Debug.LogError("The 'GraphicRaycaster' component was not found in the scene.");

        Debug.Log("Canvas configuration applied to the found canvas components.");
    }


    private static void AdjustCanvasComponent(Canvas canvasComponent) {
        if (canvasComponent == null)
        {
            Debug.LogError("The component 'Canvas' inside the 'Canvas' GameObject was not found.");
            return;
        }

        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasComponent.pixelPerfect = false;
        //canvasComponent.sortingOrder = (canvasComponent.GetComponent<GameManager>() != null)? 50 : 0;
        canvasComponent.targetDisplay = 0;
        canvasComponent.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
    }

    private static void AdjustCanvasScaler(CanvasScaler canvasScaler)
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

    private static void AdjustGraphicRaycaster(GraphicRaycaster graphicRaycaster)
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
