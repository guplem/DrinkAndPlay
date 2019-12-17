using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationFilesSelectorMenu : MonoBehaviour
{
    //[SerializeField] private LocalizationFile[] modesToIncludeInMenu;
    [SerializeField] private GameObject modeMixModePrefab;
    [SerializeField] private Transform contentsObject;
    [SerializeField] private Transform[] elementsNotInModeList;
    private bool[] modeStates;
    [SerializeField] private Button playButton;
    private Section currentSection;
    
    public void BuildModesListFor(Section section)
    {
        if (currentSection == section) return;
        
        modeStates = new bool[section.localizationFiles.Length];
        for (int modeState = 0; modeState < modeStates.Length; modeState++)
            modeStates[modeState] = true;
        
        
        UtilsUI.DestroyContentsOf(contentsObject, elementsNotInModeList.ToList());

        for (int m = 0; m < section.localizationFiles.Length; m++)
        {
            GameObject modeGo = Instantiate(modeMixModePrefab, contentsObject);
            modeGo.transform.SetSiblingIndex(m+2);
            modeGo.GetComponent<LocalizationFileToggle>().Setup(this, section.localizationFiles[m], modeStates[m]);
        }

        currentSection = section;
    }

    public void SetState(LocalizationFile localizationFile, bool newState)
    {
        for (int m = 0; m < currentSection.localizationFiles.Length; m++)
        {
            if (localizationFile.Equals(currentSection.localizationFiles[m]))
            {
                modeStates[m] = newState;
                GameManager.instance.dataManager.SetSelectedLocalizationFiles(currentSection.localizationFiles, modeStates);
                playButton.interactable = GameManager.instance.dataManager.GetSelectedLocalizationFilesQuantity()>0;
                return; 
            }

        }
        
        Debug.LogError("LoacalizationFile " + localizationFile + " not found in the MixModeMenu list called 'modesToIncludeInMenu'", gameObject);
    }
    
    public void PlaySelectedSection()
    {
        playButton.GetComponent<ButtonAnimation>().MidAnimEvent += LoadSelectedSectionAtEvent;
    }
    
    private void LoadSelectedSectionAtEvent()
    {
        GameManager.instance.PlaySection(GameManager.instance.dataManager.lastSelectedSection);
        GameManager.instance.generalUi.CloseLastOpenUiElement();
        playButton.GetComponent<ButtonAnimation>().MidAnimEvent -= LoadSelectedSectionAtEvent;
    }
}
