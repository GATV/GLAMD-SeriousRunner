using UnityEngine;
using System.Collections;

public class CameraCon2 : MonoBehaviour
{

  private Transform playerPosition;
  private Vector3 startOffset;
  private Vector3 moveVector;
    public float springForce;
    public float springDamping;

  // Use this for initialization
  void Start()
  {
    playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        startOffset = transform.position;
        //startOffset = transform.position - playerPosition.position;
    }

  // Update is called once per frame
  void Update()
  {
        //moveVector = playerPosition.position + startOffset;

        //x


        //y


        //z
        //transform.position = playerPosition.TransformPoint(startOffset);
        Vector3 vel = this.GetComponent<Rigidbody>().velocity;
        Vector3 currentPos = transform.position;
        Vector3 desiredPos = //GameObject.FindGameObjectWithTag("Player").transform.position + startOffset;
            GameObject.FindGameObjectWithTag("Player").transform.TransformPoint(startOffset);
        Vector3 deltaPos = desiredPos - currentPos;

        Vector3 force = (deltaPos * springForce) - (vel * springDamping);

        this.GetComponent<Rigidbody>().AddForce(force);
        transform.rotation = playerPosition.rotation * Quaternion.Euler(25.0f, 0.0f, 0.0f);
  }

}

