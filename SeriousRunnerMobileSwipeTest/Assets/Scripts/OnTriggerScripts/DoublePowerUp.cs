﻿using UnityEngine;
using System.Collections;

public class DoublePowerUp : MonoBehaviour
{
    // Use this for initialization
    private PlayerCon2 playerCon;
    // Use this for initialization
    void Start()
    {
        playerCon = GameObject.Find("SeriousRunnerGirl").GetComponent<PlayerCon2>();
    }

    void OnTriggerEnter()
    {
        playerCon.isDoubleBoost = true;
        Destroy(gameObject);
        playerCon.doubleInstance = (GameObject)Instantiate(playerCon.doublePrefab, new Vector3(playerCon.transform.position.x, playerCon.transform.position.y, playerCon.transform.position.z), Quaternion.identity);
    }
}