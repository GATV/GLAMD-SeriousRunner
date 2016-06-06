using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
    private PlayerCon2 playerCon;
    // Use this for initialization
    void Start () {
        playerCon = GameObject.Find("SeriousRunnerGirl").GetComponent<PlayerCon2>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        if(!playerCon.isInvincible)
        {
            playerCon.speed = 1;
            playerCon.animator.Play("Damage");
        }
        gameObject.SetActive(false);
    }
}
