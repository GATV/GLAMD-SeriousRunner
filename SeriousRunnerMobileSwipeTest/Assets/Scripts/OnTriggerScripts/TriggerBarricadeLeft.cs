using UnityEngine;
using System.Collections;

public class TriggerBarricadeLeft : MonoBehaviour {
  private PlayerCon2 playerCon;
  public GameObject splitsingBlockTrigger;
  public int layerToChangeTo;

  public Transform barricadeMount;
  void Start()
  {
    playerCon = GameObject.Find("SeriousRunnerGirl").GetComponent<PlayerCon2>();
  }

  void OnTriggerEnter()
  {
    splitsingBlockTrigger.layer = layerToChangeTo;
    Instantiate(playerCon.barricadeLeft, barricadeMount.position, barricadeMount.rotation);
  }
}
