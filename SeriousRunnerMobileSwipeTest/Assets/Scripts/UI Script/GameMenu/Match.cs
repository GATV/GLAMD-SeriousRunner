using UnityEngine;
using System.Collections;

public class Match
{
    public string MatchId { get; set; }
    public string ChallengerId { get; set; }
    public int ChallengerScore { get; set; }
    public string OpponentId { get; set; }
    public int? OpponentScore { get; set; }
    public string ReplayId { get; set; }
    public bool Completed { get; set; }
    public int Seed { get; set; }
}
