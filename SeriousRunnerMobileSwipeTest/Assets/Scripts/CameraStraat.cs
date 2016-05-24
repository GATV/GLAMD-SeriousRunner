using UnityEngine;
using System.Collections;

public class CameraStraat : MonoBehaviour {

//  float rotationSpeedY;

	// Use this for initialization
	void Start () {
  //  rotationSpeedY = 160.0f;
	}
	
	// Update is called once per frame
	void Update () {
    if (transform.position.z < -1)
    {
      transform.position = (transform.position + new Vector3(0, 0, 0.5f)) * Time.deltaTime;
    }

	}
}
