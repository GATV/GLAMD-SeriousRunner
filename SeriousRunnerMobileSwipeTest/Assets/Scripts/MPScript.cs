using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using System;
using GameUp;
using Match = Assets.Scripts.Match;

public class MPScript : MonoBehaviour
{

    public static MPScript Data { get; private set; }

    public bool SkipLogin { get; set; }
    public string ReplayData { get; set; }
    public SessionClient SessionClient { get; set; }
    public Match Match { get; set; }

    public void Clean()
    {
        GlobalRandom.Seed = DateTime.Now.Millisecond;
        ReplayData = null;
        Match = null;
    }

    void Awake()
    {
        if (Data == null)
            Data = this;
        else if (Data != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
