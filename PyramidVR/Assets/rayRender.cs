using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayRender : MonoBehaviour
{
    public Vector3 point;
    // Update is called once per frame
    void Update()
    {
      
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 3;
        Ray r = new Ray(transform.position, forward);
        point = r.GetPoint(3);
        Debug.DrawRay(transform.position, forward, Color.green);
    }
}
