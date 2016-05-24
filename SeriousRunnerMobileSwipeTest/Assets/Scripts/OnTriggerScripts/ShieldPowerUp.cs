using UnityEngine;
using System.Collections;

public class ShieldPowerUp : MonoBehaviour {
    private PlayerCon2 playerCon;
    // Use this for initialization
    void Start () {
        playerCon = GameObject.Find("SeriousRunnerGirl").GetComponent<PlayerCon2>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        playerCon.isInvincible = true;
        Destroy(gameObject);
        playerCon.shieldInstance = (GameObject)Instantiate(playerCon.shieldPrefab, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
    }
}
