using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStats : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Destroy(gameObject,15f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Hoop")
        {
            Debug.Log("Basket");
        }
    }
}
