using UnityEngine;
using System.Collections;

public class CameraCon2 : MonoBehaviour {

  private Transform playerPosition;
  private Vector3 startOffset;
  private Vector3 moveVector;
  bool cameraFollowPlayer;

  private float transition = 0.0f;
  private float animationDuration = 3.0f;

  // Use this for initialization
  void Start()
  {
    playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    startOffset = transform.position;
    cameraFollowPlayer = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (playerPosition.position.z > -4)
    {
      cameraFollowPlayer = true;
    }
    
    if (cameraFollowPlayer)
    {
      moveVector = playerPosition.position + startOffset;

      //x


      //y


      //z
      transform.position = playerPosition.TransformPoint(startOffset);
      transform.rotation = playerPosition.rotation * Quaternion.Euler(25.0f, 0.0f, 0.0f);
    }

  }

}
