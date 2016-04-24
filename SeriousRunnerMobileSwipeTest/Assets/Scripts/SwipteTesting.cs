using UnityEngine;
using System.Collections;

public class SwipteTesting : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    private float fingerstartTime = 0.0f;
    private Vector2 fingerstartPos = Vector2.zero;

    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;




    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {

        }

    }

}
