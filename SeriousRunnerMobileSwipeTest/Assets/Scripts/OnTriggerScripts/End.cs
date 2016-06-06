using UnityEngine;
using System.Collections;

public class End : MonoBehaviour {
    private PlayerCon2 playerCon;
    // Use this for initialization
    void Start ()
    {
        playerCon = GameObject.Find("SeriousRunnerGirl").GetComponent<PlayerCon2>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter()
    {
        playerCon.speed = 0.0f;
        playerCon.animator.Play("Wary");
        playerCon.finished = true;
    }
}
