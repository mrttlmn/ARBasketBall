using System;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.HLAPI.Data;
using Niantic.ARDK.Networking.HLAPI.Object;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using UnityEngine;

[RequireComponent(typeof(AuthBehaviour))]
public class BallNetworkBehaviour : NetworkedBehaviour
{
    internal GameManager Controller = null;

    private Vector3 _pos;

    // Left and right boundaries of the field, in meters
    private float _lrBound = 1.25f;

    // Forward and backwards boundaries of the field, in meters
    private float _fbBound = 1.25f;

    // Initial velocity, in meters per second
    private float _initialVelocity = 0.6f;
    private Vector3 _velocity;

    // Cache the floor level, so the ball is reset properly
    private Vector3 _initialPosition;

    // Flags for whether the game has started and if the local player is the host
    private bool _gameStart;
    private bool _isHost;

    // Store the start location of the ball
    private void Start()
    {
        _initialPosition = transform.position;
    }

    // Set up the initial conditions
    internal void GameStart(bool isHost)
    {
        _isHost = isHost;
        _gameStart = true;
        _initialPosition = transform.position;

        if (!_isHost)
            return;

        _velocity = new Vector3(_initialVelocity, 0, _initialVelocity);
    }

    // Signal that the ball has been hit, with a unit vector representing the new direction
    internal void Hit(Vector3 direction)
    {
        if (!_gameStart || !_isHost)
            return;

        _velocity = direction * _initialVelocity;
        _initialVelocity *= 1.1f;
    }

    // Perform movement, send position to non-host player
    private void Update()
    {
        if (!_gameStart || !_isHost)
            return;

        _pos = gameObject.transform.position;

        _pos.x += _velocity.x * Time.deltaTime;
        _pos.z += _velocity.z * Time.deltaTime;

        transform.position = _pos;

        if (_pos.x > _initialPosition.x + _lrBound)
            _velocity.x = -_initialVelocity;
        else if (_pos.x < _initialPosition.x - _lrBound)
            _velocity.x = _initialVelocity;

        if (_pos.z > _initialPosition.z + _fbBound)
            _velocity.z = -_initialVelocity;
        else if (_pos.z < _initialPosition.z - _fbBound)
            _velocity.z = _initialVelocity;
    }

    // Signal to host that a goal has been scored
    private void OnTriggerEnter(Collider other)
    {
        
        if (!_gameStart || !_isHost)
            return;

        _initialVelocity = 0.6f;
        _velocity = new Vector3(0, 0, _initialVelocity);
        gameObject.transform.position = _initialPosition;
        Debug.Log("This User : " + Controller._self.ToString());
        Debug.Log("Kullan�c� Say�s� : " + Controller._players.Count);
        foreach (IPeer player in Controller._players)
        {
            Debug.Log("Scored Player : " + player.Identifier);
            if (Controller._self == player)
                Controller.GoalScored(player);
        }
    }

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
