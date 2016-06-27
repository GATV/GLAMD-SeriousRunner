using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;
using GameUp;
using System;
using System.Collections.Generic;

public class DevSkipItem : MonoBehaviour {
    public Player player;

    public Button button;
    public Text playerText;
    public Image imgBackground;
    public ScrollViewDevSkip scrollView;

    public void SetPlayerInfo(Player player)
    {
        this.player = player;
        playerText.text = player.Name;
        SetFriendListBackGroundColor();
    }

    public void FakeLogin()
    {
        // API
        APIController.SavePlayer(player.PlayerId, player.Name.Replace(" ", "%20"), player.AccessToken);

        // HeroicLabs
        Client.ApiKey = "31c210da7f0b4110bc301544870733d6";
        Client.Ping(onError);
        Client.LoginOAuthFacebook(player.AccessToken, (SessionClient session) =>
        {
            MPScript.Data.SessionClient = session;
            MPScript.Data.SessionClient.Gamer((Gamer gamer) =>
            {
                string nickname = player.Name.Replace(' ', '_');
                if (gamer.Nickname != nickname)
                {
                    MPScript.Data.SessionClient.UpdateGamer(nickname, onSuccess,
                    onError);
                }
            }, onError);
        }, onError);

        // Mixpanel
        Mixpanel.DistinctID = player.PlayerId;
        Mixpanel.SendEvent("DevSkip Login");
    }

    public void SetMount()
    {
        scrollView.SetMount();
    }

    private void onSuccess()
    {
        Debug.Log("DevSkip");
    }

    private void onError(int statusCode, string reason)
    {
        Debug.LogFormat("Something went wrong with the HeroicLabs SDK ({0} - {1}", statusCode, reason);
    }

    public void SetFriendListBackGroundColor()
    {
        imgBackground.color = new Color(0.2f, 0.38f, 0.0f, 1);
    }
}
