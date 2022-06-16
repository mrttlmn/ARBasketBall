using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopController : MonoBehaviour
{
    public void DestroyHoop()
    {
        var controls = GameObject.Find("ARGameManager").GetComponent<HoopManager>();
        controls.ClearHoop();
    }
    public void HoopRotateXPlus5()
    {
        var Hoop = GameObject.Find("Hoop(Clone)");      
        Hoop.transform.Rotate(Vector3.up * 5);
    }
    public void HoopRotateXMinus5()
    {
        var Hoop = GameObject.Find("Hoop(Clone)");
        Hoop.transform.Rotate(Vector3.up * -5);
    }
}
