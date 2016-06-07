using UnityEngine;
using System.Collections;

public class LightsInstantiate : MonoBehaviour {
  public GameObject Lightobject;
  // Use this for initialization
  void Start()
  {  
    Instantiate();

    GameObject.Find("/DirectionalLight");

  }

  void Instantiate()
  {
    Instantiate(Lightobject);
  }
}
