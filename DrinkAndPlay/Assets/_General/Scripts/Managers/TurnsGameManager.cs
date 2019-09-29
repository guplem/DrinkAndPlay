
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInTurnsGame
{
    public readonly string localizedTextId;
    public bool liked;
    public readonly LocalizationFile localizationFile;

    public TextInTurnsGame(LocalizedText localizedText, LocalizationFile localizationFile)
    {
        this.localizedTextId = localizedText.id;
        this.liked = false;
        this.localizationFile = localizationFile;
    }

    public TextInTurnsGame(LocalizedText localizedText, bool liked)
    {
        this.localizedTextId = localizedText.id;
        this.liked = liked;
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

    private int minDelayForRandomChallenge = 10;
    private int maxDelayForRandomChallenge = 25;
    private int currentDelayForRandomChallenge = 0;
    
    
    private int minLikesToRatePopup = 5;
    private float minPercentageToRatePopup = 30f;

    public abstract void NextButton();
    public abstract void PreviousButton();
    
    protected TextInTurnsGame GetNextText()
    {
        return GetNextText(section.localizationFiles[0]);
    }
    
    protected TextInTurnsGame GetNextText(LocalizationFile localizationFile)
    {
        historyIndex++;
        if (historyIndex == history.Count) //We are "generating" new turns, not going back or forward
        {
            LocalizedText lt = GetRandomText(true, true, localizationFile);
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
    
    private LocalizedText GetRandomText(bool register, bool checkNotRegistered)
    {
        return GetRandomText(register, checkNotRegistered, section.localizationFiles[0]);
    }

    private LocalizedText GetRandomText(bool register, bool checkNotRegistered, LocalizationFile localizationFile)
    {
        while (true)
        {
            LocalizedText lt = GameManager.instance.localizationManager.GetLocalizedText(localizationFile, register, checkNotRegistered);
            if (lt == null)
            {
                Debug.Log("Localized text not found");

                if (GameManager.instance.dataManager.GetTextRegisteredQuantity(section) > 2) // To know if there are enough to remove the register of the 50%
                {
                    GameManager.instance.dataManager.RemoveOldestPercentageOfTextsRegistered(localizationFile, 25f);
                    Debug.Log("REMOVED 25%");
                    GameManager.instance.localizationManager.RandomizeLocalizedTexts(localizationFile); // To change the order of the new avaliable texts
                }
                else
                {
                    Debug.LogWarning("There are not enough texts for the section " + section);
                    return null;
                }
            }
            else
            {
                return lt;
            }
        }
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
        
        return history[historyIndex].localizedTextId;
    }
    
    protected TextInTurnsGame GetCurrentText()
    {
        if (historyIndex < 0)
            return null;
        
        return history[historyIndex];
    }
    
    protected void SetupTextInCard(TextInTurnsGame getPreviousText)
    {
        if (getPreviousText == null)
        {
            Debug.LogWarning("The obtained text for the card is null.", gameObject);    
            return;
        }
        
        sentenceText.Localize(getPreviousText.localizedTextId, getPreviousText.localizationFile);

        likeButton.SetToInitialState();
        
        if (IsCurrentTextLiked())
            likeButton.Switch();


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
}
