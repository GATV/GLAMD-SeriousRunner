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

  private bool isAllowedTurn;
  private float setSpeed;

  //Mobile controls
  private float fingerStartTime = 0.0f;
  private Vector2 fingerStartPos = Vector2.zero;
  private bool isSwipe = false;
  private float minSwipeDist = 50.0f;
  private float maxSwipeTime = 0.5f;

  //life
  private int lives;
  private bool isAlive;

  //Jumping
  public float jumpTime;
  public float jumpHeight;

  private float jumpTimer;
  private bool switchable;

  //Nieuw toegevoegd    
  private float gameTime;
  private bool finished;

  //Used for power-ups
  public float speedBoostTime;
  public GameObject shieldPrefab;
  public float invincibleTimer;

  private bool isInvincible;
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
  private Animator animator;

  //UI
  public Text countText;
  public Text winText;
  public Text lifeText;
  public Text deathText;
  public Text timeText;

  private int count;

  public Button ButtonLeft;
  public Button ButtonRight;
  public Button ButtonPause;

  public Text pauseButtonText;
  public bool ButtonLeftTurnClicked;
  public bool ButtonRightTurnClicked;

  // Use this for initialization
  private CharacterController controller;

  void Start()
  {
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
    lives = 2;
    jumpTimer = jumpTime;
    SetLifeText();
    isAlive = true;
    switchable = true;

    ButtonLeft = GameObject.FindGameObjectWithTag("LeftTurnButton").GetComponent<Button>();
    ButtonRight = GameObject.FindGameObjectWithTag("RightTurnButton").GetComponent<Button>();

    ButtonLeft.interactable = false;
    ButtonRight.interactable = false;
    pauseButtonText.text = "";
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
                  if (currentLane < Lane.Right && !paused && switchable)
                  {
                    currentLane++;
                    controller.Move(directionMovements[GetDirection(false)] * laneDistance);
                  }
                }
                else
                {
                  //left
                  if (currentLane > Lane.Left && !paused && switchable)
                  {
                    currentLane--;
                    controller.Move(directionMovements[GetDirection(true)] * laneDistance);
                  }
                }
              }

              if (swipeType.y != 0.0f)
              {
                if (swipeType.y > 0.0f)
                {
                  //jump
                  if (transform.position.y < 0.2)
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

    if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > Lane.Left && !paused && switchable)
    {
      currentLane--;
      controller.Move(directionMovements[GetDirection(true)] * laneDistance);
      //rb.transform.position += GetLaneDirection(true) * laneDistance;
    }
    else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < Lane.Right && !paused && switchable)
    {
      currentLane++;
      controller.Move(directionMovements[GetDirection(false)] * laneDistance);

      // rb.transform.position += GetLaneDirection(false) * laneDistance;
    }

    if (Input.GetKeyDown(KeyCode.D) && isAllowedTurn && turnDirectionAllowed != "Left")
    {
      currentDirection = GetDirection(false);
      transform.Rotate(0, 90, 0);
      isAllowedTurn = false;
      TurnMade("Right");
    }
    else if (Input.GetKeyDown(KeyCode.A) && isAllowedTurn && turnDirectionAllowed != "Right")
    {
      currentDirection = GetDirection(true);
      transform.Rotate(0, -90, 0);
      isAllowedTurn = false;
      TurnMade("Left");
    }

    //Nieuw mobile
    if (ButtonLeftTurnClicked && isAllowedTurn && turnDirectionAllowed != "Right")
    {
      currentDirection = GetDirection(true);
      transform.Rotate(0, -90, 0);
      isAllowedTurn = false;
      TurnMade("Left");

      ButtonLeftTurnClicked = false;
      ButtonLeft.interactable = false;
      ButtonRight.interactable = false;
    }
    if (ButtonRightTurnClicked && isAllowedTurn && turnDirectionAllowed != "Left")
    {
      currentDirection = GetDirection(false);
      transform.Rotate(0, 90, 0);
      isAllowedTurn = false;

      TurnMade("Right");

      ButtonRightTurnClicked = false;
      ButtonRight.interactable = false;
      ButtonLeft.interactable = false;
    }



    //Collisions
    if (speed < setSpeed & isAlive & !finished)
    {
      Debug.Log(speed.ToString());
      speed += Time.deltaTime;
      if (speed > setSpeed)
      {
        speed = setSpeed;
      }
    }

    //Jumping
    if (Input.GetKeyDown(KeyCode.Space) & transform.position.y < 0.2)
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
      Debug.Log(jumpTimer);
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
    animator.SetFloat("Speed", speed);

    //Werkt niet. Misschien later.
    //if(Input.GetKeyDown(KeyCode.LeftControl))
    //if (Input.GetKey(KeyCode.LeftControl))
    //{
    //    animator.Play("Sneak");
    //}

    //Timetracking
    if (!finished & isAlive)
    {
      gameTime += Time.deltaTime;

      var minutes = gameTime / 60;
      var seconds = gameTime % 60;
      var fraction = (gameTime * 100) % 100;

      timeText.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
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

  Direction GetDirection(bool leftArrow)
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

    //Restarting (button)
    //if (!isAlive)
    //{
    //    //Uitgecommentarieerd
    //    //if (Input.GetKeyDown(KeyCode.Space))
    //    //int xpos = (Screen.width) - (80) / 2;
    //    //int ypos = (Screen.height) - (40) / 2;
    //    if (GUI.Button(new Rect(510, 100, 80, 40), "Restart"))
    //    {
    //        SceneManager.LoadScene("SeriousRunnerTest");
    //    }
    //}
  }

  void OnTriggerEnter(Collider other)
  {
    //Picking up coin
    if (other.gameObject.CompareTag("Coin"))
    {


      Coin game = other.GetComponent<Coin>();
      audio.PlayOneShot(game.coin);
  
      other.gameObject.SetActive(false);

      count += 1;
      SetCountText();
    }

    //Finish
    if (other.gameObject.CompareTag("End"))
    {
      winText.text = "Finish";
      speed = 0.0f;
      //Uitgecommenarieerd
      //animator.Stop();
      animator.Play("Wary");
      //Nieuw toegevoegd
      finished = true;
    }


    if (other.gameObject.CompareTag("LaneTrigger"))
    {
      isAllowedTurn = false;
    }

    //Turns
    if (other.gameObject.CompareTag("LaneTriggerExit"))
    {
      turningPosition = transform.position;
      isAllowedTurn = true;
      other.gameObject.SetActive(false);
      if (other.gameObject.layer == 8)
      {
        turnDirectionAllowed = "Right";
      }
      else if (other.gameObject.layer == 9)
      {
        turnDirectionAllowed = "Left";
      }
      else
      {
        turnDirectionAllowed = "Splits";
      }
      ButtonLeft.interactable = true;
      ButtonRight.interactable = true;
    }

    //Lives
    if (other.gameObject.CompareTag("Obstacle"))
    {
      if (!isInvincible)
      {
        lives--;
        SetLifeText();
        speed = 1;
        animator.Play("Damage2");
      }
      if (lives == 0)
      {
        isAlive = false;
        speed = 0;
        animator.Play("Idle");
        deathText.text = "Game over...";
      }
      other.gameObject.SetActive(false);
    }

    //Automatic turn in split
    if (other.gameObject.CompareTag("SidePiece3") && isAllowedTurn)
    {
      lives--;
      SetLifeText();
      if (lives == 0)
      {
        isAlive = false;
        speed = 0;
        animator.Play("Idle");
        deathText.text = "Game over...";
      }
      else
      {
        int leftOrRight = UnityEngine.Random.Range(0, 1);
        if (leftOrRight == 0)
        {
          currentDirection = GetDirection(false);
          transform.Rotate(0, 90, 0);
          isAllowedTurn = false;
          TurnMade("Right");
        }
        else
        {
          currentDirection = GetDirection(true);
          transform.Rotate(0, -90, 0);
          isAllowedTurn = false;
          TurnMade("Left");
        }
      }
    }

    //Automatic turn in right bend
    if (other.gameObject.CompareTag("SidePiece2") && isAllowedTurn && turnDirectionAllowed != "Left")
    {

      lives--;
      SetLifeText();
      if (lives == 0)
      {
        isAlive = false;
        speed = 0;
        animator.Play("Idle");
        deathText.text = "Game over...";
      }
      else
      {
        currentDirection = GetDirection(false);
        transform.Rotate(0, 90, 0);
        isAllowedTurn = false;
        TurnMade("Right");
      }
    }

    //Automatic turn in left bend
    if (other.gameObject.CompareTag("SidePiece2") && isAllowedTurn && turnDirectionAllowed != "Right")
    {
      lives--;
      SetLifeText();
      if (lives == 0)
      {
        isAlive = false;
        speed = 0;
        animator.Play("Idle");
        deathText.text = "Game over...";
      }
      else
      {
        currentDirection = GetDirection(true);
        transform.Rotate(0, -90, 0);
        isAllowedTurn = false;
        TurnMade("Left");
      }
    }

    //Automatic turn for right barricade
    if (other.gameObject.CompareTag("BarricadeRight") && isAllowedTurn)
    {
      lives--;
      SetLifeText();
      if (lives == 0)
      {
        isAlive = false;
        speed = 0;
        animator.Play("Idle");
        deathText.text = "Game over...";
      }
      else
      {
        currentDirection = GetDirection(true);
        transform.Rotate(0, -90, 0);
        isAllowedTurn = false;
        TurnMade("Left");
      }
    }

    //Automatic turn for left barricade
    if (other.gameObject.CompareTag("BarricadeLeft") && isAllowedTurn)
    {
      lives--;
      SetLifeText();
      if (lives == 0)
      {
        isAlive = false;
        speed = 0;
        animator.Play("Death");
        deathText.text = "Game over...";
      }
      else
      {
        currentDirection = GetDirection(false);
        transform.Rotate(0, 90, 0);
        isAllowedTurn = false;
        TurnMade("Right");
      }
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

  void SetLifeText()
  {
    lifeText.text = "Lives: " + lives.ToString();
  }
  void SetCountText()
  {
    countText.text = "Coins: " + count.ToString();
  }

  void TurnMade(string key)
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
        if (transform.position.x - turningPosition.x < -2.0f)
        {
          positionTurnFix = Convert.ToInt32(turningPosition.x);
          transform.position = new Vector3(positionTurnFix - 1, transform.position.y, transform.position.z);
          currentLane = Lane.Left;
        }
        else if (transform.position.x - turningPosition.x < -4.0f)
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
        if (transform.position.z - turningPosition.z < -2.0f)
        {
          positionTurnFix = Convert.ToInt32(turningPosition.z);
          transform.position = new Vector3(transform.position.x, transform.position.y, positionTurnFix - 1);
          currentLane = Lane.Left;
        }
        else if (transform.position.z - turningPosition.z < -4.0f)
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
