using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SharedAR : MonoBehaviour
{
    public InputField SessionIDField;
    public GameObject PeerPoseIndicator;

    private IARNetworking _arNetworking;
    private IMultipeerNetworking _networking;
    private IARSession _session;

    private Dictionary<IPeer,GameObject> _poseIndicatorDict = new Dictionary<IPeer,GameObject>();
    public void CreateAndRunSharedAR()
    {
        _arNetworking = ARNetworkingFactory.Create();

        _networking = _arNetworking.Networking;
        _session = _arNetworking.ARSession;

        var worldTrackConfig = ARWorldTrackingConfigurationFactory.Create();
        worldTrackConfig.WorldAlignment = WorldAlignment.Gravity;
        worldTrackConfig.IsAutoFocusEnabled = true;

        worldTrackConfig.IsSharedExperienceEnabled = true;

        _session.Run(worldTrackConfig);
        _session.Ran += OnSessionRan;

        var sessionID = SessionIDField.text;
        var sesionIdAsByte = Encoding.UTF8.GetBytes(sessionID);

        _networking.Join(sesionIdAsByte);
        _networking.Connected += OnNetworkConnected;
    }
    private void OnSessionRan(ARSessionRanArgs args)
    {
        Debug.Log(message: "AR Session Baþladý");
    }
    private void OnNetworkConnected(ConnectedArgs args)
    {
        Debug.LogFormat("Networking joied, peerID:{0},isHost : {1}", args.Self, args.IsHost);
    }
    private void OnDestroy()
    {
        _session.Dispose();
        _networking.Dispose();
        _arNetworking.Dispose();
    }
    private void  OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        if (!_poseIndicatorDict.ContainsKey(args.Peer))
            _poseIndicatorDict.Add(args.Peer, Instantiate(PeerPoseIndicator));
        
        GameObject poseIndicator;
        if (_poseIndicatorDict.TryGetValue(args.Peer, out poseIndicator))
            poseIndicator.transform.position = args.Pose.ToPosition() + new Vector3(0, 0, -0.5f);
    }
}


