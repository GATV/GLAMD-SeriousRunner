using UnityEngine;
using System.Collections;

public class TriggerLeft2 : MonoBehaviour {
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
        Instantiate(playerCon.barricadeRight, new Vector3(45f, 0f, 198.5f), new Quaternion(0f, 0f, 0f, 0f));
    }
}
