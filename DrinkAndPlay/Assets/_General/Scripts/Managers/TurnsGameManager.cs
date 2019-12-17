
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TextInTurnsGame
{
    public readonly LocalizedText localizedText;
    public bool liked;
    public readonly LocalizationFile localizationFile;

    public TextInTurnsGame(LocalizedText localizedText, LocalizationFile localizationFile)
    {
        this.localizedText = localizedText;
        this.liked = false; // TODO: History of likes? (TBD)
        this.localizationFile = localizationFile;
    }
}

public abstract class TurnsGameManager : SectionManager
{
    
    [SerializeField] private Localizer sentenceText;
    [SerializeField] protected ImageSwitcher likeButton;
    [SerializeField] private Button backButton;
    
    
    #region Texts
    
    private List<TextInTurnsGame> history = new List<TextInTurnsGame>();
    private int historyIndex = -1;

    private int minDelayForRandomChallenge = 8;
    private int maxDelayForRandomChallenge = 16;
    private int currentDelayForRandomChallenge = 0;
    
    
    private int minLikesToRatePopup = 4;
    private float minPercentageToRatePopup = 15f;
    public TextInTurnsGame currentTextInCard { get; private set; }

    public abstract void NextButton();
    public abstract void PreviousButton();
    
    protected TextInTurnsGame GetNextText(bool checkIfSelected)
    {
        if (!checkIfSelected)
            return GetNextText(section.localizationFiles[Random.Range(0, section.localizationFiles.Length)]);
        
        return GetNextText(gm.dataManager.GetRandomSelectedLocalizationFiles());
    }
    
    protected TextInTurnsGame GetNextText(LocalizationFile localizationFile)
    {
        if (localizationFile == null)
        {
            Debug.LogError("Trying to get a text from a 'null' localization file. - TurnsManager", gameObject);
            return null;
        }
        
        historyIndex++;
        if (historyIndex == history.Count) //We are "generating" new sentences, not going back or forward
        {
            LocalizedText lt = GameManager.instance.localizationManager.SearchLocalizedText(localizationFile, true, true, true, true);
            RegisterNewTextInHistory(lt, localizationFile);
            ProcessRandomChallenge();
        }
        
        return GetCurrentText();
    }

    public bool AreWeOnTopHistory() //The newest sentence
    {
        return historyIndex == history.Count - 1;
    }
    
    public bool AreWeOnBottomHistory() //The oldest sentence
    {
        return historyIndex == 0;
    }

    private void RegisterNewTextInHistory(LocalizedText localizedText, LocalizationFile localizationFile)
    {
        history.Add(new TextInTurnsGame(localizedText, localizationFile));
        
        if (historyIndex == history.Count)
            historyIndex++;
    }

    private void ProcessRandomChallenge()
    {
        currentDelayForRandomChallenge++;
        if (currentDelayForRandomChallenge < minDelayForRandomChallenge) return;
        float probability = (currentDelayForRandomChallenge - minDelayForRandomChallenge) / (maxDelayForRandomChallenge - minDelayForRandomChallenge) * 100;
        if (probability >= Random.Range(0.0f, 100.0f))
        {
            GameManager.instance.generalUi.ShowRandomChallenge();
            currentDelayForRandomChallenge = 0;
        }
    }

    
    protected string GetPreviousTextId()
    {
        if (history.Count <= 0)
            return null;
        
        historyIndex--;
        
        if (historyIndex < 0)
            historyIndex = 0;

        return GetCurrentTextId();
    }
    
    protected TextInTurnsGame GetPreviousText()
    {
        if (history.Count <= 0)
            return null;
        
        historyIndex--;
        
        if (historyIndex < 0)
            historyIndex = 0;

        return GetCurrentText();
    }

    protected string GetCurrentTextId()
    {
        if (historyIndex < 0)
            return null;
        
        return history[historyIndex].localizedText.id;
    }
    
    protected TextInTurnsGame GetCurrentText()
    {
        if (historyIndex < 0)
            return null;

        if (historyIndex >= history.Count)
        {
            Debug.LogError("Trying to get the text with number " + historyIndex + " in historey but there are only " + history.Count + " elements on the list (" + (history.Count-1) + " is the top index)");
            return null;
        }
        
        return history[historyIndex];
    }
    
    protected void SetupTextInCard(TextInTurnsGame textInCard)
    {
        if (textInCard == null)
        {
            Debug.LogError("The obtained text for the card is null.", gameObject);    
            return;
        } 
        else if (textInCard.localizedText == null)
        {
            Debug.LogError("The text for the card does not have a localized text.", gameObject);    
            return;
        }
        
        Debug.Log("Setting text in card: " + textInCard.localizedText +  ".\nFrom the localization file: '" + textInCard.localizationFile + "' after a search for a text with NaughtyLevel = " + GameManager.instance.dataManager.naughtyLevel);
        
        sentenceText.Localize(textInCard.localizedText.id, textInCard.localizationFile);

        likeButton.SetToInitialState();
        
        if (IsCurrentTextLiked())
            likeButton.Switch();

        currentTextInCard = textInCard;

        SetBackButtonAvaliability();
    }

    private void SetBackButtonAvaliability()
    {
        if (backButton == null) return;

        backButton.interactable = !AreWeOnBottomHistory();
    }

    #endregion Texts


    #region GameFunctions

    public void Like()
    {
        history[historyIndex].liked = !history[historyIndex].liked;
        likeButton.Switch();

        // Check if the rate popup should be shown
        
        if (history.Count < minLikesToRatePopup)    
            return;

        int likeCount = 0;
        foreach (TextInTurnsGame txt in history)
            if (txt.liked)
                likeCount++;
        
        if (likeCount < minLikesToRatePopup)
            return;

        float percentage = likeCount * 100f / history.Count;
        if (percentage < minPercentageToRatePopup)
            return;

        if (gm.dataManager.ratePopupShown || gm.dataManager.ratedApp)
            return;
        
        gm.generalUi.ShowRatePopup();
    }

    public bool IsCurrentTextLiked()
    {
        return history[historyIndex].liked;
    }

    public void AddSentence()
    {
        GameManager.instance.generalUi.OpenFeedbackMenuCurrentSection();
    }

    public void Share()
    {
        gm.generalUi.Share();
        Debug.Log("Sharing.");
        
    }

    #endregion

    public void ErrorButton()
    {
        gm.generalUi.OpenErrorMenu(currentTextInCard);
    }
}
