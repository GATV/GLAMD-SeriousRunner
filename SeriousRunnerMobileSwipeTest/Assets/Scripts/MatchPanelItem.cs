using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class MatchPanelItem : MonoBehaviour
{
    private Match match;
    private int myScore;
    private int theirScore;

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
            friendNameText.text = match.OpponentId;
            friendScoreText.text = match.OpponentScore.ToString();
            myScore = match.ChallengerScore;
            theirScore = match.OpponentScore;        
        }
        else
        {
            friendNameText.text = match.ChallengerId;
            friendScoreText.text = match.ChallengerScore.ToString();
            myScore = match.OpponentScore;
            theirScore = match.ChallengerScore;
        }

        SetMatchButtonColor();
    }

    public void SetFakeMatch(string name , int score)
    {
        friendNameText.text = name;
        friendScoreText.text =score.ToString();
    }
    public void SetMatchButtonColor()
    {        
        if (match.Completed)
        {
            // Challenger wins if it's a draw
            if (myScore > theirScore || (myScore == theirScore && myScore == match.ChallengerScore))
            {
                // Gewonnen green
                matchButtonText.text = "Gewonnen";                
                matchButton.interactable = false;
                imgBackground.color = new Color(0, 255, 12, 100);
            }
            else
            {
                // Verloren rood
                matchButtonText.text = "Verloren";                
                matchButton.interactable = false;
                imgBackground.color = new Color(255, 0, 0, 100);
            }
        }
        else
        {
            if (myScore == -1)
            {
                // Start blue
                matchButtonText.text = "Start";
                matchButton.interactable = true;
                imgBackground.color = new Color(0, 55, 255, 100);                
            }
            else
            {
                // Bezig  teal
                matchButtonText.text = "Bezig";
                matchButton.interactable = false;
                imgBackground.color = new Color(0, 0, 255, 100);                
            }
        }    
    }    

    public void Button_Click()
    {
        // Play
        if (matchButton.interactable)
            Debug.Log("Let's go!");        
    }
}
