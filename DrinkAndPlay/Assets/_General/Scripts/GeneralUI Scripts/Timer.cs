﻿using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    
    [SerializeField] private Button restartButton;
    [SerializeField] private Image playPauseImage;
    [SerializeField] private Button playPauseButton;
    [SerializeField] private TMP_Text remainingTimeText;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;
    private int originalTime;
    
    private int remainingTime
    {
        get { return _remainingTime; }
        set
        {
            _remainingTime = value;
            remainingTimeText.text = remainingTime.ToString();

            restartButton.interactable = remainingTime != originalTime;
            playPauseButton.interactable = remainingTime > 0;
        }
    }
    private int _remainingTime;

    private IEnumerator TimerCorroutine;
    private bool isTimerRunning
    {
        get { return _isTimerRunning; }
        set
        {
            if (isTimerRunning && !value)
            {
                _isTimerRunning = false;
                if (TimerCorroutine != null)
                    StopCoroutine(TimerCorroutine);
                
            }
            else if (!isTimerRunning && value)
            {
                _isTimerRunning = true;
                if (TimerCorroutine == null)
                    TimerCorroutine = DecreaseOneSecond();
                StartCoroutine(TimerCorroutine);
            }
            else if (remainingTime <= 0 && !value)
            {
                _isTimerRunning = false;
            }
            else
            {
                _isTimerRunning = value;
            }
            
            if (isTimerRunning)
                playPauseImage.sprite = pauseSprite;
            else
                playPauseImage.sprite = playSprite;
        }
    }
    private bool _isTimerRunning = false;

    public void PlayPause()
    {
        isTimerRunning = !isTimerRunning;
    }

    public void Pause()
    {
        isTimerRunning = false;
    }

    public void Play()
    {
        isTimerRunning = true;
    }

    public void SetTimerReadyFor(int seconds)
    {
        SetTimerEnabled(seconds > 0);
        
        originalTime = seconds;
        remainingTime = originalTime;
    }

    private void SetTimerEnabled(bool active)
    {
        foreach(Transform child in transform)
        {
            SetChildsActiveRecursively(active, child);
        }
    }

    private void SetChildsActiveRecursively(bool active, Transform transformOfChild)
    {
        foreach(Transform child in transformOfChild)
        {
            SetChildsActiveRecursively(active, child);
        }
        gameObject.SetActive(active);
    }

    public void ResetTimer()
    {
        Pause();
        SetTimerReadyFor(originalTime);
    }
    
    public void SetTimerReadyFor(TMP_Text tmp)
    {
        Pause();
        int time = -1;
        int.TryParse(tmp.text, out time);
        SetTimerReadyFor(time);
    }

    private IEnumerator DecreaseOneSecond()
    {
        while (isTimerRunning)
        {
            if (remainingTime <= 0)
                isTimerRunning = false;
            else
                remainingTime--;
            yield return new WaitForSeconds(1f);
        }
    }
}
