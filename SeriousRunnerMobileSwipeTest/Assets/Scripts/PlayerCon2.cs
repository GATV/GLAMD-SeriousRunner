using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerCon2 : MonoBehaviour
{
    public AudioSource audio;

    //Movement
    public Direction currentDirection;
    public float speed;
    public float rotateSpeed;
    public Lane currentLane;
    public float laneDistance;

    //newwww
    public bool isAllowedTurn;
    public float setSpeed;

    //new
    public float xPosition;
    public float yPosition;

    public CameraCon2 cameraControls;
    //newwww
    public float turnRotationValue;
    private Vector3 turningVectorValue;

    //Mobile controls
    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;
    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;


    //Jumping
    public float jumpTime;
    public float jumpHeight;

    private float jumpTimer;
    private bool switchable;

    //Nieuw toegevoegd    
    private float gameTime;
    public bool finished;

    //power-ups
    //shield
    public GameObject shieldPrefab;
    public GameObject shieldInstance;

    public float invincibleTimer;
    public bool isInvincible;
    private float invincibleTime;
    //speed
    public float speedBoostTime;
    private float speedTimer;
    //double
    public float doublePowerUpTime;
    public GameObject doublePrefab;
    public GameObject doubleInstance;
    public bool isDoubleBoost;
    private float doublePowerUpTimer;

    //Obstacles
    public GameObject barricadeRight;
    public GameObject barricadeLeft;

    //Random
    public Vector3 turningPosition;
    public Turn? turnDirectionAllowed;
    public int positionTurnFix;
    private bool paused = false;

    private Dictionary<Direction, Vector3> directionMovements;
    public Animator animator;

    //UI
    public Text countText;
    public Text winText;
    public Text timeText;
    public Text scoreText;
    private float seconds;
    private float minutes;
    private float score;
    public int count;

    public FadeInAndOut FinishPanel;

    public Button ButtonPause;

    public Text pauseButtonText;
    public bool ButtonLeftTurnClicked;
    public bool ButtonRightTurnClicked;

    // Use this for initialization
    private CharacterController controller;

    void Start()
    {
        float turnRotationValue = gameObject.transform.rotation.y;

        audio = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
        directionMovements = new Dictionary<Direction, Vector3>()
        {
            { Direction.North, Vector3.forward },
            { Direction.East, Vector3.right },
            { Direction.South, Vector3.back },
            { Direction.West, Vector3.left }
    };

        controller = GetComponent<CharacterController>();
        count = 0;
        SetCountText();
        winText.text = "";
        animator = GetComponent<Animator>();
        setSpeed = speed;
        jumpTimer = jumpTime;
        switchable = true;
        isDoubleBoost = false;
        seconds = 0;
        minutes = 0;

        pauseButtonText.text = "";

        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    //Update is called once per frame
    void Update()
    {
        //Mobile controls
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;

                    case TouchPhase.Ended:

                        float gestureTime = Time.time - fingerStartTime;
                        float gestureDist = (touch.position - fingerStartPos).magnitude;
                        if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                        {
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeType = Vector2.zero;

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                //horizontal swipe
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else
                            {
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f)
                            {
                                if (swipeType.x > 0.0f)
                                {
                                    //right
                                    if (currentLane < Lane.Right && switchable && !paused)
                                    {
                                        currentLane++;
                                        //controller.Move(directionMovements[GetDirection(Turn.Right)] * laneDistance);

                                        //new
                                        switch (currentDirection)
                                        {
                                            case Direction.North:
                                                xPosition += 2;
                                                break;
                                            case Direction.East:
                                                yPosition -= 2;
                                                break;
                                            case Direction.West:
                                                yPosition += 2;
                                                break;
                                            case Direction.South:
                                                xPosition -= 2;
                                                break;
                                        }


                                    }
                                }
                                else
                                {
                                    //left
                                    if (currentLane > Lane.Left && !paused && switchable)
                                    {
                                        currentLane--;
                                        //controller.Move(directionMovements[GetDirection(Turn.Left)] * laneDistance);
                                        //Nieuw
                                        switch (currentDirection)
                                        {
                                            case Direction.North:
                                                xPosition -= 2;
                                                break;
                                            case Direction.East:
                                                yPosition += 2;
                                                break;
                                            case Direction.West:
                                                yPosition -= 2;
                                                break;
                                            case Direction.South:
                                                xPosition += 2;
                                                break;
                                        }

                                    }
                                }
                            }

                            if (swipeType.y != 0.0f)
                            {
                                if (swipeType.y > 0.0f)
                                {
                                    //jump
                                    if (transform.position.y < 0.2 && !paused)
                                    {
                                        jumpTimer = 0.0f;
                                        switchable = false;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        //PC controls
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            buttonPauseClick();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > Lane.Left && !paused && switchable)
        {
            currentLane--;
            //controller.Move(directionMovements[GetDirection(Turn.Left)] * laneDistance);
            //rb.transform.position += GetLaneDirection(true) * laneDistance;

            //Nieuw            
            switch (currentDirection)
            {
                case Direction.North:
                    xPosition -= 2;
                    break;
                case Direction.East:
                    yPosition += 2;
                    break;
                case Direction.West:
                    yPosition -= 2;
                    break;
                case Direction.South:
                    xPosition += 2;
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < Lane.Right && !paused && switchable)
        {
            currentLane++;
            //controller.Move(directionMovements[GetDirection(Turn.Right)] * laneDistance);

            // rb.transform.position += GetLaneDirection(false) * laneDistance;

            //Nieuw
            switch (currentDirection)
            {
                case Direction.North:
                    xPosition += 2;
                    break;
                case Direction.East:
                    yPosition -= 2;
                    break;
                case Direction.West:
                    yPosition += 2;
                    break;
                case Direction.South:
                    xPosition -= 2;
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && isAllowedTurn && turnDirectionAllowed != Turn.Left)
        {
            if (turnRotationValue + 90 > 350)
                turnRotationValue = 0.0f;
            else
                turnRotationValue += 90.0f;

            turningVectorValue = new Vector3(0, turnRotationValue, 0);
            iTween.RotateTo(gameObject, turningVectorValue, 1.0f);
            currentDirection = GetDirection(Turn.Right);
            isAllowedTurn = false;
            TurnMade(Turn.Right);
            //Nieuw
            xPosition = transform.position.x;
            yPosition = transform.position.z;
        }
        else if (Input.GetKeyDown(KeyCode.A) && isAllowedTurn && turnDirectionAllowed != Turn.Right)
        {
            if (turnRotationValue - 90 < -350)
                turnRotationValue = 0.0f;
            else
                turnRotationValue -= 90.0f;

            turningVectorValue = new Vector3(0, turnRotationValue, 0);
            iTween.RotateTo(gameObject, turningVectorValue, 1.0f);
            //transform.Rotate(0, -90, 0);    
            currentDirection = GetDirection(Turn.Left);
            isAllowedTurn = false;
            TurnMade(Turn.Left);
            //Nieuw
            xPosition = transform.position.x;
            yPosition = transform.position.z;

        }

        //Nieuw mobile
        if (ButtonLeftTurnClicked && isAllowedTurn && turnDirectionAllowed != Turn.Right && !paused)
        {
            currentDirection = GetDirection(Turn.Left);
            if (turnRotationValue - 90 < -350)
                turnRotationValue = 0.0f;
            else
                turnRotationValue -= 90.0f;

            turningVectorValue = new Vector3(0, turnRotationValue, 0);
            iTween.RotateTo(gameObject, turningVectorValue, 1.0f);
            //iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .01));
            isAllowedTurn = false;
            TurnMade(Turn.Left);

            //Nieuw
            xPosition = transform.position.x;
            yPosition = transform.position.z;

            ButtonLeftTurnClicked = false;
        }

        if (ButtonRightTurnClicked && isAllowedTurn && turnDirectionAllowed != Turn.Left && !paused)
        {
            currentDirection = GetDirection(Turn.Right);
            // iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .01));
            //transform.Rotate(0, 90, 0);
            if (turnRotationValue + 90 > 350)
                turnRotationValue = 0.0f;
            else
                turnRotationValue += 90.0f;

            turningVectorValue = new Vector3(0, turnRotationValue, 0);
            iTween.RotateTo(gameObject, turningVectorValue, 1.0f);

            isAllowedTurn = false;

            TurnMade(Turn.Right);

            //Nieuw
            xPosition = transform.position.x;
            yPosition = transform.position.z;

            ButtonRightTurnClicked = false;
        }



        //Collisions
        if (speed < setSpeed && !finished)
        {
            //Debug.Log(speed.ToString());
            speed += Time.deltaTime;
            if (speed > setSpeed)
            {
                speed = setSpeed;
            }
        }

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) & transform.position.y < 0.2 && !paused)
        {
            //animator.Play("Jump");
            jumpTimer = 0.0f;
            switchable = false;
        }
        else if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            }
        }
        if (jumpTimer < jumpTime)
        {
            //Debug.Log(jumpTimer);
            jumpTimer = Math.Min(jumpTimer + Time.deltaTime, jumpTime);

            float blend = jumpTimer / jumpTime;

            Vector3 pos = new Vector3();
            pos.y += (float)Math.Sin(blend * Math.PI) * jumpHeight;
            transform.position = new Vector3(transform.position.x, pos.y, transform.position.z);
        }
        else
        {
            switchable = true;
        }

        controller.Move(directionMovements[currentDirection] * speed * Time.deltaTime);

        //Nieuw
        Vector3 posLaneSwitch = transform.position;

        if (currentDirection == Direction.North | currentDirection == Direction.South)
        {
            posLaneSwitch.x = Mathf.MoveTowards(posLaneSwitch.x, xPosition, speed * Time.deltaTime);
        }
        else
        {
            posLaneSwitch.z = Mathf.MoveTowards(posLaneSwitch.z, yPosition, speed * Time.deltaTime);
        }

        transform.position = posLaneSwitch;

        animator.SetFloat("Speed", speed);

        //Timetracking
        if (!finished)
        {
            //gameTime += Time.deltaTime;

            //var minutes = gameTime / 60;
            //var seconds = gameTime % 60;
            //var fraction = (gameTime * 100) % 100;

            //timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

            //adding seconds
            seconds += Time.deltaTime;
            //adding minutes
            if (Mathf.Floor(seconds) >= 60)
            {
                seconds = 0;
                minutes++;
            }
            timeText.text = string.Format("{0:00} : {1:00}", minutes, Mathf.Floor(seconds));
        }

        //Speed power-up
        if (speed > setSpeed)
        {
            speedTimer += Time.deltaTime;
            if (speedTimer >= speedBoostTime)
            {
                speed = setSpeed;
                speedTimer = 0;
            }
        }

        //Shield power-up
        if (isInvincible)
        {
            invincibleTime += Time.deltaTime;
            shieldInstance.transform.position = new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z);
            if (invincibleTime >= invincibleTimer)
            {
                isInvincible = false;
                Destroy(shieldInstance);
                invincibleTime = 0;
            }
        }

        //Double power-up
        if (isDoubleBoost)
        {
            doublePowerUpTimer += Time.deltaTime;
            doubleInstance.transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
            if (doublePowerUpTimer >= doublePowerUpTime)
            {
                isDoubleBoost = false;
                Destroy(doubleInstance);
                doublePowerUpTimer = 0;
            }
        }
    }

    public void RestartGameButtonClick()
    {
        SceneManager.LoadScene("SeriousRunnerTest");
    }

    public void buttonPauseClick()
    {
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1f;
            paused = false;
            pauseButtonText.text = "";
        }
        else
        {
            Time.timeScale = 0.0f;
            paused = true;
            pauseButtonText.text = "Game is paused";
        }
    }

    public void TurnLeft()
    {
        ButtonLeftTurnClicked = true;
    }

    public void TurnRight()
    {
        ButtonRightTurnClicked = true;
    }

    public Direction GetDirection(Turn t)
    {
        return (Direction)(((int)currentDirection + (t == Turn.Left ? 3 : 1)) % 4);
    }

    //bool togglePause()
    //{
    //    if (Time.timeScale == 0.0f)
    //    {
    //        Time.timeScale = 1f;
    //        return false;
    //    }
    //    else
    //    {
    //        Time.timeScale = 0.0f;
    //        return true;
    //    }
    //}

    void OnGUI()
    {
        //Pausing
        //if (paused)
        //{
        //    GUI.Label(new Rect(7, 60, 300, 40), "Game is Paused! Press esc");
        //    //if (GUI.Button(new Rect(0, 0, 80, 40"Click me to unpause")))
        //    //{
        //    //    paused = togglePause();
        //    //}
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        //Finish
        if (other.gameObject.CompareTag("End"))
        {
            winText.text = "Finish";
            speed = 0.0f;
            animator.Play("Wary");
            finished = true;
            FinishPanel.FadeIn();
            score = (500 - ((minutes * 60) + Mathf.Floor(seconds))) + (count * 2);
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void SetCountText()
    {
        countText.text = "Coins: " + count.ToString();
    }

    public void TurnMade(Turn t)
    {
        if (t == Turn.Left || t == Turn.Splits)
        {
            if (currentDirection == Direction.West)
            {
                if (transform.position.z - turningPosition.z < 2.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix + 1);
                    currentLane = Lane.Left;
                }
                else if (transform.position.z - turningPosition.z < 4.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix + 3);
                    currentLane = Lane.Middle;
                }
                else
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix + 5);
                    currentLane = Lane.Right;
                }
            }
            if (currentDirection == Direction.North)
            {
                if (transform.position.x - turningPosition.x < 2.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix + 1, transform.position.y, transform.position.z);
                    currentLane = Lane.Left;
                }
                else if (transform.position.x - turningPosition.x < 4.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix + 3, transform.position.y, transform.position.z);
                    currentLane = Lane.Middle;
                }
                else
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix + 5, transform.position.y, transform.position.z);
                    currentLane = Lane.Right;
                }
            }
            if (currentDirection == Direction.South)
            {
                //-18 ~-19.99   -  -18      
                if (transform.position.x - turningPosition.x > -2.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix - 1, transform.position.y, transform.position.z);
                    currentLane = Lane.Left;
                }
                else if (transform.position.x - turningPosition.x > -4.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix - 3, transform.position.y, transform.position.z);
                    currentLane = Lane.Middle;
                }
                else
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix - 5, transform.position.y, transform.position.z);
                    currentLane = Lane.Right;
                }
            }
            if (currentDirection == Direction.East)
            {
                if (transform.position.z - turningPosition.z > -2.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix - 1);
                    currentLane = Lane.Left;
                }
                else if (transform.position.z - turningPosition.z > -4.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix - 3);
                    currentLane = Lane.Middle;
                }
                else
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix - 5);
                    currentLane = Lane.Right;
                }
            }
        }
        else if (t == Turn.Right || t == Turn.Splits)
        {
            if (currentDirection == Direction.East)
            {
                if (transform.position.z - turningPosition.z > 4.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix + 5);
                    currentLane = Lane.Left;
                }
                else if (transform.position.z - turningPosition.z > 2.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix + 3);
                    currentLane = Lane.Middle;
                }
                else
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix + 1);
                    currentLane = Lane.Right;
                }
            }
            if (currentDirection == Direction.North)
            {
                if (transform.position.x - turningPosition.x < -4.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix - 5, transform.position.y, transform.position.z);
                    currentLane = Lane.Left;
                }
                else if (transform.position.x - turningPosition.x < -2.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix - 3, transform.position.y, transform.position.z);
                    currentLane = Lane.Middle;
                }
                else
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix - 1, transform.position.y, transform.position.z);
                    currentLane = Lane.Right;
                }
            }
            if (currentDirection == Direction.South)
            {
                if (transform.position.x - turningPosition.x > 4.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix + 5, transform.position.y, transform.position.z);
                    currentLane = Lane.Left;
                }
                else if (transform.position.x - turningPosition.x > 2.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix + 3, transform.position.y, transform.position.z);
                    currentLane = Lane.Middle;
                }
                else
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.x);
                    transform.position = new Vector3(positionTurnFix + 1, transform.position.y, transform.position.z);
                    currentLane = Lane.Right;
                }
            }
            if (currentDirection == Direction.West)
            {
                if (transform.position.z - turningPosition.z > -4.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix - 5);
                    currentLane = Lane.Left;
                }
                else if (transform.position.z - turningPosition.z > -2.0f)
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix - 3);
                    currentLane = Lane.Middle;
                }
                else
                {
                    positionTurnFix = Convert.ToInt32(turningPosition.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix - 1);
                    currentLane = Lane.Right;
                }
            }
        }
    }
}