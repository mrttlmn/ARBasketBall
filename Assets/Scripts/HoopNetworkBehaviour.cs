using System;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.HLAPI.Data;
using Niantic.ARDK.Networking.HLAPI.Object;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using UnityEngine;
public class HoopNetworkBehaviour : NetworkedBehaviour
{
    protected override void SetupSession(out Action initializer, out int order)
    {
        initializer = () =>
        {
            var auth = Owner.Auth;
            var descriptor = auth.AuthorityToObserverDescriptor(TransportType.UnreliableUnordered);

            new UnreliableBroadcastTransformPacker
            (
              "netTransform",
              transform,
              descriptor,
              TransformPiece.Position,
              Owner.Group
            );
        };

        order = 0;
    }
}