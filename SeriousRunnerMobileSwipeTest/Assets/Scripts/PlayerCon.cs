using UnityEngine;
using System.Collections;

public class PlayerCon : MonoBehaviour
{

    private CharacterController controller;
    private float speed = 5.0f;
    private Vector3 lastPosition;

    private float animationDuration = 3.0f;

    Animator anim;


    private int lane;

    bool paused = false;


    //Moving 
    private Vector3 moveVector;

    private float verticalVelocity = 0.0f;
    private float gravity = 2.0f;

    // Use this for initialization
    void Start()
    {
        lane = 0;

        lastPosition = gameObject.transform.position;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = togglePause();
        }

        Vector3 newPosition = transform.position;
        newPosition.x = lane;
        transform.position = newPosition;

        if (lane == 0)
        {
            newPosition.x = 0;
        }

        if (Time.time < animationDuration)
        {

            return;
        }
        moveVector = Vector3.zero;
        //X - Left and right

        if (Input.GetKeyDown(KeyCode.A) && !paused)
        {
            if (lane > -1)
            {
                lane--;
            }
        }
        if (Input.GetKeyDown(KeyCode.D) && !paused)
        {
            if (lane < 1)
            {
                lane++;
            }
        }


        //Y - Up and Down
        moveVector.y = verticalVelocity;

        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        //Z - Forward and Backward
        moveVector.z = speed;


        controller.Move(moveVector * Time.deltaTime);


        if (lastPosition != gameObject.transform.position)
        {
            anim.SetFloat("Speed", speed);

        }

        lastPosition = gameObject.transform.position;


    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }

    void OnGUI()
    {
        if (paused)
        {
            GUILayout.Label("Game is paused!");
            if (GUILayout.Button("Click me to unpause"))
                paused = togglePause();
        }
    }

}
