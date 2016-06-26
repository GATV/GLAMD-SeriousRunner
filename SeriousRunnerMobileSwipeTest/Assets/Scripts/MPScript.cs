using UnityEngine;
using System.Collections;
using GameUp;

public class MPScript : MonoBehaviour {

    public static MPScript Data { get; private set; }

    public  bool SkipLogin { get; set; }
    public int? Seed { get; set; }
    public string ReplayData { get; set; }
    public SessionClient SessionClient { get; set; }

    void Awake()
    {
        if (Data == null)
            Data = this;
        else if (Data != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
