using UnityEngine;
using System.Collections;

public class SidePiece3 : MonoBehaviour {
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
        if(playerCon.isAllowedTurn)
        {
            int leftOrRight = UnityEngine.Random.Range(0, 1);
            Vector3 turningVectorValue;

            if (leftOrRight == 0)
            {
                playerCon.currentDirection = playerCon.GetDirection(false);                
                if(playerCon.turnRotationValue + 90 > 350)
                {
                    playerCon.turnRotationValue = 0.0f;
                }
                else
                {
                    playerCon.turnRotationValue += 90.0f;
                }

                turningVectorValue = new Vector3(0, playerCon.turnRotationValue, 0);
                iTween.RotateTo(player.gameObject, turningVectorValue, 0.5f);
                playerCon.isAllowedTurn = false;
                playerCon.TurnMade("Right");

                playerCon.xPosition = playerCon.transform.position.x;
                playerCon.yPosition = playerCon.transform.position.z;
            }
            else
            {
                playerCon.currentDirection = playerCon.GetDirection(true);
                if(playerCon.turnRotationValue - 90 < -350)
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
}
