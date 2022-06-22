using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;

public class BallThrow : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    
    [SerializeField]
    private NetworkedUnityObject ballPrefab;

    private float defForce = 100.0f;

    public Vector3 forceVec;
   
    public void LaunchBall(float force)
    {
        forceVec = arCamera.transform.forward;
        forceVec.y += 0.5f;
        var newBall = ballPrefab;
        Debug.Log(newBall.gameObject.name);
        var ballRb = newBall.gameObject.GetComponent<Rigidbody>();
        force = force + defForce;
        if (force > 400) force = 400;
        if (force < defForce) force = defForce; 
        ballRb.AddForce(forceVec * force,ForceMode.Impulse);
        ballRb.useGravity = true;
        var x = newBall.NetworkSpawn(arCamera.transform.position, Quaternion.identity);
        
        Debug.Log(x.name);
    }
}
