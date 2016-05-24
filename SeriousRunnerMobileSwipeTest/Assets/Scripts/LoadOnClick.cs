using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour
{

  private bool loadScene = false;

  [SerializeField]
  private int scene;
  [SerializeField]
  private Text loadingText;

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
      loadingText.text = "Loading...";
      StartCoroutine(LoadNewScene());
    }

    if (loadScene == true)
    {
      loadingText.color = new Color(loadingText.color.r,
                                    loadingText.color.g,
                                    loadingText.color.b,
                                    Mathf.PingPong(Time.time, 1));
    }
  }

  private IEnumerator LoadNewScene()
  {
    yield return new WaitForSeconds(3);

    AsyncOperation async = SceneManager.LoadSceneAsync(scene);

    while (!async.isDone)
    {
      yield return null;
    }
  }
}
