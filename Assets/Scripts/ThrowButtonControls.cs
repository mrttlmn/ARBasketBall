using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowButtonControls : MonoBehaviour
{
    public float timePassed;
    float timeRemaining;
    public void pointDown()
    {
        timePassed = Time.time;
    }
    public void pointUp()
    {
        timeRemaining = Time.time - timePassed;
        Debug.Log(timeRemaining);
        var Throw = GameObject.Find("ARGameManager").GetComponent<BallThrow>();
        Throw.LaunchBall(timeRemaining);
    }
    
}
