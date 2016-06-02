using UnityEngine;
using System.Collections;

public class CameraCon2 : MonoBehaviour
{

  private Transform playerPosition;
  private Vector3 startOffset;
  //private Vector3 moveVector;
  private Vector3 ShakeAmount;

  // Use this for initialization
  void Start()
  {
    ShakeAmount = new Vector3(0.0f, 0.2f, 0.0f);
    playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    startOffset = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
   // moveVector = playerPosition.position + startOffset;

    //x


    //y


    //z
    transform.position = playerPosition.TransformPoint(startOffset);
    transform.rotation = playerPosition.rotation * Quaternion.Euler(15.0f, 0.0f, 0.0f);
  }

  public void ShakeCamera()
  {
    iTween.ShakePosition(gameObject, ShakeAmount, 0.15f);
  }

}

