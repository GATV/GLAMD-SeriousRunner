using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ScrollviewMatches : MonoBehaviour
{

    public GameObject template_MatchItem;
    private List<Match> matchesDataList = new List<Match>();

    //Was testing purpose

    // Use this for initialization
    void Start()
    {
        //matchesDataList.Add(new MatchDataContainer("GAT Vermeulen", 674, "Gewonnen"));


        //foreach (MatchDataContainer data in matchesDataList)
        //{
        //    GameObject objToInstantiate = Instantiate(template_MatchItem) as GameObject;
        //    objToInstantiate.SetActive(true);
        //    MatchPanelItem MPI = objToInstantiate.GetComponent<MatchPanelItem>();
        //    MPI.SetMatchInfo(data.friendName, data.score, data.matchState);
        //}
    }

    // Update is called once per frame

    public void ButtonClicked(string ChalID)
    {
        Debug.Log(ChalID + " has been challenged! ");
    }
}
