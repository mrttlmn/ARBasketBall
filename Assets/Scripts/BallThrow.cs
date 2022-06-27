using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDK.Networking.HLAPI.Authority;
using Niantic.ARDK.Networking.HLAPI;
using Niantic.ARDK.Networking;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.Networking.HLAPI.Data;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking.HLAPI.Routing;

public class BallThrow : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;
    
    [SerializeField]
    private NetworkedUnityObject ballPrefab;
    private GameObject _ball;

    private float defForce = 100.0f;

    public Vector3 forceVec;

    private IARNetworking _arNetworking;
    private IPeer _self;

    private bool _synced = false;
    private IHlapiSession _manager;
    private IAuthorityReplicator _auth;

    public void Start()
    {
        ARNetworkingFactory.ARNetworkingInitialized += OnAnyARNetworkingSessionInitialized;

    }
    public void LaunchBall(float force)
    {
        if (!_synced)
            return;


        forceVec = arCamera.transform.forward;
        forceVec.y += 0.5f;

        _ball = ballPrefab.NetworkSpawn
          (
            _arNetworking.Networking,
            arCamera.transform.position,
            Quaternion.identity
          ).gameObject;

        var ballRb = _ball.gameObject.GetComponent<Rigidbody>();
        force = force + defForce;
        if (force > 400) force = 400;
        if (force < defForce) force = defForce;
        ballRb.AddForce(forceVec * force);
        ballRb.useGravity = true;

    }

    private void OnPeerStateReceived(PeerStateReceivedArgs args)
    {

        if (_self.Identifier != args.Peer.Identifier)
        {

            if (args.State == PeerState.Stable)
            {
                _synced = true;
            }

            return;
        }

    }

    private void OnAnyARNetworkingSessionInitialized(AnyARNetworkingInitializedArgs args)
    {
        _arNetworking = args.ARNetworking;
        _arNetworking.PeerStateReceived += OnPeerStateReceived;
        _arNetworking.Networking.Connected += OnDidConnect;
    }
    private void OnDidConnect(ConnectedArgs connectedArgs)
    {

        _self = connectedArgs.Self;

        _manager = new HlapiSession(19244);

        var group = _manager.CreateAndRegisterGroup(new NetworkId(4321));
        _auth = new GreedyAuthorityReplicator("pongHLAPIAuth", group);

        //   _auth.TryClaimRole(_isHost ? Role.Authority : Role.Observer, () => { }, () => { });

        var authToObserverDescriptor =
          _auth.AuthorityToObserverDescriptor(TransportType.ReliableUnordered);

    }
}
