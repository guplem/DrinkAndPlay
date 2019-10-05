using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomChallengesMenu : MonoBehaviour
{
    [SerializeField] private Toggle rndChallengesToggle;
    private bool setUpDone = false; 
    
    private void Start()
    {
        rndChallengesToggle.isOn = GameManager.instance.dataManager.randomChallenges;
        setUpDone = true;
    }

    public void SetRandomChallengeTo(bool state)
    {
        if (setUpDone)
            GameManager.instance.dataManager.randomChallenges = state;
    }
}
