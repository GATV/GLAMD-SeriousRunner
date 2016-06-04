using UnityEngine;
using System.Collections;

public class CoinDouble : MonoBehaviour
{
    public GameObject player;
    private PlayerCon2 playerCon;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("SeriousRunnerGirl");
        playerCon = player.GetComponent<PlayerCon2>();
    }

    void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
