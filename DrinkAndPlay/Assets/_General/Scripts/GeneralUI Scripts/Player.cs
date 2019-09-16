using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string player;
    private PlayersMenu playersMenu;
    [SerializeField] private TextMeshProUGUI playerText;

    public void Setup(string player, PlayersMenu playersMenu)
    {
        this.player = player;
        this.playersMenu = playersMenu;
        playerText.text = player;
    }

    public void RemovePlayer()
    {
        GameManager.instance.dataManager.RemovePlayer(player);
        playersMenu.BuildPlayerList();
        playersMenu.SetDoneButtonAvailability();
    }

}
