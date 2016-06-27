using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using Assets.Scripts.Helpers;
using System;

public class MatchPanelItem : MonoBehaviour
{
    private Match match;
    private int myScore;
    private int theirScore;
    private string friendId;

    public Text friendNameText;
    public Text friendScoreText;
    public Button matchButton;
    public Text matchButtonText;
    public Image imgBackground;
    public ScrollviewMatches scrollView;

    public void SetMatchInfo(Match match)
    {
        this.match = match;

        if (Mixpanel.DistinctID == match.ChallengerId)
        {
            friendId = match.OpponentId;
            theirScore = match.OpponentScore;
            myScore = match.ChallengerScore;
        }
        else
        {
            friendId = match.ChallengerId;
            theirScore = match.ChallengerScore;
            myScore = match.OpponentScore;
        }

        if (theirScore == -1)
            friendScoreText.text = string.Empty;
        else
            friendScoreText.text = theirScore.ToString();

        string name = APIController.GetPlayerName(friendId);
        if (!string.IsNullOrEmpty(name))
            friendNameText.text = name;
        else
            friendNameText.text = "Error: Name Missing";

        SetMatchButtonColor();
    }

    public void SetMatchButtonColor()
    {
        if (match.Completed)
        {
            // Challenger wins if it's a draw
            if (myScore > theirScore || (myScore == theirScore && myScore == match.ChallengerScore))
            {
                // Gewonnen
                matchButtonText.text = "Gewonnen";
                matchButton.interactable = false;
                imgBackground.color = new Color(0.2f, 0.75f, 0.2f, 1);
            }
            else
            {
                // Verloren
                matchButtonText.text = "Verloren";
                matchButton.interactable = false;
                imgBackground.color = new Color(1, 0, 0, 1);
            }
        }
        else
        {
            if (myScore == -1)
            {
                // Start
                matchButtonText.text = "Start";
                matchButton.interactable = true;
                imgBackground.color = new Color(0.2f, 0.2f, 0.85f, 1);
            }
            else
            {
                // Bezig
                matchButtonText.text = "Bezig";
                matchButton.interactable = false;
                imgBackground.color = new Color(0.1f, 0.65f, 0.8f, 1);
            }
        }
    }

    public void Button_Click()
    {
        MPScript.Data.Match = match;
        MPScript.Data.ReplayData = APIController.GetReplay(new Guid(match.ReplayId));
        GlobalRandom.Seed = match.Seed;
        SceneManager.LoadScene(2);
    }
}
