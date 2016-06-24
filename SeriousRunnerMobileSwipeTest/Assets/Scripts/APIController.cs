using UnityEngine;
using System.Collections;
using System;
using LitJson;
using System.Collections.Generic;
using Assets.Scripts;

public static class APIController
{
    private const string GET_SEP = "http://anthony-api.bouwe.it/api/SEP/{0}";
    private const string INCREMENT_SEP = "http://anthony-api.bouwe.it/api/SEP/{0}?increment={1}";

    private const string GET_REPLAY = "http://anthony-api.bouwe.it/api/Replay/{0}";
    private const string SAVE_REPLAY = "http://anthony-api.bouwe.it/api/Replay?replayData={0}";

    private const string GET_PLAYER = "http://anthony-api.bouwe.it/api/Player/{0}";
    private const string SAVE_PLAYER = "http://anthony-api.bouwe.it/api/Player/{0}?name={1}";

    private const string GET_MATCH = "http://anthony-api.bouwe.it/api/Match/{0}";
    private const string UPDATE_MATCH = "http://anthony-api.bouwe.it/api/Match/{0}?opponentScore={1}&completed={2}";
    private const string SAVE_MATCH = "http://anthony-api.bouwe.it/api/Match?challengerId={0}&challengerScore={1}&opponentId={2}&replayId={3}&seed={4}";    
    private const string GET_MATCHES = "http://anthony-api.bouwe.it/api/Match?playerId={0}";     

    public static Match[] GetMatches(string playerId)
    {
        WWW www = new WWW(string.Format(GET_MATCHES, playerId));
        while (!www.isDone) { }
        return JsonMapper.ToObject<Match[]>(www.text.Replace("null", "-1"));
    }

    public static Match GetMatch(Guid matchId)
    {
        WWW www = new WWW(string.Format(GET_MATCH, matchId));
        while (!www.isDone) { }
        return JsonMapper.ToObject<Match>(www.text.Replace("null", "-1"));
    }

    public static Match UpdateMatch(Guid matchId, int opponentScore, bool completed)
    {
        WWW www = new WWW(string.Format(UPDATE_MATCH, matchId, opponentScore, completed));
        Debug.Log(string.Format(UPDATE_MATCH, matchId, opponentScore, completed));
        while (!www.isDone) { }
        return JsonMapper.ToObject<Match>(www.text.Replace("null", "-1"));
    }

    public static Guid SaveMatch(string challengerId, int challengerScore, string opponentId, Guid replayId, int seed)
    {
        WWW www = new WWW(string.Format(SAVE_MATCH, challengerId, opponentId, replayId));
        while (!www.isDone) { }
        return new Guid(www.text.Trim('"'));
    }

    public static string GetPlayerName(string playerId)
    {
        WWW www = new WWW(string.Format(GET_PLAYER, playerId));
        while (!www.isDone) { }
        return www.text.Trim('"');
    }

    public static void SavePlayer(string playerId, string name)
    {
        WWW www = new WWW(string.Format(SAVE_PLAYER, playerId, name));
        while (!www.isDone) { }
    }

    public static string GetReplay(Guid replayId)
    {
        WWW www = new WWW(string.Format(GET_REPLAY, replayId));
        while (!www.isDone) { }
        return www.text.Trim('"');
    }

    public static Guid SaveReplay(string replayData)
    {
        WWW www = new WWW(string.Format(SAVE_REPLAY, replayData));
        while (!www.isDone) { }
        return new Guid(www.text.Trim('"'));
    }

    public static int GetSEP(Guid sepId)
    {
        WWW www = new WWW(string.Format(GET_SEP, sepId));
        while (!www.isDone) { }
        return int.Parse(www.text);
    }

    public static int IncrementSEP(Guid sepId, int increment)
    {
        WWW www = new WWW(string.Format(INCREMENT_SEP, sepId, increment));
        while (!www.isDone) { }
        return int.Parse(www.text);
    }
}


