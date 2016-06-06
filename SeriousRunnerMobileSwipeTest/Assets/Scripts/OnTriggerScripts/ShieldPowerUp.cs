﻿using UnityEngine;
using System.Collections;

public class ShieldPowerUp : MonoBehaviour
{
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
        if (playerCon.shieldInstance != null)
        {
            playerCon.invincibleTimer = 0;
            Destroy(playerCon.shieldInstance);
        }
        playerCon.isInvincible = true;
        Destroy(gameObject);
        playerCon.shieldInstance = (GameObject)Instantiate(playerCon.shieldPrefab, new Vector3(playerCon.transform.position.x, playerCon.transform.position.y, playerCon.transform.position.z), Quaternion.identity);
    }
}
