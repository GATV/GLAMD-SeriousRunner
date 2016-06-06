﻿using UnityEngine;
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
            Vector3 turningVectorValue;
            if (playerCon.turnDirectionAllowed == Turn.Splits)
            {
                int leftOrRight = UnityEngine.Random.Range(0, 1);
                

                if (leftOrRight == 0)
                {
                    playerCon.currentDirection = playerCon.GetDirection(Turn.Right);
                    if (playerCon.turnRotationValue + 90 > 350)
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
                    playerCon.TurnMade(Turn.Right);

                    playerCon.xPosition = playerCon.transform.position.x;
                    playerCon.yPosition = playerCon.transform.position.z;
                }
                else
                {
                    playerCon.currentDirection = playerCon.GetDirection(Turn.Left);
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
                    playerCon.TurnMade(Turn.Left);

                    playerCon.xPosition = playerCon.transform.position.x;
                    playerCon.yPosition = playerCon.transform.position.z;
                }
            }
            else if(playerCon.turnDirectionAllowed == Turn.Left)
            {
                playerCon.currentDirection = playerCon.GetDirection(Turn.Left);
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
                playerCon.TurnMade(Turn.Left);

                playerCon.xPosition = playerCon.transform.position.x;
                playerCon.yPosition = playerCon.transform.position.z;
            }
            else
            {
                playerCon.currentDirection = playerCon.GetDirection(Turn.Right);
                if (playerCon.turnRotationValue + 90 > 350)
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
                playerCon.TurnMade(Turn.Right);

                playerCon.xPosition = playerCon.transform.position.x;
                playerCon.yPosition = playerCon.transform.position.z;
            }
        }
    }
}
