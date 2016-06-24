using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Assets.Scripts;
using System.Linq;

public class ScrollviewMatches : MonoBehaviour
{
    public GameObject scrollContent;
    public GameObject template_MatchItem;

    private static Match[] matches;
    private static GameObject template;
    private static GameObject content;
    private static ScrollviewMatches scrollView;

    public static void UpdateList()
    {
        foreach (Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }

        matches = APIController.GetMatches(Mixpanel.DistinctID);

        foreach (Match m in matches.OrderByDescending(x => x.Date))
        {
            GameObject objToInstantiate = Instantiate(template);
            objToInstantiate.SetActive(true);
            MatchPanelItem MPI = objToInstantiate.GetComponent<MatchPanelItem>();
            MPI.SetMatchInfo(m);
            objToInstantiate.transform.SetParent(content.transform, false);
            MPI.scrollView = scrollView;
        }
    }


    // Use this for initialization
    void Start()
    {
        template = template_MatchItem;
        content = scrollContent;
        scrollView = this;        
    }

    // Update is called once per frame

    public void ButtonClicked(string ChalID)
    {
        Debug.Log(ChalID + " has been challenged! ");
    }
}
