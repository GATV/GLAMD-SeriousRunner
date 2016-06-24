using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SharredEffortPot : MonoBehaviour {

    private Guid activeSEP = new Guid("A773A316-821C-44A9-B0CD-7BFAEA17E556");
    public Text SEPCountText;

    public void SEP()
    {
        int sep = APIController.GetSEP(activeSEP);

        SEPCountText.text = sep.ToString();
        Debug.Log("SEP: "  + sep);
    }
}
