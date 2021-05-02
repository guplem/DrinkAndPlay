using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Player player;
    private PlayersMenu playersMenu;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private Button removeButton;
    [SerializeField] private Toggle enabledToggle;
    private bool setUpDone = false;

    public void Setup(Player player, PlayersMenu playersMenu, bool allowRemoval)
    {
        this.player = player;
        this.playersMenu = playersMenu;
        
        playerText.text = player.name;
        enabledToggle.isOn = player.enabled;
        
        removeButton.interactable = allowRemoval || !player.enabled;
        enabledToggle.interactable = allowRemoval || !player.enabled;
        
        this.playersMenu.SetDoneButtonAvailability();
        setUpDone = true;
        
    }

    public void RemovePlayer()
    {
        GameManager.instance.dataManager.RemovePlayer(player);
        UpdateList();
    }

    private void UpdateList()
    {
        playersMenu.BuildPlayerList();
        playersMenu.SetDoneButtonAvailability();
    }

    public void SetPlayerEnabled(bool state)
    {
        if (!setUpDone)
            return;
        
        GameManager.instance.dataManager.SetPlayerEnabled(player, state);
        UpdateList();
    }

}
