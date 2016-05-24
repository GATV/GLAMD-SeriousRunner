using UnityEngine;
using System.Collections;

public class TurningDirectionAllowed : MonoBehaviour {

  public int turningDirection;

	// Use this for initialization
	void Start () {
    if (gameObject.layer.Equals("Right"))
    {
      turningDirection = 2;
    }
    else if (gameObject.layer.Equals("Left"))
    {
      turningDirection = 1;
    }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
