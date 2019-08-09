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


    public void OpenConfigMenu()
    {
        Debug.Log("Opening ConfigMenu");
        configMenu.Show();
    }
    public void OpenRandomSentencesMenu()
    {
        Debug.Log("Opening RandomSentencesMenu");
        randomSentencesMenu.Show();
    }
    public void OpenLanguageMenu()
    {
        Debug.Log("Opening LanguageMenu");
        languageMenu.Show();
    }
    public void OpenPlayersMenu()
    {
        Debug.Log("Opening PlayersMenu");
        playersMenu.Show();
    }
    public void OpenNaughtyLevelMenu()
    {
        Debug.Log("Opening NaughtyLevelMenu");
        naughtyLevelMenu.Show();
    }
    public void OpenDonations()
    {
        Debug.Log("Opening Donations");
        //TODO: Open link
    }
    public void OpenFeedbackMenu()
    {
        Debug.Log("Opening FeedbackMenu");
        FeedbackMenu.Show();
    }
    public void OpenAddSentenceMenu()
    {
        Debug.Log("Opening AddSentenceMenu");
        AddSentenceMenu.Show();
    }



}
