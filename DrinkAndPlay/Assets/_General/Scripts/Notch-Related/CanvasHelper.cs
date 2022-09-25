using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//https://forum.unity.com/threads/canvashelper-resizes-a-recttransform-to-iphone-xs-safe-area.521107/
[RequireComponent(typeof(Canvas))]
public class CanvasHelper : MonoBehaviour
{
    private static UnityEvent onOrientationChange = new UnityEvent();
    private static UnityEvent onResolutionChange = new UnityEvent();
    private static bool isLandscape { get; set; }

    private static List<CanvasHelper> helpers = new List<CanvasHelper>();

    private static bool screenChangeVarsInitialized = false;
    private static ScreenOrientation lastOrientation = ScreenOrientation.Portrait;
    private static Vector2 lastResolution = Vector2.zero;
    private static Rect lastSafeArea = Rect.zero;

    private Canvas canvas;
    private RectTransform rectTransform;

    [SerializeField] private RectTransform safeArea;

    private void Awake()
    {
        if (!helpers.Contains(this))
            helpers.Add(this);

        canvas = GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();

        //safeAreaTransform = transform.Find("SafeArea") as RectTransform;

        if (safeArea == null)
            Debug.LogError("SafeArea not referenced at the object " + name, gameObject);
        safeArea.anchorMax = Vector2.one;
        safeArea.anchorMin = Vector2.zero;


        if (!screenChangeVarsInitialized)
        {
            lastOrientation = Screen.orientation;
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;
            lastSafeArea = Screen.safeArea;

            screenChangeVarsInitialized = true;
        }
    }

    private void Start()
    {
        ApplySafeArea();
    }

    private void Update()
    {
        if (helpers[0] != this)
            return;

        if (Application.isMobilePlatform)
        {
            if (Screen.orientation != lastOrientation)
                OrientationChanged();

            if (Screen.safeArea != lastSafeArea)
                SafeAreaChanged();
        }
        else
        {
            //resolution of mobile devices should stay the same always, right?
            // so this check should only happen everywhere else
            if (Screen.width != lastResolution.x || Screen.height != lastResolution.y)
                ResolutionChanged();
        }
    }

    private void ApplySafeArea()
    {
        if (this.safeArea == null)
            return;

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        Rect pixelRect = canvas.pixelRect;
        anchorMin.x /= pixelRect.width;
        anchorMin.y /= pixelRect.height;
        anchorMax.x /= pixelRect.width;
        anchorMax.y /= pixelRect.height;

        this.safeArea.anchorMin = anchorMin;
        this.safeArea.anchorMax = anchorMax;

        // Debug.Log(
        // "ApplySafeArea:" +
        // "\n Screen.orientation: " + Screen.orientation +
        // #if UNITY_IOS
        // "\n Device.generation: " + UnityEngine.iOS.Device.generation.ToString() +
        // #endif
        // "\n Screen.safeArea.position: " + Screen.safeArea.position.ToString() +
        // "\n Screen.safeArea.size: " + Screen.safeArea.size.ToString() +
        // "\n Screen.width / height: (" + Screen.width.ToString() + ", " + Screen.height.ToString() + ")" +
        // "\n canvas.pixelRect.size: " + canvas.pixelRect.size.ToString() +
        // "\n anchorMin: " + anchorMin.ToString() +
        // "\n anchorMax: " + anchorMax.ToString());
    }

    private void OnDestroy()
    {
        if (helpers != null && helpers.Contains(this))
            helpers.Remove(this);
    }

    private static void OrientationChanged()
    {
        //Debug.Log("Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);

        lastOrientation = Screen.orientation;
        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;

        isLandscape = lastOrientation == ScreenOrientation.LandscapeLeft || lastOrientation == ScreenOrientation.LandscapeRight || lastOrientation == ScreenOrientation.LandscapeLeft;
        onOrientationChange.Invoke();

    }

    private static void ResolutionChanged()
    {
        if (lastResolution.x == Screen.width && lastResolution.y == Screen.height)
            return;

        //Debug.Log("Resolution changed from " + lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);

        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;

        isLandscape = Screen.width > Screen.height;
        onResolutionChange.Invoke();
    }

    private static void SafeAreaChanged()
    {
        if (lastSafeArea == Screen.safeArea)
            return;

        //Debug.Log("Safe Area changed from " + lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);

        lastSafeArea = Screen.safeArea;

        for (int i = 0; i < helpers.Count; i++)
        {
            helpers[i].ApplySafeArea();
        }
    }

    private static Vector2 GetCanvasSize()
    {
        return helpers[0].rectTransform.sizeDelta;
    }

    public static Vector2 GetSafeAreaSize()
    {
        for (int i = 0; i < helpers.Count; i++)
        {
            if (helpers[i].safeArea != null)
            {
                return helpers[i].safeArea.sizeDelta;
            }
        }

        return GetCanvasSize();
    }
}