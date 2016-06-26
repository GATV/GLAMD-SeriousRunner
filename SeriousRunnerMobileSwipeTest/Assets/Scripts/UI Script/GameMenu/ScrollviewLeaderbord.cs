using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Assets.Scripts;
using System;
using GameUp;
using Facebook.Unity;

public class ScrollviewLeaderbord : MonoBehaviour
{

    public GameObject scrollContent;
    public GameObject template_LeaderBordItem;

    private static GameObject template;
    private static GameObject content;
    private static ScrollviewLeaderbord scrollView;

    public static void UpdateList()
    {
        foreach (Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }

        Client.ApiKey = "31c210da7f0b4110bc301544870733d6";
        string leaderboardId = "b967fed849f543babe70212b0f39f3c0";
        MPScript.Data.SessionClient.LeaderboardAndRank(leaderboardId, onSuccess, onError);
    }

    private static void onSuccess(LeaderboardAndRank leaderboard)
    {
        List<LeaderBordRanking> rankings = new List<LeaderBordRanking>();

        Rank userRank = leaderboard.Rank;
        foreach (Leaderboard.Entry entry in leaderboard.Leaderboard)
        {            
            rankings.Add(new LeaderBordRanking
            {
                NameID = entry.Name.Replace('_', ' '),
                Rank = (int)entry.Rank,
                Score = (int)entry.Score,
                IsPersonalScore = entry.Rank == userRank.Ranking
            });
        }


        foreach (LeaderBordRanking lbr in rankings.OrderBy(x => x.Rank))
        {
            GameObject objToInstantiate = Instantiate(template);
            objToInstantiate.SetActive(true);
            LeaderBordPanelItem LPI = objToInstantiate.GetComponent<LeaderBordPanelItem>();
            LPI.setRankInfo(lbr);
            objToInstantiate.transform.SetParent(content.transform, false);
            LPI.scrollView = scrollView;
        }
    }

    private static void onError(int statusCode, string reason)
    {
        Debug.LogFormat("Something went wrong with the HeroicLabs SDK ({0} - {1}", statusCode, reason);
    }

    void Start()
    {
        template = template_LeaderBordItem;
        content = scrollContent;
        scrollView = this;
    }

}
