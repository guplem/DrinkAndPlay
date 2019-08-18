using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayersMenu : MonoBehaviour
{
    [SerializeField] private Button doneButton;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform contentsObject;
    [Tooltip("Elements that are child of the 'Contents' GameObject that are not part of the player's list (input field, white spaces, ...)")]
    [SerializeField] private Transform[] elementsNotInPlayerList;
    [SerializeField] private TMP_InputField addPlayerInputField;
    private void Start()
    {
        BuildPlayerList();
    }

    public void AddPlayer(string newPlayer)
    {
        if (!GameManager.instance.dataManager.CanAddPlayer(newPlayer))
            return;

        GameManager.instance.dataManager.AddPlayer(newPlayer);
        BuildPlayerList();
        addPlayerInputField.text = "";
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
        
        foreach (string player in GameManager.instance.dataManager.GetPlayers())
        {
            Instantiate(playerPrefab, contentsObject).GetComponent<Player>().Setup(player, this);
        }
    }
}
