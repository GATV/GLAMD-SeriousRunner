using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MatchPanelItem : MonoBehaviour
{

    private string friendName;
    private int friendScore;
    private string matchState;

    public Text friendNameText;
    public Text friendScoreText;
    public Button matchButton;
    public Text matchButtonText;
    public Image imgBackground;
    public ScrollviewMatches scrollView;

    Dictionary<string, Color> colorDictionary =
    new Dictionary<string, Color>();

    public void SetMatchInfo(Match match)
    {
        friendName = match.ChallengerId;
        friendScore = match.ChallengerScore;

        //Moet nog aanpassen op wat conditie
        matchState = "Gewonnen";

        friendScoreText.text = friendScore.ToString();
        matchButtonText.text = matchState;
        friendNameText.text = friendName;
        SetMatchBackgroundColor(matchState);
    }

    public Color GetMatchColor(string matchstate)
    {
        switch (matchstate)
        {
            case "Gewonnen"://Groen
                colorDictionary.Add(matchstate, new Color( /*R*/ 0,
                                                           /*G*/ 255,
                                                           /*B*/ 12,
                                                           /*a*/ 100));
                matchButton.interactable = false;
                break;
            case "Verloren"://Rood
                colorDictionary.Add(matchstate, new Color( /*R*/ 255,
                                                           /*G*/ 0,
                                                           /*B*/ 0,
                                                           /*a*/ 100));
                matchButton.interactable = false;
                break;
            case "Bezig"://Teal
                colorDictionary.Add(matchstate, new Color( /*R*/ 0,
                                                        /*G*/ 255,
                                                        /*B*/ 255,
                                                        /*a*/ 100));
                matchButton.interactable = false;
                break;
            case "Start"://Blue
                colorDictionary.Add(matchstate, new Color( /*R*/ 0,
                                                        /*G*/ 55,
                                                        /*B*/ 255,
                                                        /*a*/ 100));
                matchButton.interactable = true;
                break;
        }
        return colorDictionary[matchstate];
    }

    public void SetMatchBackgroundColor(string matchState)
    {
        imgBackground.color = GetMatchColor(matchState);
    }

    public void Button_Click()
    {
        scrollView.ButtonClicked(friendName);
    }






}
