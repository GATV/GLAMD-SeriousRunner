using UnityEngine;
using System.Collections;

public class BarricadeRight : MonoBehaviour {
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
        Debug.Log("Hit trigger");
        Vector3 turningVectorValue;

        if (playerCon.isAllowedTurn// & playerCon.turnDirectionAllowed != "Right")
            )
        {
            playerCon.currentDirection = playerCon.GetDirection(true);
            if (playerCon.turnRotationValue - 90 < -350)
            {
                playerCon.turnRotationValue = 0.0f;
            }
            else
            {
                playerCon.turnRotationValue -= 90.0f;
            }

            turningVectorValue = new Vector3(0, playerCon.turnRotationValue, 0);
            iTween.RotateTo(player.gameObject, turningVectorValue, 0.5f);
            playerCon.isAllowedTurn = false;
            playerCon.TurnMade("Left");

            playerCon.xPosition = playerCon.transform.position.x;
            playerCon.yPosition = playerCon.transform.position.z;
        }
    }
}
