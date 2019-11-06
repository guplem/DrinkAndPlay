using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayersMenu : MonoBehaviour
{
    [SerializeField] private Button doneButton;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform contentsObject;
    [SerializeField] private Toggle rndPlayerOrderToggle;
    [Tooltip("Elements that are child of the 'Contents' GameObject that are not part of the player's list (input field, white spaces, ...)")]
    [SerializeField] private Transform[] elementsNotInPlayerList;
    [SerializeField] private TMP_InputField addPlayerInputField;
    [SerializeField] private Localizer minNumberOfPlayersDescription;

    [SerializeField] private Button playButton;
    [SerializeField] private GameObject[] playButtonElements;
    private Section selectedSection;
    
    private bool setUpDone = false; 
    
    private void Start()
    {
        //BuildPlayerList();
        SetDoneButtonAvailability();
        rndPlayerOrderToggle.isOn = GameManager.instance.dataManager.randomPlayerOrder;
        setUpDone = true;
    }
    
    public void SetRandomPlayerOrder(bool state)
    {
        if (setUpDone)
            GameManager.instance.dataManager.randomPlayerOrder = state;
    }

    public void AddPlayer(string newPlayer)
    {
        if (!GameManager.instance.dataManager.CanAddPlayer(newPlayer))
            return;

        GameManager.instance.dataManager.AddPlayer(newPlayer);
        BuildPlayerList();
        
        addPlayerInputField.text = "";
        UtilsUI.ClearSelectedElement();
    }

    private string currentPlayerWriting = "";
    public void PlayerChanged(string newPlayer)
    {
        currentPlayerWriting = newPlayer;
        SetDoneButtonAvailability();
    }

    public void SetDoneButtonAvailability()
    {
        doneButton.interactable = GameManager.instance.dataManager.CanAddPlayer(currentPlayerWriting);
    }

    public void BuildPlayerList()
    {
        UtilsUI.DestroyContentsOf(contentsObject, elementsNotInPlayerList.ToList());

        for (int p = 0; p < GameManager.instance.dataManager.GetPlayers().Count; p++)
        {
            string player = GameManager.instance.dataManager.GetPlayer(p);
            GameObject playerGo = Instantiate(playerPrefab, contentsObject);
            playerGo.transform.SetSiblingIndex(p+5);
            
            bool allowRemoval = true;
            if (SectionManager.instance.section.minNumberOfPlayers > 0)
                allowRemoval = SectionManager.instance.section.minNumberOfPlayers <
                               GameManager.instance.dataManager.GetPlayersQuantity();

            playerGo.GetComponent<Player>().Setup(player, this, allowRemoval);
        }
        
        if (selectedSection != null)
            playButton.interactable = GameManager.instance.HaveEnoughPlayersFor(selectedSection);
    }

    public void HidePlayersAdditionalElements()
    {
        minNumberOfPlayersDescription.gameObject.SetActive(false);
        SetupPlayButton(null);
    }

    public void ShowPlayersAdditionalElements(int minPlayerQuantity, Section sectionToPlayAfter)
    {
        ShowPlayersDescription(minPlayerQuantity);
        SetupPlayButton(sectionToPlayAfter);
    }
    
    private void ShowPlayersDescription(int minPlayerQuantity)
    {
        minNumberOfPlayersDescription.gameObject.SetActive(true);
        minNumberOfPlayersDescription.Localize();
        minNumberOfPlayersDescription.textsToLocalize[0].tmProGui.text += " " + minPlayerQuantity + "+";
    }

    private void SetupPlayButton(Section sectionToPlayAfter)
    {
        selectedSection = sectionToPlayAfter;
        foreach (GameObject element in playButtonElements)
            element.SetActive(sectionToPlayAfter != null);
    }
    
    
    public void PlaySelectedSection()
    {
        playButton.GetComponent<ButtonAnimation>().MidAnimEvent += LoadSelectedSectionAtEvent;
    }
    
    private void LoadSelectedSectionAtEvent()
    {
        GameManager.instance.PlaySection(selectedSection);
        GameManager.instance.generalUi.CloseLastOpenUiElement();
        
        playButton.GetComponent<ButtonAnimation>().MidAnimEvent -= LoadSelectedSectionAtEvent;
    }
}
