using UnityEngine;
using System.Collections;

public class TriggerLeft1 : MonoBehaviour {
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
        Instantiate(playerCon.barricadeRight, new Vector3(3f, 0f, 77.5f), new Quaternion(0f, 180f, 0f, 0f));
    }
}
