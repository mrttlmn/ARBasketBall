using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.HLAPI;
using Niantic.ARDK.Networking.HLAPI.Authority;
using Niantic.ARDK.Networking.HLAPI.Object;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.HLAPI;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Networking.HLAPI.Authority;
using Niantic.ARDK.Networking.HLAPI.Routing;
using Niantic.ARDK.Networking.HLAPI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Niantic.ARDK.Utilities.Input.Legacy;
using Niantic.ARDK.AR.ARSessionEventArgs;

public class GameManager : MonoBehaviour
{
    /// Prefabs to be instantiated when the game starts
    [SerializeField]
    private NetworkedUnityObject HoopPrefab = null;

    // [SerializeField]
    //  private NetworkedUnityObject ballPrefab = null;

    // [SerializeField]
    //  private NetworkedUnityObject playerPrefab = null;

    /// Reference to the StartGame button
    //[SerializeField]
    //private GameObject startGame = null;

    [SerializeField]
    private Button joinButton = null;

    [SerializeField]
    private FeaturePreloadManager preloadManager = null;

    /// Reference to AR Camera, used for hit test
    [SerializeField]
    private Camera _camera = null;

    /// References to game objects after instantiation
    private GameObject _ball;

    //  private GameObject _player;
    private GameObject _playingField;



    /// HLAPI Networking objects
    private IHlapiSession _manager;

    private IAuthorityReplicator _auth;
    private MessageStreamReplicator<Vector3> _hitStreamReplicator;

    private INetworkedField<Vector3> _fieldPosition;
    private INetworkedField<byte> _gameStarted;

    /// Cache your location every frame
    private Vector3 _location;

    /// Some fields to provide a lockout upon hitting the ball, in case the hit message is not
    /// processed in a single frame


    private IARNetworking _arNetworking;

    private bool _isHost;
    private IPeer _self;

    private bool _gameStart;
    private bool _synced;

    public Text _debug;
    private void Start()
    {
        ARNetworkingFactory.ARNetworkingInitialized += OnAnyARNetworkingSessionInitialized;


        if (preloadManager.AreAllFeaturesDownloaded())
            OnPreloadFinished(true);
        else
            preloadManager.ProgressUpdated += PreloadProgressUpdated;


    }

    private void PreloadProgressUpdated(FeaturePreloadManager.PreloadProgressUpdatedArgs args)
    {
        if (args.PreloadAttemptFinished)
        {
            preloadManager.ProgressUpdated -= PreloadProgressUpdated;
            OnPreloadFinished(args.FailedPreloads.Count == 0);
        }
    }

    private void OnPreloadFinished(bool success)
    {
        if (!success)
            Debug.LogError("Failed to download resources needed to run AR Multiplayer");
    }

    // When all players are ready, create the game. Only the host will have the option to call this
    public void StartGame()
    {
        _gameStart = true;
        _gameStarted.Value = Convert.ToByte(true);

    }

    // Instantiate game objects
    private void InstantiateObjects(Vector3 position)
    {
        if (_playingField != null && _isHost)
        {
            Debug.Log("Relocating the playing field!");
            _fieldPosition.Value = new Optional<Vector3>(position);
            //    _player.transform.position = position + new Vector3(0, 0, -1);
            _playingField.transform.position = position;
            // _ball.transform.position = position;

            return;
        }

        Debug.Log("Instantiating the playing field!");

        // Both players want to spawn an avatar that they are the Authority of
        var startingOffset = _isHost ? new Vector3(0, 0, -1) : new Vector3(0, 0, 1);

        /*  _player =
            playerPrefab.NetworkSpawn
            (
              _arNetworking.Networking,
              position + startingOffset,
              Quaternion.identity,
              Role.Authority
            )   
            .gameObject;*/

        // Only the host should spawn the remaining objects
        if (!_isHost)
            return;

        // Instantiate the playing field at floor level
        if (_playingField == null)
            _playingField = HoopPrefab.NetworkSpawn(_arNetworking.Networking, position, Quaternion.identity).gameObject;

        // Set the score text for all players
        //    _scoreText.Value = "Score: 0 - 0";

        // Spawn the ball and set up references
        /*    _ballBehaviour = ballPrefab.NetworkSpawn(_arNetworking.Networking,position,Quaternion.identity).DefaultBehaviour as BBallBehaviour;*/



    }


