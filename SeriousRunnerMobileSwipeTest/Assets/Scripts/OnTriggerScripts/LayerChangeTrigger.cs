using UnityEngine;
using System.Collections;

public class LayerChangeTrigger : MonoBehaviour {
  public GameObject splitsingBlock;
  public int layerToChangeTo;

  void OnTriggerEnter()
  {
    splitsingBlock.layer = layerToChangeTo;

  }
}
