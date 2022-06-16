using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SwipeDetector;

public class SwipeLogger : MonoBehaviour
{
    public Text Debug;
    private void Awake()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }
    private void SwipeDetector_OnSwipe(SwipeData data)
    {
    }
}
