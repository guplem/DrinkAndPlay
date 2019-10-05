using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualsMenu : MonoBehaviour
{
    [SerializeField] private Toggle darkModeToggle;
    private bool setUpDone = false; 
    
    private void Start()
    {
        if (!setUpDone)
        {
            darkModeToggle.isOn = GameManager.instance.dataManager.darkMode;
            setUpDone = true;
        }

    }

    public void SetDarkModeTo(bool state)
    {
        if (setUpDone)
            GameManager.instance.dataManager.darkMode = state;
    }
}
