using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUI : MonoBehaviour
{
    [Header("Top Bar")]
    [SerializeField] public GameObject topBar;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject sectionTitle;
    [SerializeField] private GameObject configButton;

    [Header("Bottom Bar")]
    [SerializeField] public GameObject bottomBar;
    [SerializeField] private GameObject likeButton;
    [SerializeField] private GameObject addButton;
    [SerializeField] private GameObject shareButton;

    [Header("Menus")]
    [SerializeField] private AnimationUI configMenu;
    [SerializeField] private AnimationUI randomSentencesMenu;
    [SerializeField] private AnimationUI languageMenu;
    [SerializeField] private AnimationUI playersMenu;
    [SerializeField] private AnimationUI naughtyLevelMenu;
    [SerializeField] private AnimationUI FeedbackMenu;
    [SerializeField] private AnimationUI AddSentenceMenu;

    private Stack<AnimationUI> openUI = new Stack<AnimationUI>();

    public void SetupFor(Section section)
    {
        topBar.SetActive(section.topBar);
        backButton.SetActive(section.backButton);
        sectionTitle.SetActive(section.sectionTitle);
        configButton.SetActive(section.configButton);

        bottomBar.SetActive(section.bottomBar);
        likeButton.SetActive(section.likeButton);
        addButton.SetActive(section.addButton);
        shareButton.SetActive(section.shareButton);

        if (section.sectionTitle)
            sectionTitle.GetComponent<Localizer>().Localize(section.nameId);

    }

    public void CloseLastOpenUiElement()
    {
        if (openUI.Count > 0)
            Hide(openUI.Pop());
        else
            GameManager.Instance.LoadSection(GameManager.Instance.landingSection);
    }

    public bool Show(AnimationUI UiElement)
    {
        if (openUI.Contains(UiElement))
        {
            Debug.LogError("Trying to open an UI element (" + UiElement + ") that already is opened. This can not happen");
            return false;
        }

        UiElement.Show();
        openUI.Push(UiElement);
        return true;
    }

    public bool Hide(AnimationUI UiElement)
    {
        if (openUI.Contains(UiElement))
        {
            Debug.LogError("Trying to close an UI Element (" + UiElement + ") was not the las opened. This can not happen.");
            return false;
        }

        UiElement.Hide();
        return true;
    }

    public void OpenConfigMenu()
    {
        Debug.Log("Opening ConfigMenu");
        Show(configMenu);
    }
    public void OpenRandomSentencesMenu()
    {
        Debug.Log("Opening RandomSentencesMenu");
        Show(randomSentencesMenu);
    }
    public void OpenLanguageMenu()
    {
        Debug.Log("Opening LanguageMenu");
        Show(languageMenu);
    }
    public void OpenPlayersMenu()
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
        //TODO: Open link
    }
    public void OpenFeedbackMenu()
    {
        Debug.Log("Opening FeedbackMenu");
        Show(FeedbackMenu);
    }
    public void OpenAddSentenceMenu()
    {
        Debug.Log("Opening AddSentenceMenu");
        Show(AddSentenceMenu);
    }





}
