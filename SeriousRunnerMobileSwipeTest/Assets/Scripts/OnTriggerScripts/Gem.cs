using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour
{
    public GameObject player;
    private PlayerCon2 playerCon;
    public AudioClip coin;

    public AudioSource audio;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("SeriousRunnerGirl");
        playerCon = player.GetComponent<PlayerCon2>();
        audio = player.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        audio.PlayOneShot(coin);
        gameObject.SetActive(false);
        if (!playerCon.isDoubleBoost)
        {
            playerCon.coins = playerCon.coins + 5;
        }
        else playerCon.coins = playerCon.coins + 10;
        playerCon.SetCoinText();
    }
}

