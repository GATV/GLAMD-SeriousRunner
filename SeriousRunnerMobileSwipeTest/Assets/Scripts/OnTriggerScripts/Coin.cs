using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private GameObject player;
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
        Destroy(gameObject);
        if (playerCon.isDoubleBoost)
        {
            playerCon.count += 2;
        }
        else playerCon.count++;
        playerCon.coinInstance = (GameObject)Instantiate(playerCon.coinPrefab, playerCon.getPlayerPos(), Quaternion.identity);
        playerCon.SetCountText();
    }
}