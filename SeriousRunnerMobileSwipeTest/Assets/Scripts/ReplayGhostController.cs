using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;

public class ReplayGhostController : MonoBehaviour
{
    private Replay r;
    private Direction lastDirection;
    private Queue<Direction> directionQueue;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        string data = MPScript.Data.ReplayData;

        if (data != null)
        {
            animator = GetComponent<Animator>();
            r = new Replay(data);
            directionQueue = r.GetDirectionQueue();
            lastDirection = directionQueue.Dequeue();
            InvokeRepeating("UpdateOrientation", 0, 0.2f);
            animator.SetFloat("Speed", 10);
            iTween.MoveTo(gameObject, iTween.Hash("path", r.Path, "time", r.Count * 0.2f, "easetype", iTween.EaseType.linear));
        }
        else
            gameObject.SetActive(false);
    }

    void UpdateOrientation()
    {
        if (directionQueue.Count > 0)
        {
            Direction newDirection = directionQueue.Dequeue();
            if (lastDirection != newDirection)
            {
                iTween.RotateTo(gameObject, new Vector3(0, (int)newDirection * 90, 0), 1);
                lastDirection = newDirection;
            }
        }
        else
        {
            CancelInvoke("UpdateOrientation");
            animator.SetFloat("Speed", 0);
            animator.Play("Wary");
            float offset = gameObject.transform.position.y;
            iTween.MoveTo(gameObject, gameObject.transform.position - new Vector3(0, offset, 0), 0.5f);
        }
    }
}
