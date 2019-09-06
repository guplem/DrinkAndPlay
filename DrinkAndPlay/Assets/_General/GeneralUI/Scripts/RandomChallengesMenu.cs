using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomChallengesMenu : MonoBehaviour
{
    [SerializeField] private Toggle rndChallengesToggle;
    
    private void Start()
    {
        rndChallengesToggle.isOn = GameManager.instance.dataManager.randomChallenges;
    }

    public void SetRandomChallengeTo(bool state)
    {
        GameManager.instance.dataManager.randomChallenges = state;
    }
}
