using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System;
using System.Collections.Generic;

public class FBScript : MonoBehaviour
{

    public string mixpanelToken;
    public GameObject CameraGuide;

    public Transform Mount;

    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);

        Mixpanel.Token = mixpanelToken;
        Mixpanel.SuperProperties.Add("platform", Application.platform.ToString());
        Mixpanel.SuperProperties.Add("quality", QualitySettings.names[QualitySettings.GetQualityLevel()]);
        Mixpanel.SuperProperties.Add("fullscreen", Screen.fullScreen);
        Mixpanel.SuperProperties.Add("resolution", Screen.width + "x" + Screen.height);
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
            }
            else
            {

                Debug.Log("FB is not logged in");
            }

            FB.API("me?fields=first_name,last_name,email", HttpMethod.GET, RetrieveName);
            CameraGuide.GetComponent<MenuCamController>().setMount(Mount);            
        }
    }

    private void RetrieveName(IGraphResult result)
    {
        Mixpanel.DistinctID = (string)result.ResultDictionary["id"];
        Mixpanel.SendUser(new Dictionary<string, object>
        {
            { "$first_name", result.ResultDictionary["first_name"] },
            { "$last_name", result.ResultDictionary["last_name"] },
            { "$name", String.Format("{0} {1}", result.ResultDictionary["first_name"], result.ResultDictionary["last_name"]) },
            { "$email", result.ResultDictionary.ContainsKey("email") ? result.ResultDictionary["email"] : "-" }
        });
        Mixpanel.SendEvent("Login");       
    }
}
