using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Assets.Scripts;
using System;
using GameUp;
using Facebook.Unity;
using UnityEngine.UI;

public class ScrollviewFriendList : MonoBehaviour
{
    public GameObject scrollContent;
    public GameObject template_FriendListItem;
    public Button uitdagenButton;

    private static GameObject template;
    private static GameObject content;
    private static ScrollviewFriendList scrollView;
    private List<Player> checkedPlayers = new List<Player>();

    public static void UpdateList()
    {
        foreach (Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }

        Player[] players = APIController.GetAllPlayers();

        foreach (Player p in players.Where(x => x.PlayerId != Mixpanel.DistinctID))
        {
            GameObject objToInstantiate = Instantiate(template);
            objToInstantiate.SetActive(true);
            FriendListPanelItem FPI = objToInstantiate.GetComponent<FriendListPanelItem>();
            FPI.SetPlayerInfo(p);
            objToInstantiate.transform.SetParent(content.transform, false);
            FPI.scrollView = scrollView;
        }
    }

    public void UpdateCheckedPlayerList(Player player, bool value)
    {
        if (value)
            checkedPlayers.Add(player);
        else
            checkedPlayers.Remove(player);

        uitdagenButton.interactable = checkedPlayers.Count > 0;
    }

    public void ChallengeSelectedPlayers()
    {
        MPScript.Data.ChallengedPlayers = checkedPlayers.ToArray();
    }

    void Start()
    {
        template = template_FriendListItem;
        content = scrollContent;
        scrollView = this;
    }
}
