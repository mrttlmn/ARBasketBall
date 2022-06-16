using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    public bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    public static event Action<SwipeData> OnSwipe = delegate { };

    public Text Debug;
    private bool throwTimerLock;
    private float throwTimer = 3f;
    private void Update()
    {

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }
            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
                var distance = Vector2.Distance(fingerDownPosition, fingerUpPosition);

                var BallLauncher = GameObject.Find("ARGameManager").GetComponent<BallThrow>();
                if (distance < 300)
                {
                    BallLauncher.LaunchBall(0f);
                }
                if (distance > 300 && distance < 1000)
                {
                    BallLauncher.LaunchBall(distance / 5);
                }
            }
        }
    }
    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
        }
    }
    private void SendSwipe(SwipeDirection direction)
    {

        SwipeData swipeData = new SwipeData()
        {
            direction = direction,
            startPosition = fingerDownPosition,
            endPosition = fingerUpPosition,
        };
        OnSwipe(swipeData);
    }
    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }
    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }
    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }
    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }
    public struct SwipeData
    {
        public Vector2 startPosition;
        public Vector2 endPosition;
        public SwipeDirection direction;
    }
    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
