using UnityEngine;
using System.Collections;

public class LaneTriggerExit : MonoBehaviour {
    private GameObject player;
    private PlayerCon2 playerCon;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("SeriousRunnerGirl");
        playerCon = player.GetComponent<PlayerCon2>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        playerCon.turningPosition = player.transform.position;
        playerCon.isAllowedTurn = true;
        gameObject.SetActive(false);
        if(gameObject.layer == 8)
        {
            playerCon.turnDirectionAllowed = "Right";
        }
        else if(gameObject.layer == 9)
        {
            playerCon.turnDirectionAllowed = "Left";
        }
        else
        {
            playerCon.turnDirectionAllowed = "Splits";
        }
    }
}
