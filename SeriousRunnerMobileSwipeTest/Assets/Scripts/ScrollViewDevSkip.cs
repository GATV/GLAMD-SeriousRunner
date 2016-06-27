using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class ScrollViewDevSkip : MonoBehaviour {

    public GameObject scrollContent;
    public GameObject template_DevSkipItem;
    public GameObject CameraGuide;
    public Transform Mount;

    private static GameObject template;
    private static GameObject content;
    private static ScrollViewDevSkip scrollView;

    public static void UpdateList()
    {
        foreach (Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }

        Player[] players = APIController.GetAllPlayers();
        foreach (Player p in players)
        {
            GameObject objToInstantiate = Instantiate(template);
            objToInstantiate.SetActive(true);
            DevSkipItem FPI = objToInstantiate.GetComponent<DevSkipItem>();
            FPI.SetPlayerInfo(p);
            objToInstantiate.transform.SetParent(content.transform, false);
            FPI.scrollView = scrollView;
        }
    }

    public void SetMount()
    {
        CameraGuide.GetComponent<MenuCamController>().setMount(Mount);
    }

    void Start()
    {
        template = template_DevSkipItem;
        content = scrollContent;
        scrollView = this;
    }
}
