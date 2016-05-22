using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerCon2 : MonoBehaviour
{
  AudioSource audio;
  //Movement
  public Direction currentDirection;
  public float speed;
  public float rotateSpeed;
  public Lane currentLane;
  public float laneDistance;

    //Nieuw
    //private bool isAllowedTurn;
    public bool isAllowedTurn;
    //private float setSpeed;
    public float setSpeed;


    //new
    //Nieuw
    //private float xPosition;
    //private float yPosition;
    public float xPosition;
    public float yPosition;

    //Nieuw
    //private float turnRotationValue;
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
    //Nieuw
    //private bool finished;
    public bool finished;

  //Used for power-ups
  public float speedBoostTime;
  public GameObject shieldPrefab;
  public float invincibleTimer;

    //Nieuw
    //private bool isInvincible;
    public bool isInvincible;
  private float invincibleTime;
  private GameObject shieldInstance;
  private float speedTimer;

  //Obstacles
  public GameObject barricadeRight;
  public GameObject barricadeLeft;

  //Random
  public Vector3 turningPosition;
  public string turnDirectionAllowed;
  public int positionTurnFix;
  private bool paused = false;

  private Dictionary<Direction, Vector3> directionMovements;
    //Nieuw
    //private Animator animator;
    public Animator animator;
  

  //UI
  public Text countText;
  public Text winText;
  public Text timeText;

  private int count;
    public int coinCount;

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
        //Nieuw
        coinCount = 0;
    SetCountText();
    winText.text = "";
    animator = GetComponent<Animator>();
    setSpeed = speed;
    jumpTimer = jumpTime;
    switchable = true;




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
                    //controller.Move(directionMovements[GetDirection(false)] * laneDistance);

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
                  if (currentLane > Lane.Left && !paused && switchable )
                  {
                    currentLane--;
                    //controller.Move(directionMovements[GetDirection(true)] * laneDistance);
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
                  if (transform.position.y < 0.2 && !paused )
                  {
                    jumpTimer = 0.0f;
                    switchable = false;
                  }
                }
                else
                {
                  //sneak? crouch?
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

    if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > Lane.Left && !paused && switchable )
    {
      currentLane--;
      //controller.Move(directionMovements[GetDirection(true)] * laneDistance);
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
    else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < Lane.Right && !paused && switchable )
    {
      currentLane++;
      //controller.Move(directionMovements[GetDirection(false)] * laneDistance);

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

    if (Input.GetKeyDown(KeyCode.D) && isAllowedTurn && turnDirectionAllowed != "Left")
    {
      if (turnRotationValue + 90 > 350)
        turnRotationValue = 0.0f;
      else
        turnRotationValue += 90.0f;


      turningVectorValue = new Vector3(0, turnRotationValue, 0);
      iTween.RotateTo(gameObject, turningVectorValue, 0.5f);
      currentDirection = GetDirection(false);
      isAllowedTurn = false;
      TurnMade("Right");
      //Nieuw
      xPosition = transform.position.x;
      yPosition = transform.position.z;
    }
    else if (Input.GetKeyDown(KeyCode.A) && isAllowedTurn && turnDirectionAllowed != "Right")
    {
      if (turnRotationValue - 90 < -350)
        turnRotationValue = 0.0f;
      else
        turnRotationValue -= 90.0f;


      turningVectorValue = new Vector3(0, turnRotationValue, 0);
      iTween.RotateTo(gameObject, turningVectorValue, 0.5f);
      //transform.Rotate(0, -90, 0);    
      currentDirection = GetDirection(true);
      isAllowedTurn = false;
      TurnMade("Left");
      //Nieuw
      xPosition = transform.position.x;
      yPosition = transform.position.z;

    }

    //Nieuw mobile
    if (ButtonLeftTurnClicked && isAllowedTurn && turnDirectionAllowed != "Right" && !paused )
    {
      currentDirection = GetDirection(true);
      if (turnRotationValue - 90 < -350)
        turnRotationValue = 0.0f;
      else
        turnRotationValue -= 90.0f;


      turningVectorValue = new Vector3(0, turnRotationValue, 0);
      iTween.RotateTo(gameObject, turningVectorValue, 0.5f);
      //iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .01));
      isAllowedTurn = false;
      TurnMade("Left");

      //Nieuw
      xPosition = transform.position.x;
      yPosition = transform.position.z;

      ButtonLeftTurnClicked = false;

    }
    if (ButtonRightTurnClicked && isAllowedTurn && turnDirectionAllowed != "Left" && !paused )
    {
      currentDirection = GetDirection(false);
      // iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .01));
      //transform.Rotate(0, 90, 0);
      if (turnRotationValue + 90 > 350)
        turnRotationValue = 0.0f;
      else
        turnRotationValue += 90.0f;


      turningVectorValue = new Vector3(0, turnRotationValue, 0);
      iTween.RotateTo(gameObject, turningVectorValue, 0.5f);

      isAllowedTurn = false;

      TurnMade("Right");


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
    if (Input.GetKeyDown(KeyCode.Space) & transform.position.y < 0.2 && !paused )
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

    //Werkt niet. Misschien later.
    //if(Input.GetKeyDown(KeyCode.LeftControl))
    //if (Input.GetKey(KeyCode.LeftControl))
    //{
    //    animator.Play("Sneak");
    //}

    //Timetracking
    if (!finished )
    {
      gameTime += Time.deltaTime;

      var minutes = gameTime / 60;
      var seconds = gameTime % 60;
      var fraction = (gameTime * 100) % 100;

      //timeText.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
    }

    //Speed power-up
    if (speed > setSpeed)
    {
      speedTimer += Time.deltaTime;
      if (speedTimer >= speedBoostTime)
      {
        speed = setSpeed;
      }
    }

    //Shield power-up
    if (isInvincible)
    {
      invincibleTime += Time.deltaTime;
      shieldInstance.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
      if (invincibleTime >= invincibleTimer)
      {
        isInvincible = false;
        Destroy(shieldInstance);
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

    //Nieuw
  public Direction GetDirection(bool leftArrow)
  {
    switch (currentDirection)
    {
      case Direction.North: return leftArrow ? Direction.West : Direction.East;
      case Direction.East: return leftArrow ? Direction.North : Direction.South;
      case Direction.South: return leftArrow ? Direction.East : Direction.West;
      case Direction.West: return leftArrow ? Direction.South : Direction.North;
      default: throw new Exception("Missing Direction");
    }
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
    //Picking up coin
    //if (other.gameObject.CompareTag("Coin"))
    //{


    //  Coin game = other.GetComponent<Coin>();
    //  audio.PlayOneShot(game.coin);

    //  other.gameObject.SetActive(false);

    //  count += 1;
    //  SetCountText();
    //}

    //Finish
    //if (other.gameObject.CompareTag("End"))
    //{
    //  winText.text = "Finish";
    //  speed = 0.0f;
    //  //Uitgecommenarieerd
    //  //animator.Stop();
    //  animator.Play("Wary");
    //  //Nieuw toegevoegd
    //  finished = true;
    //}


    //if (other.gameObject.CompareTag("LaneTrigger"))
    //{
    //  isAllowedTurn = false;
    //}

    //Turns
    //if (other.gameObject.CompareTag("LaneTriggerExit"))
    //{
    //  turningPosition = transform.position;
    //  isAllowedTurn = true;
    //  other.gameObject.SetActive(false);
    //  if (other.gameObject.layer == 8)
    //  {
    //    turnDirectionAllowed = "Right";
    //  }
    //  else if (other.gameObject.layer == 9)
    //  {
    //    turnDirectionAllowed = "Left";
    //  }
    //  else
    //  {
    //    turnDirectionAllowed = "Splits";
    //  }

    //}

    //Lives
    //if (other.gameObject.CompareTag("Obstacle"))
    //{
    //  if (!isInvincible)
    //  {
    //    speed = 1;
    //    animator.Play("Damage2");
    //  }
    //  other.gameObject.SetActive(false);
    //}

    //Automatic turn in split
    //if (other.gameObject.CompareTag("SidePiece3") && isAllowedTurn)
    //{


    //    int leftOrRight = UnityEngine.Random.Range(0, 1);
    //    if (leftOrRight == 0)
    //    {
    //      currentDirection = GetDirection(false);
    //      //transform.Rotate(0, 90, 0);
    //      if (turnRotationValue + 90 > 350)
    //        turnRotationValue = 0.0f;
    //      else
    //        turnRotationValue += 90.0f;


    //      turningVectorValue = new Vector3(0, turnRotationValue, 0);
    //      iTween.RotateTo(gameObject, turningVectorValue, 0.5f);
    //      isAllowedTurn = false;
    //      TurnMade("Right");

    //      //Nieuw
    //      xPosition = transform.position.x;
    //      yPosition = transform.position.z;
    //    }
    //    else
    //    {
    //      currentDirection = GetDirection(true);
    //      //transform.Rotate(0, -90, 0);
    //      if (turnRotationValue - 90 < -350)
    //        turnRotationValue = 0.0f;
    //      else
    //        turnRotationValue -= 90.0f;


    //      turningVectorValue = new Vector3(0, turnRotationValue, 0);
    //      iTween.RotateTo(gameObject, turningVectorValue, 0.5f);
    //      isAllowedTurn = false;
    //      TurnMade("Left");

    //      //Nieuw
    //      xPosition = transform.position.x;
    //      yPosition = transform.position.z;
    //    }
      
    //}

    //Automatic turn in right bend
    //if (other.gameObject.CompareTag("SidePiece2") && isAllowedTurn && turnDirectionAllowed != "Left")
    //{

    //    currentDirection = GetDirection(false);
    //    //transform.Rotate(0, 90, 0);
    //    if (turnRotationValue + 90 > 350)
    //      turnRotationValue = 0.0f;
    //    else
    //      turnRotationValue += 90.0f;


    //    turningVectorValue = new Vector3(0, turnRotationValue, 0);
    //    iTween.RotateTo(gameObject, turningVectorValue, 0.5f);
    //    isAllowedTurn = false;
    //    TurnMade("Right");

    //    //Nieuw
    //    xPosition = transform.position.x;
    //    yPosition = transform.position.z;
    //  }
    

    //Automatic turn in left bend
    //if (other.gameObject.CompareTag("SidePiece2") && isAllowedTurn && turnDirectionAllowed != "Right")
    //{
 
    //    currentDirection = GetDirection(true);
    //    //transform.Rotate(0, -90, 0);
    //    if (turnRotationValue - 90 < -350)
    //      turnRotationValue = 0.0f;
    //    else
    //      turnRotationValue -= 90.0f;


    //    turningVectorValue = new Vector3(0, turnRotationValue, 0);
    //    iTween.RotateTo(gameObject, turningVectorValue, 0.5f);
    //    isAllowedTurn = false;
    //    TurnMade("Left");
    //    //Nieuw
    //    xPosition = transform.position.x;
    //    yPosition = transform.position.z;
    //  }
    

    //Automatic turn for right barricade
    //if (other.gameObject.CompareTag("BarricadeRight") && isAllowedTurn)
    //{
     
    //    currentDirection = GetDirection(true);
    //    //transform.Rotate(0, -90, 0);
    //    if (turnRotationValue - 90 < -350)
    //      turnRotationValue = 0.0f;
    //    else
    //      turnRotationValue -= 90.0f;


    //    turningVectorValue = new Vector3(0, turnRotationValue, 0);
    //    iTween.RotateTo(gameObject, turningVectorValue, 0.5f);
    //    isAllowedTurn = false;
    //    TurnMade("Left");

    //    //Nieuw
    //    xPosition = transform.position.x;
    //    yPosition = transform.position.z;
    //  }
    

    //Automatic turn for left barricade
    if (other.gameObject.CompareTag("BarricadeLeft") && isAllowedTurn)
    {
 
        currentDirection = GetDirection(false);
        //transform.Rotate(0, 90, 0);
        if (turnRotationValue + 90 > 350)
          turnRotationValue = 0.0f;
        else
          turnRotationValue += 90.0f;


        turningVectorValue = new Vector3(0, turnRotationValue, 0);
        iTween.RotateTo(gameObject, turningVectorValue, 0.5f);
        isAllowedTurn = false;
        TurnMade("Right");

        //Nieuw
        xPosition = transform.position.x;
        yPosition = transform.position.z;
      
    }

    //Left trigger after split
    if (other.gameObject.CompareTag("TriggerLeft1"))
    {
      Instantiate(barricadeRight, new Vector3(3f, 0f, 72.5f), new Quaternion(0f, 0f, 0f, 0f));
    }

    //Left trigger after split
    if (other.gameObject.CompareTag("TriggerRight1"))
    {
      Instantiate(barricadeLeft, new Vector3(-3f, 0f, 72.5f), new Quaternion(0f, 0f, 0f, 0f));
    }

    //Left trigger after split
    if (other.gameObject.CompareTag("TriggerLeft2"))
    {
      Instantiate(barricadeRight, new Vector3(45f, 0f, 198.5f), new Quaternion(0f, 0f, 0f, 0f));
    }

    //Left trigger after split
    if (other.gameObject.CompareTag("TriggerRight2"))
    {
      Instantiate(barricadeLeft, new Vector3(39f, 0f, 198.5f), new Quaternion(0f, 0f, 0f, 0f));
    }


    //Speed power-up
    if (other.gameObject.CompareTag("SpeedBoost"))
    {
      speed += 4;
      Destroy(other.gameObject);
      //other.gameObject.SetActive(false);
    }

    //Shield power-up
    if (other.gameObject.CompareTag("InvincibilityBoost"))
    {
      isInvincible = true;
      Destroy(other.gameObject);
      //other.gameObject.SetActive(false);
      shieldInstance = (GameObject)Instantiate(shieldPrefab, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
    }
  }


  public void SetCountText()
  {
        //countText.text = "Coins: " + count.ToString();
        countText.text = "Coins: " + coinCount.ToString();
  }

    //Nieuw
  public void TurnMade(string key)
  {
    if (key == "Left")
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
    else if (key == "Right")
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
          positionTurnFix = Convert.ToInt32(turningPosition.z);
          transform.position = new Vector3(positionTurnFix + 3, transform.position.y, transform.position.z);
          currentLane = Lane.Middle;
        }
        else
        {
          positionTurnFix = Convert.ToInt32(turningPosition.z);
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
