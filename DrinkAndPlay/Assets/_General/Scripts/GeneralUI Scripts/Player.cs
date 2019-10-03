using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private string player;
    private PlayersMenu playersMenu;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private Button removeButton;

    public void Setup(string player, PlayersMenu playersMenu, bool allowRemoval)
    {
        this.player = player;
        this.playersMenu = playersMenu;
        playerText.text = player;
        removeButton.interactable = allowRemoval;
    }

    public void RemovePlayer()
    {
        GameManager.instance.dataManager.RemovePlayer(player);
        playersMenu.BuildPlayerList();
        playersMenu.SetDoneButtonAvailability();
    }

}
