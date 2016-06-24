using UnityEngine;
using System.Collections;

public class MPScript : MonoBehaviour {

    public static MPScript Data { get; private set; }

    public int? Seed { get; set; }
    public string ReplayData { get; set; }

    void Awake()
    {
        if (Data == null)
            Data = this;
        else if (Data != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

	public void UpdateMultiplayerMenu()
    {
        ScrollviewMatches.UpdateList();
    }
}
