using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GeneralUI : MonoBehaviour
{
    [Header("Top Bar")]
    [SerializeField] public GameObject topBar;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject sectionTitle;
    [SerializeField] private GameObject gameTitle;
    [SerializeField] private GameObject configButton;

    /*[Header("Bottom Bar")]
    [SerializeField] public GameObject bottomBar;
    [SerializeField] private GameObject likeButton;
    [SerializeField] private GameObject addButton;
    [SerializeField] private GameObject shareButton;*/

    [Header("Menus")]
    [SerializeField] private AnimationUI configMenu;
    [SerializeField] private AnimationUI languageMenu;
    [SerializeField] private AnimationUI visualsMenu;
    [SerializeField] private AnimationUI playersMenu;
    private PlayersMenu playersMenuController;
    [SerializeField] private AnimationUI naughtyLevelMenu;
    [SerializeField] private AnimationUI feedbackMenu;
    [SerializeField] private AnimationUI randomChallengesMenu;
    
    [Header("Popups")]
    [SerializeField] private AnimationUI ratePopup;
    [SerializeField] private AnimationUI randomChallengePopup;
    [SerializeField] private LocalizationFile randomChallengesLocalizationFile;
    [SerializeField] private Localizer randomChallengesText;

    private Stack<AnimationUI> openUI = new Stack<AnimationUI>();

    
    
    public void SetupFor(Section section)
    {
        Debug.Log("Setting up General UI for the section '" + section + "'");
        
        if (section == null)
            Debug.LogError("The General UI can not be set up for a null section.", gameObject);
        
        topBar.SetActive(section.topBar);
        backButton.SetActive(section.backButton);
        sectionTitle.SetActive(section.sectionTitle);
        gameTitle.SetActive(section.appTitle);
        configButton.SetActive(section.configButton);

        if (section.sectionTitle)
            sectionTitle.GetComponent<Localizer>().Localize(section.nameId);

        if (playersMenuController == null)
            playersMenuController = playersMenu.GetComponent<PlayersMenu>();
    }

    public void CloseLastOpenUiElement()
    {
        if (openUI.Count > 0)
        {
            AnimationUI lastElement = openUI.Pop();
            if (lastElement == null)
            {
                CloseLastOpenUiElement();
                return;
            }
            else
            {
                Hide(lastElement);
            }
        }
            
        else
            GameManager.instance.PlaySection(GameManager.instance.landingSection);
    }

    public bool Show(AnimationUI uiElement)
    {
        if (openUI.Contains(uiElement))
        {
            Debug.LogWarning("Trying to open an UI element (" + uiElement + ") that already is opened. This can not happen");
            return false;
        }

        uiElement.Show();
        openUI.Push(uiElement);
        return true;
    }

    public bool Hide(AnimationUI uiElement)
    {
        if (openUI.Contains(uiElement))
        {
            Debug.LogError("Trying to close an UI Element (" + uiElement + ") was not the las opened. This can not happen.");
            return false;
        }

        uiElement.Hide();
        return true;
    }

    public void OpenConfigMenu()
    {
        Debug.Log("Opening ConfigMenu");
        Show(configMenu);
    }
    public void OpenLanguageMenu()
    {
        Debug.Log("Opening LanguageMenu");
        Show(languageMenu);
    }
    public void OpenVisualsMenu()
    {
        Debug.Log("Opening VisualsMenu");
        Show(visualsMenu);
    }
    public void OpenPlayersMenu()
    {
        ShowPlayersMenu();
        if (SectionManager.instance.section.minNumberOfPlayers > 0)
            playersMenuController.ShowPlayersAdditionalElements(SectionManager.instance.section.minNumberOfPlayers, null);
        else
            playersMenuController.HidePlayersAdditionalElements();

        playersMenuController.BuildPlayerList();
    }
    public void OpenPlayersMenu(int minPlayerNumber, Section sectionToOpenAfter)
    {
        ShowPlayersMenu();
        playersMenuController.ShowPlayersAdditionalElements(minPlayerNumber, sectionToOpenAfter);
        
        playersMenuController.BuildPlayerList();
    }

    private void ShowPlayersMenu()
    {
        Debug.Log("Opening PlayersMenu");
        Show(playersMenu);
    }
    public void OpenNaughtyLevelMenu()
    {
        Debug.Log("Opening NaughtyLevelMenu");
        Show(naughtyLevelMenu);
    }
    public void OpenDonations()
    {
        Debug.Log("Opening Donations");
        Application.OpenURL("https://paypal.me/gillemp");
    }
    public void OpenFeedbackMenu()
    {
        Debug.Log("Opening FeedbackMenu - General");
        Show(feedbackMenu);
        feedbackMenu.GetComponent<FeedbackMenu>().Setup(FeedbackMenu.FeedbackTime.General);
    }
    
    public void OpenFeedbackMenuCocktails()
    {
        Debug.Log("Opening FeedbackMenu - Cocktails");
        Show(feedbackMenu);
        feedbackMenu.GetComponent<FeedbackMenu>().Setup(FeedbackMenu.FeedbackTime.Cocktail);
    }
    
    public void OpenFeedbackMenuCurrentSection()
    {
        Debug.Log("Opening FeedbackMenu - Current Section");
        Show(feedbackMenu);
        feedbackMenu.GetComponent<FeedbackMenu>().Setup(FeedbackMenu.FeedbackTime.Section);
    }
    
    public void OpenRandomChallengesMenu()
    {
        Debug.Log("Opening RandomChallengesMenu");
        Show(randomChallengesMenu);
    }

    public void ShowRandomChallenge()
    {
        Debug.Log("Displaying random challenge");
        randomChallengePopup.Show();

        if (!GameManager.instance.localizationManager.IsSectionLocalized(randomChallengesLocalizationFile))
            GameManager.instance.localizationManager.LoadCurrentLanguageFor(randomChallengesLocalizationFile);

        LocalizedText lt = GameManager.instance.localizationManager.GetLocalizedText(randomChallengesLocalizationFile, true, true);
        randomChallengesText.Localize(lt.id);
    }

    public void ShowRatePopup()
    {
        Debug.Log("Displaying rate popup");
        ratePopup.Show();
        GameManager.instance.dataManager.ratePopupShown = true;
    }

    public void RateApp()
    {
        Application.OpenURL ("market://details?id=com.TriunityStudios.DrinkAndPlay");
        GameManager.instance.dataManager.ratedApp = true;
    }
    
    public void HideRatePopup()
    {
        ratePopup.Hide();
    }

    public void Share()
    {
        StartCoroutine( TakeSSAndShare() );
    }
    
    private IEnumerator TakeSSAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
        ss.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
        ss.Apply();

        string filePath = Path.Combine( Application.temporaryCachePath, "shared img.png" );
        File.WriteAllBytes( filePath, ss.EncodeToPNG() );
	
        // To avoid memory leaks
        Destroy( ss );

        string text = GameManager.instance.localizationManager.GetLocalizedText(GameManager.instance.uiLocalizationFile, "ShareText", false).text;
        new NativeShare().AddFile(filePath).SetText(text).Share();

        // Share on WhatsApp only, if installed (Android only)
        // if( NativeShare.TargetExists( "com.whatsapp" ) )
        // new NativeShare().AddFile( filePath ).SetText( "Hello world!" ).SetTarget( "com.whatsapp" ).Share();
    }

    public void ShowInformationPopup(LocalizationFile messageLocalizationFile, string messageId, LocalizationFile buttonLocalizationFile, string buttonId)
    {
        Debug.Log("Displaying information popup with text with id = ''" + messageId + "'");
        //TODO - Not necessary yet. Prepared for future needs
    }
}