using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Assets.Scripts;

public class ScrollviewMatches : MonoBehaviour
{
    public GameObject scrollContent;
    public GameObject template_MatchItem;
    private List<Match> matches = new List<Match>();

    //Was testing purpose

    // Use this for initialization
    void Start()
    {
        matches.Add(new Match
        {
            MatchId = "MatchId",
            ChallengerId = Mixpanel.DistinctID,
            ChallengerScore = 123,
            OpponentId = "John Doe",
            OpponentScore = -1,
            ReplayId = "ReplayId",
            Completed = false,
            Seed = 1337
        });

        foreach (Match m in matches)
        {

            GameObject objToInstantiate = Instantiate(template_MatchItem) as GameObject;
            objToInstantiate.SetActive(true);
            MatchPanelItem MPI = objToInstantiate.GetComponent<MatchPanelItem>();
            MPI.scrollView = this;
            MPI.SetMatchInfo(m);
            objToInstantiate.transform.SetParent(scrollContent.transform, false);

        }


    }

    // Update is called once per frame

    public void ButtonClicked(string ChalID)
    {
        Debug.Log(ChalID + " has been challenged! ");
    }
}
