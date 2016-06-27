using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using System;

public class FriendListPanelItem : MonoBehaviour
{

    public Player player;

    public Toggle toggleBox;
    public Text playerText;
    public Image imgBackground;
    public ScrollviewFriendList scrollView;

    public void SetPlayerInfo(Player player)
    {
        this.player = player;
        playerText.text = player.Name;

        SetFriendListBackGroundColor();

    }

    public void UpdateValue()
    {
        scrollView.UpdateCheckedPlayerList(player, toggleBox.isOn);
    }

    public void SetFriendListBackGroundColor()
    {
        imgBackground.color = new Color(0.2f, 0.38f, 0.0f, 1);
    }

}

