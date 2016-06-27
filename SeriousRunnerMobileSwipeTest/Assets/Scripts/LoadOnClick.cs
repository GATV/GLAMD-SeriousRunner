using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Assets.Scripts.Helpers;

public class LoadOnClick : MonoBehaviour
{

    private bool loadScene = false;

    [SerializeField]
    private int scene;
    [SerializeField]
    private Image loadingIMG;

    private bool loadEnabled = false;

    public int LoadEnabled
    {
        set
        {
            scene = value;
            loadEnabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!loadScene && loadEnabled)
        {
            loadScene = true;
            StartCoroutine(LoadNewScene());
        }

        if (loadScene == true)
        {
            loadingIMG.color = new Color(loadingIMG.color.r,
                                          loadingIMG.color.g,
                                          loadingIMG.color.b,
                                          Mathf.PingPong(Time.time, 1));
        }
    }

    private IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(5);

        if (scene == 1)
            MPScript.Data.SkipLogin = true;
        else if (scene == 2)
        {
            if (MPScript.Data.ChallengedPlayers != null)
                GlobalRandom.Seed = DateTime.Now.Millisecond;
            else
                MPScript.Data.Clean();
        }

        SceneManager.LoadScene(scene);
    }
}
