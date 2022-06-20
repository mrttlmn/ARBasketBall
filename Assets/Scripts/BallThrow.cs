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
    private GameObject ballPrefab;

    private float defForce = 100.0f;

    public Vector3 forceVec;
    // Update is called once per frame
    public NetworkedUnityObject _objectToNetworkSpawn;
    public void LaunchBall(float force)
    {
        NetworkedUnityObject spawnedInstance = _objectToNetworkSpawn.NetworkSpawn(arCamera.transform.position);
        forceVec = arCamera.transform.forward;
        forceVec.y += 0.5f;
        var ballRb = spawnedInstance.gameObject.GetComponent<Rigidbody>();
        force = force + defForce;
        if (force > 400) force = 400;
        if (force < defForce) force = defForce;
        ballRb.AddForce(forceVec * force);
    }
    
}