    // Every frame, detect if you have hit the ball
    // If so, either bounce the ball (if host) or tell host to bounce the ball
    private void Update()
    {
        if (_manager != null)
            _manager.SendQueuedData();

        if (_synced && !_gameStart && _isHost)
        {
            _debug.text = "synced";
            if (PlatformAgnosticInput.touchCount <= 0)
                return;

            var touch = PlatformAgnosticInput.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _debug.text = "Click Approved";
                FindFieldLocation(touch);
            }
        }

        if (!_gameStart)
            return;

    }

    private void FindFieldLocation(Touch touch)
    {
        var currentFrame = _arNetworking.ARSession.CurrentFrame;

        if (currentFrame == null)
            return;

        var results =
          currentFrame.HitTest(_camera.pixelWidth, _camera.pixelHeight, touch.position, ARHitTestResultType.ExistingPlaneUsingExtent);

        if (results.Count <= 0)
        {
            Debug.Log("Unable to place the field at the chosen location. Can't find a valid surface");

            return;
        }

        // Get the closest result
        var result = results[0];

        var hitPosition = result.WorldTransform.ToPosition();
        _debug.text = "findfieldworks";
        InstantiateObjects(hitPosition);
    }

    // Every updated frame, get our location from the frame data and move the local player's avatar
    private void OnFrameUpdated(FrameUpdatedArgs args)
    {
        _location = MatrixUtils.PositionFromMatrix(args.Frame.Camera.Transform);

    }

    private void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        _debug.text = _self.Identifier + " )( " + args.Peer.Identifier.ToString();
        if (_self.Identifier != args.Peer.Identifier)
        {
            _debug.text = "stabling";

            if (args.State == PeerState.Stable)
            {
                _synced = true;

                if (_isHost)
                {
                    InstantiateObjects(_location);
                }
                else
                {
                    InstantiateObjects(_arNetworking.LatestPeerPoses[args.Peer].ToPosition());
                }
            }

            return;
        }


    }

    private void OnDidConnect(ConnectedArgs connectedArgs)
    {
        _isHost = connectedArgs.IsHost;
        _self = connectedArgs.Self;

        _manager = new HlapiSession(19244);

        var group = _manager.CreateAndRegisterGroup(new NetworkId(4321));
        _auth = new GreedyAuthorityReplicator("pongHLAPIAuth", group);

        _auth.TryClaimRole(_isHost ? Role.Authority : Role.Observer, () => { }, () => { });

        var authToObserverDescriptor =
          _auth.AuthorityToObserverDescriptor(TransportType.ReliableUnordered);

        _fieldPosition =
          new NetworkedField<Vector3>("fieldReplicator", authToObserverDescriptor, group);

        _fieldPosition.ValueChangedIfReceiver += OnFieldPositionDidChange;


        _gameStarted = new NetworkedField<byte>("gameStarted", authToObserverDescriptor, group);


    }

    private void OnFieldPositionDidChange(NetworkedFieldValueChangedArgs<Vector3> args)
    {
        var value = args.Value;
        if (!value.HasValue)
            return;

        var offsetPos = value.Value + new Vector3(0, 0, 1);
        //    _player.transform.position = offsetPos;
    }



    private void OnAnyARNetworkingSessionInitialized(AnyARNetworkingInitializedArgs args)
    {
        _debug.text = "t";
        _arNetworking = args.ARNetworking;
        _arNetworking.PeerStateReceived += OnPeerStateReceived;

        _arNetworking.ARSession.FrameUpdated += OnFrameUpdated;
        _arNetworking.Networking.Connected += OnDidConnect;
    }

    private void OnDestroy()
    {
        ARNetworkingFactory.ARNetworkingInitialized -= OnAnyARNetworkingSessionInitialized;

        if (_arNetworking != null)
        {
            _arNetworking.PeerStateReceived -= OnPeerStateReceived;
            _arNetworking.ARSession.FrameUpdated -= OnFrameUpdated;
            _arNetworking.Networking.Connected -= OnDidConnect;
        }
    }
}
