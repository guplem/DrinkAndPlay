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

    public void AddPlayer(string newPlayerName)
    {
        Player newPlayer = new Player(newPlayerName);
        
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
        Player writtenPlayer = new Player(currentPlayerWriting);
        doneButton.interactable = GameManager.instance.dataManager.CanAddPlayer(writtenPlayer);
    }

    public void BuildPlayerList()
    {
        UtilsUI.DestroyContentsOf(contentsObject, elementsNotInPlayerList.ToList());

        bool allowRemoval = true;
        if (SectionManager.instance.section.minNumberOfPlayers > 0)
            allowRemoval = CanAPlayerBeDisabledOrRemoved();
        
        for (int p = 0; p < GameManager.instance.dataManager.GetPlayers().Count; p++)
        {
            Player player = GameManager.instance.dataManager.GetPlayer(p);
            GameObject playerGo = Instantiate(playerPrefab, contentsObject);
            playerGo.transform.SetSiblingIndex(p+5);
            


            playerGo.GetComponent<PlayerUI>().Setup(player, this, allowRemoval);
        }

        if (GameManager.instance.dataManager.lastSelectedSection != null)
        {
            playButton.interactable = GameManager.instance.dataManager.HaveEnougheNABLEDPlayersFor(GameManager.instance.dataManager.lastSelectedSection);
            ShowPlayersDescription(GameManager.instance.dataManager.lastSelectedSection.minNumberOfPlayers);
        }
        else
        {
            ShowPlayersDescription(allowRemoval? -1 : SectionManager.instance.section.minNumberOfPlayers);
        }
            
    }

    private bool CanAPlayerBeDisabledOrRemoved()
    {
        return SectionManager.instance.section.minNumberOfPlayers < GameManager.instance.dataManager.GetEnabledPlayersQuantity();
    }

    public void HidePlayersAdditionalElements()
    {
        ShowPlayButton(null);
    }

    public void ShowPlayersAdditionalElements(int minPlayerQuantity, Section sectionToPlayAfter)
    {
        ShowPlayButton(sectionToPlayAfter);
    }
    
    private void ShowPlayersDescription(int minPlayerQuantity)
    {
        if (minPlayerQuantity < 0)
        {
            minNumberOfPlayersDescription.gameObject.SetActive(false);
            return;            
        }
        
        minNumberOfPlayersDescription.gameObject.SetActive(true);
        minNumberOfPlayersDescription.Localize();
        minNumberOfPlayersDescription.textsToLocalize[0].tmProGui.text += " " + minPlayerQuantity + "+";
    }

    private void ShowPlayButton(Section sectionToPlayAfter)
    {
        GameManager.instance.dataManager.lastSelectedSection = sectionToPlayAfter;
        foreach (GameObject element in playButtonElements)
            element.SetActive(sectionToPlayAfter != null);
    }
    
    
    public void PlaySelectedSection()
    {
        playButton.GetComponent<ButtonAnimation>().MidAnimEvent += LoadSelectedSectionAtEvent;
    }
    
    private void LoadSelectedSectionAtEvent()
    {
        GameManager.instance.generalUi.CloseLastOpenUiElement();
        GameManager.instance.PlaySection(GameManager.instance.dataManager.lastSelectedSection);

        playButton.GetComponent<ButtonAnimation>().MidAnimEvent -= LoadSelectedSectionAtEvent;
    }
}
