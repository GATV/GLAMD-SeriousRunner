using UnityEngine;
using System.Collections;

public class TriggerBarricadeRight : MonoBehaviour
{
  private PlayerCon2 playerCon;
  public GameObject splitsingBlock;
  public int layerToChangeTo;

  public Transform barricadeMount;
  void Start()
  {
    playerCon = GameObject.Find("SeriousRunnerGirl").GetComponent<PlayerCon2>();
  }

  void OnTriggerEnter()
  {
    splitsingBlock.layer = layerToChangeTo;
    Instantiate(playerCon.barricadeRight, barricadeMount.position, barricadeMount.rotation);
  }
}
