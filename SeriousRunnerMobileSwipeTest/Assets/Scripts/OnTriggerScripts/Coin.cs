using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private GameObject player;
    private PlayerCon2 playerCon;
    public AudioClip coin;
    public AudioSource audiosourcePlayer;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("SeriousRunnerGirl");
        playerCon = player.GetComponent<PlayerCon2>();
        audiosourcePlayer = player.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (audiosourcePlayer != null)
        {
            audiosourcePlayer.PlayOneShot(coin);
            Destroy(gameObject);
            if (playerCon.isDoubleBoost)
            {
                playerCon.coins += 2;
            }
            else playerCon.coins++;
            {
                //playerCon.coinInstance = (GameObject)Instantiate(playerCon.coinPrefab, playerCon.getPlayerPos(), Quaternion.identity);            
            }
            playerCon.SetCoinText();
        } 
    }
}