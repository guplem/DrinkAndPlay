using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Localizer : MonoBehaviour
{

    [SerializeField] public string id;
    [SerializeField] public bool isUI;
    [SerializeField] public bool automaticallyLocalize = false;
    [SerializeField] public bool registerTimestampAtLocalize;

    private string currentLanguage = "";
    private bool previouslyStarted;
    private bool subscribed = false;
    private TextMeshProUGUI TMProGUI;

    private void OnEnable()
    {
        if (TMProGUI == null)
        {
            TMProGUI = GetComponent<TextMeshProUGUI>();

            if (TMProGUI == null)
                Debug.LogError("The 'TextMeshProUGUI' component could not be found in the object " + name, gameObject);
        }

        if (!subscribed)
        {
            LocalizationManager.OnLocalizeAllAction += Localize;
            subscribed = true;
        }

        if (previouslyStarted)
            Start();

    }

    private void OnDisable()
    {
        if (subscribed)
        {
            LocalizationManager.OnLocalizeAllAction -= Localize;
            subscribed = false;
        }
    }

    private void Start()
    {
        previouslyStarted = true;

        if (automaticallyLocalize)
        {
            if (currentLanguage != GameManager.Instance.dataManager.language)
                Localize();
        }
    }


    public void Localize(string id)
    {
        this.id = id;
        Localize();
    }


    public void Localize()
    {

        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("Trying to localize the object '" + gameObject.name + "' but the 'id' in the 'Localizer' component is null or empty");
            return;
        }

        Section section = isUI ? GameManager.Instance.uiSection : SectionManager.Instance.section;

        if (TMProGUI == null)
        {
            Debug.LogWarning("TMProGUI is null in " + name, gameObject);

            /*
            TMProGUI = GetComponent<TextMeshProUGUI>();

            if (TMProGUI == null)
                Debug.LogWarning("2nd FAIL in " + name, gameObject);
            else
                Debug.Log("Hot fix in " + name, gameObject);
            */
        }

        currentLanguage = GameManager.Instance.dataManager.language;

        TMProGUI.text = GameManager.Instance.localizationManager.GetLocalizedText(section, id, registerTimestampAtLocalize).text;
    }

}