using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System;
using System.Collections.Generic;
using GameUp;

public class FBScript : MonoBehaviour
{
    public static FBScript Instance;
    public string mixpanelToken;
    public GameObject CameraGuide;
    public Transform Mount;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            FB.Init(SetInit, OnHideUnity);

            Mixpanel.Token = mixpanelToken;
            Mixpanel.SuperProperties.Add("platform", Application.platform.ToString());
            Mixpanel.SuperProperties.Add("quality", QualitySettings.names[QualitySettings.GetQualityLevel()]);
            Mixpanel.SuperProperties.Add("fullscreen", Screen.fullScreen);
            Mixpanel.SuperProperties.Add("resolution", Screen.width + "x" + Screen.height);

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            if (MPScript.Data.SkipLogin)
            {
                CameraGuide.GetComponent<MenuCamController>().setMount(Mount);
                MPScript.Data.SkipLogin = false;
            }
            Destroy(gameObject);
        }
    }

    public void Invite()
    {
        FB.AppRequest("You have been invited to play!", OGActionType.TURN, null, null, null, null, AfterInvite);
    }

    private void AfterInvite(IAppRequestResult result)
    {
        if (result.To != null)
            foreach (string id in result.To)
                FB.API("/id", HttpMethod.GET, RetrieveInvitedName);
    }

    private void RetrieveInvitedName(IGraphResult result)
    {
        // Does not work until FB Inviting works
        // APIController.SavePlayer(result.ResultDictionary["id"].ToString(), result.ResultDictionary["name"].ToString().Replace(" ", "%20"));
    }

    private void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("FB is logged in");
        }
        else
        {
            Debug.Log("FB is not logged in");
        }
    }

    private void OnHideUnity(bool isUnityShown)
    {
        if (!isUnityShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FBLogIn()
    {
        if (FB.IsLoggedIn)
            FB.LogOut();

        IEnumerable<string> permissions = new string[] { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, AuthCallBack);

    }

    public void FBLogOut()
    {
        Mixpanel.SendEvent("Logout");
        FB.LogOut();
    }

    private void AuthCallBack(ILoginResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("FB is logged in");
                FB.API("me?fields=first_name,last_name,email", HttpMethod.GET, RetrieveName);
                CameraGuide.GetComponent<MenuCamController>().setMount(Mount);
            }
            else
            {

                Debug.Log("FB is not logged in");
            }
        }
    }

    private void onError(int statusCode, string reason)
    {
        Debug.LogFormat("Something went wrong with the HeroicLabs SDK ({0} - {1}", statusCode, reason);
    }

    private void RetrieveName(IGraphResult result)
    {
        foreach (var a in result.ResultDictionary)
            Debug.Log(a.Key + "-" + a.Value);

        string id = (string)result.ResultDictionary["id"];
        string name = String.Format("{0} {1}", result.ResultDictionary["first_name"], result.ResultDictionary["last_name"]);

        // API
        APIController.SavePlayer(id, name.Replace(" ", "%20"), AccessToken.CurrentAccessToken.TokenString);

        // HeroicLabs
        Client.ApiKey = "31c210da7f0b4110bc301544870733d6";
        Client.Ping(onError);
        Client.LoginOAuthFacebook(AccessToken.CurrentAccessToken.TokenString, (SessionClient session) =>
        {
            MPScript.Data.SessionClient = session;
            MPScript.Data.SessionClient.Gamer((Gamer gamer) =>
            {
                string nickname = name.Replace(' ', '_');
                if (gamer.Nickname != nickname)
                {
                    MPScript.Data.SessionClient.UpdateGamer(nickname, onSuccess,
                    onError);
                }
            }, onError);
        }, onError);

        // Mixpanel
        Mixpanel.DistinctID = id;
        Mixpanel.SendUser(new Dictionary<string, object>
        {
            { "$first_name", result.ResultDictionary["first_name"] },
            { "$last_name", result.ResultDictionary["last_name"] },
            { "$name", name },
            { "$email", result.ResultDictionary.ContainsKey("email") ? result.ResultDictionary["email"] : "-" }
        });
        Mixpanel.SendEvent("Login");
    }

    private void onSuccess()
    {
        Debug.Log("Added player on HeroicLabs");
    }
}
