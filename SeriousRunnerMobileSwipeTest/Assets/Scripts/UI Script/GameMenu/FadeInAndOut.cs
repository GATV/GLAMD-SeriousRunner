using UnityEngine;
using System.Collections;

public class FadeInAndOut : MonoBehaviour
{

    public GameObject objectToControl;
    public Transform rightPoint;
    public Transform centerPoint;

    private int fadeSelect = 0;
    private float lerpValue = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (fadeSelect == 0)
        {
            //Nothing happens
        }
        else if (fadeSelect == 1)
        {
            if (lerpValue >= 1)
            {
                fadeSelect = 0;
                lerpValue = 0.0f;
            }
            else
            {
                lerpValue += 0.1f;
                objectToControl.transform.position = Vector3.Lerp(objectToControl.transform.position, centerPoint.position, lerpValue);
            }
        }
        else if (fadeSelect == 2)
        {
            if (lerpValue >= 1)
            {
                fadeSelect = 0;
                lerpValue = 0.0f;
            }
            else
            {
                lerpValue += 0.1f;
                objectToControl.transform.position = Vector3.Lerp(objectToControl.transform.position, rightPoint.position, lerpValue);
            }
        }
    }

    public void FadeIn()
    {
        fadeSelect = 1;
    }

    public void FadeOut()
    {
        fadeSelect = 2;
    }
}
