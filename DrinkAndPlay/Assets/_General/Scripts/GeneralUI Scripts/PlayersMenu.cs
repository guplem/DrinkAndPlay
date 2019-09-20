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
    [Tooltip("Elements that are child of the 'Contents' GameObject that are not part of the player's list (input field, white spaces, ...)")]
    [SerializeField] private Transform[] elementsNotInPlayerList;
    [SerializeField] private TMP_InputField addPlayerInputField;
    [SerializeField] private Localizer minNumberOfPlayersDescription;
    private void Start()
    {
        BuildPlayerList();
        SetDoneButtonAvailability();
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
            playerGo.transform.SetSiblingIndex(p+4);
            playerGo.GetComponent<Player>().Setup(player, this);
        }
        
    }

    public void HidePLayersDescription()
    {
        minNumberOfPlayersDescription.gameObject.SetActive(false);
    }

    public void ShowPlayersDescription(int minPlayerQuantity)
    {
        minNumberOfPlayersDescription.gameObject.SetActive(true);
        minNumberOfPlayersDescription.Localize();
        minNumberOfPlayersDescription.textsToLocalize[0].tmProGui.text += " " + minPlayerQuantity + "+";
    }
}
