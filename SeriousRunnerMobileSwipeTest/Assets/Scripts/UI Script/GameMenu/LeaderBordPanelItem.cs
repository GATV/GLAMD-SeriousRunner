using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts;
using System.Collections;

public class LeaderBordPanelItem : MonoBehaviour
{
    private LeaderBordRanking ranking;    

    public Text RankIDText;
    public Text RankScoreText;
    public Text RankNameText;
    public Image imgBackground;
    public ScrollviewLeaderbord scrollView;

    public void setRankInfo(LeaderBordRanking ranking)
    {
        this.ranking = ranking;

        RankNameText.text = ranking.NameID;
        RankScoreText.text = ranking.Score.ToString();
        RankIDText.text = ranking.Rank.ToString();

        SetLeaderbordRankingColor();
    }

    public void SetLeaderbordRankingColor()
    {
        if (ranking.IsPersonalScore)
        {
            imgBackground.color = new Color(0.1f, 0.65f, 0.8f, 1);//TEAL
        }
        else
        {
            imgBackground.color = new Color(0.2f, 0.2f, 0.85f, 1);//BLUE
        }
    }

}
