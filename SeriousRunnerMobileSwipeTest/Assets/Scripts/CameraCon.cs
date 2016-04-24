using UnityEngine;
using System.Collections;

public class CameraCon : MonoBehaviour
{

    private Transform lookAt;
    private Vector3 startOffset;

    private Vector3 moveVector;
    private Space offsetPositionSpace = Space.Self;

    private float transition = 0.0f;
    //private float animationDuration = 3.0f;
    //private Vector3 animationOffset = new Vector3(0.0f, 5.0f, 5.0f);

    // Use this for initialization
    void Start()
    {
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        startOffset = transform.position - lookAt.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = lookAt.position + startOffset;

        //X
        moveVector.x = 0.0f;

        //Y
        moveVector.y = Mathf.Clamp(moveVector.y, 3, 5);

        //Z

        if (transition > 1.0f && offsetPositionSpace == Space.Self)
        {
            transform.position = lookAt.TransformPoint(startOffset);
            transform.LookAt(lookAt.position + Vector3.down);
        }
        else
        {
            //idle
            //transform.position = Vector3.Lerp(moveVector + animationOffset, moveVector, transition);
            //transition += Time.deltaTime * 1 / animationDuration;

            //transform.LookAt(lookAt.position + Vector3.up + Vector3.up);

            transform.position = lookAt.TransformPoint(startOffset);
            transform.rotation = lookAt.rotation;

        }

    }
}


