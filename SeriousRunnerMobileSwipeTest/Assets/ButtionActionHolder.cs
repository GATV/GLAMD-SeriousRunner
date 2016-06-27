using UnityEngine;
using System.Collections;

public class ButtionActionHolder : MonoBehaviour
{
    public void AppRequest()
    {
        FBScript.Instance.Invite();
    }

    public void LoginFacebook()
    {
        FBScript.Instance.FBLogIn();
    }

    public void LogoutFacebook()
    {
        FBScript.Instance.FBLogOut();
    }

    public void UpdateMultiplayerMenu()
    {
        ScrollviewMatches.UpdateList();
    }

    public void UpdateLeaderboard()
    {
        ScrollviewLeaderbord.UpdateList();
    }

    public void UpdateFriendList()
    {
        ScrollviewFriendList.UpdateList();
    }

    public void UpdateDevSkipList()
    {
        ScrollViewDevSkip.UpdateList();
    }
}
