using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.External;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDK.Utilities.Input.Legacy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopManager : MonoBehaviour
{
    public Camera Camera;

    /// The types of hit test results to filter against when performing a hit test.
    [EnumFlagAttribute]
    public ARHitTestResultType HitTestType = ARHitTestResultType.ExistingPlane;

    /// The object we will place when we get a valid hit test result!
    /// A list of placed game objects to be destroyed in the OnDestroy method.
    private List<GameObject> _placedObjects = new List<GameObject>();
    /// Internal reference to the session, used to get the current frame to hit test against.
    private IARSession _session;

    public bool hoopPlaced = false;

    public NetworkedUnityObject _hoop;
    private void Start()
    {
        ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
    }

    private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args)
    {
        _session = args.Session;
        _session.Deinitialized += OnSessionDeinitialized;
    }

    private void OnSessionDeinitialized(ARSessionDeinitializedArgs args)
    {
        ClearObjects();
    }

    private void OnDestroy()
    {
        ARSessionFactory.SessionInitialized -= OnAnyARSessionDidInitialize;

        _session = null;

        ClearObjects();
    }

    private void ClearObjects()
    {
        foreach (var placedObject in _placedObjects)
        {
            Destroy(placedObject);
        }

        _placedObjects.Clear();
    }
    //private void CallobrateHoop()
    //{
    //    Hoop.transform.position = HoopAnchnor.WorldTransform.ToPosition();
    //    Debug.Log(Hoop.transform.position + " -- " + HoopAnchnor.WorldTransform.ToPosition());
    //}
    private void Update()
    {
        //if (Hoop != null && HoopAnchnor != null)
        //{
        //    Debug.Log("Callobrating.");
        //    Debug.Log(HoopAnchnor.WorldTransform.ToRotation());

        //    CallobrateHoop();
        //}

        if (_session == null)
        {
            return;
        }

        if (PlatformAgnosticInput.touchCount <= 0)
        {
            return;
        }

        var touch = PlatformAgnosticInput.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {

            TouchBegan(touch);

        }
    }

    private void TouchBegan(Touch touch)
    {

        var currentFrame = _session.CurrentFrame;
        if (currentFrame == null)
        {
            return;
        }

        var results = currentFrame.HitTest
        (
          Camera.pixelWidth,
          Camera.pixelHeight,
          touch.position,
          HitTestType
        );

        int count = results.Count;


        if (count <= 0)
            return;


        if (!hoopPlaced)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
          
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Handheld.Vibrate();
                Vector3 newVector = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                var rotation = hit.transform.parent.gameObject.transform.rotation.eulerAngles;
                rotation.z += 180;
                NetworkedUnityObject spawnedHoop = _hoop.NetworkSpawn(newVector,Quaternion.Euler(rotation));

                _placedObjects.Add(spawnedHoop.gameObject);
                hoopPlaced = true;

            }
        }

    }
    public void ClearHoop()
    {
        _placedObjects.Clear();
        
        hoopPlaced = false;
        
    }

}
