using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;

public class BallThrow : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    
    [SerializeField]
    private GameObject ballPrefab;

    private float defForce = 100.0f;

    public Vector3 forceVec;
    // Update is called once per frame
   
    public void LaunchBall(float force)
    {
        forceVec = arCamera.transform.forward;
        forceVec.y += 0.5f;
        var ball = Instantiate(ballPrefab,arCamera.transform.position,Quaternion.identity);
        var ballRb = ball.GetComponent<Rigidbody>();
        force = force + defForce;
        if (force > 400) force = 400;
        if (force < defForce) force = defForce; 
        ballRb.AddForce(forceVec * force);
    }
}
