using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Localizer : MonoBehaviour
{

    [SerializeField] public string id;
    [SerializeField] public bool warnIfNullId = true;
    [SerializeField] public bool automaticallyLocalizeOnEnable = true;
    [SerializeField] public bool registerTimestampAtLocalize;
    [SerializeField] public bool isUI;

    private string currentLanguage = "";
    private bool started = false;
    private TextMeshProUGUI TMProGUI;


    private void Start()
    {
        TMProGUI = GetComponent<TextMeshProUGUI>();
        started = true;
        OnEnable();
    }


    private void OnEnable()
    {
        if (started)
        {
            LocalizationManager.OnLocalizeAllAction += Localize;

            if (automaticallyLocalizeOnEnable)
                if (currentLanguage != GameManager.Instance.configurationManager.language)
                    Localize();
        }
    }

    private void OnDisable()
    {
        if (started)
            LocalizationManager.OnLocalizeAllAction -= Localize;
    }

    public void ForceLocalize(string id)
    {
        this.id = id;
        Localize();
    }


    public void Localize()
    {

        if ((string.IsNullOrEmpty(id)) && (warnIfNullId))
        {
            Debug.LogWarning("Trying to localize the object '" + gameObject.name + "' but the 'id' in the 'Localizer' component is null or empty (may be intended)");
            return;
        }

        Section section = isUI ? GameManager.Instance.uiSection : SectionManager.Instance.section;

        TMProGUI.text = GameManager.Instance.localizationManager.GetLocalizedText(section, id, registerTimestampAtLocalize).text;
    }

}