using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
    private GameObject player;
    private PlayerCon2 playerCon;
    public AudioClip damageFX;

    public AudioSource audio;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("SeriousRunnerGirl");
        playerCon = player.GetComponent<PlayerCon2>();
        audio = player.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter()
    {
        audio.PlayOneShot(damageFX);
        if(!playerCon.isInvincible)
        {
            playerCon.speed = 1;
            playerCon.animator.Play("Damage");
        }
        gameObject.SetActive(false);
    }
}
