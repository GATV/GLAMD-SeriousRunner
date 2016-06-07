using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System;

public class FBSharerController : MonoBehaviour {

	public void Share()
    {
        FB.ShareLink(new Uri(@"http://seriousrequest.3fm.nl/"), "3FM Serious Runner Title", "3FM Serious Runner Description", new Uri(@"http://wordeenklantmagneet.nl/wp-content/uploads/2013/05/serious-request-logo.png"), afterShare);
    }

    private void afterShare(IShareResult result)
    {
        Mixpanel.SendEvent("Shared");
    }
}
