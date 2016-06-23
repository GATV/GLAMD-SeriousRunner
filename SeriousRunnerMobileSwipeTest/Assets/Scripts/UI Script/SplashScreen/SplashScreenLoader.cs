using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenLoader : MonoBehaviour
{
    [SerializeField]
    private Image loadingIMG;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("CountDown");
    }

    // Update is called once per frame
    void Update()
    {
        loadingIMG.color = new Color(loadingIMG.color.r,
                                      loadingIMG.color.g,
                                      loadingIMG.color.b,
                                      Mathf.PingPong(Time.time, 1));
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(6);

        AsyncOperation async = SceneManager.LoadSceneAsync(1);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
