using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MixModeMenu : MonoBehaviour
{
    [SerializeField] private LocalizationFile[] modesToIncludeInMenu;
    [SerializeField] private GameObject modeMixModePrefab;
    [SerializeField] private Transform contentsObject;
    [SerializeField] private Transform[] elementsNotInModeList;
    private bool[] modeStates;
    private bool alreadyBuilt = false;
    
    public void BuildModesList()
    {
        if (alreadyBuilt) return;    
        
        modeStates = new bool[modesToIncludeInMenu.Length];
        for (int modeState = 0; modeState < modeStates.Length; modeState++)
            modeStates[modeState] = true;
        
        
        UtilsUI.DestroyContentsOf(contentsObject, elementsNotInModeList.ToList());

        for (int m = 0; m < modesToIncludeInMenu.Length; m++)
        {
            GameObject modeGo = Instantiate(modeMixModePrefab, contentsObject);
            modeGo.transform.SetSiblingIndex(m+5);
            modeGo.GetComponent<ModeMixModePrefab>().Setup(this, modesToIncludeInMenu[m], modeStates[m]);
        }

        alreadyBuilt = true;
    }

    public void SetState(LocalizationFile localizationFile, bool newState)
    {
        for (int m = 0; m < modesToIncludeInMenu.Length; m++)
        {
            if (localizationFile.Equals(modesToIncludeInMenu[m]))
                modeStates[m] = newState;
            return;
        }
        
        Debug.LogError("LoacalizationFile " + localizationFile + " not found in the MixModeMenu list called 'modesToIncludeInMenu'", gameObject);
    }
}
