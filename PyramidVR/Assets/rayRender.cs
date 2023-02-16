using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayRender : MonoBehaviour
{
    public Vector3 point;
    
    public GameObject pyramid;

    public float distance;
    // Update is called once per frame
    void Update()
    {
        //Vector3 position = transform.position;
        //distance = Vector3.Distance(transform.position, sphere.transform.position);
        //Debug.Log(Vector3.Distance(transform.position, GameObject.Find("meshPyramid").GetComponent<buildMesh>().vert2));
        distance = Vector3.Distance(transform.position, GameObject.Find("meshPyramid").GetComponent<buildMesh>().vert2);
        //Debug.Log(GameObject.Find("meshPyramid").GetComponent<buildMesh>().vert2);
        Vector3 forward = transform.TransformDirection(Vector3.forward) * distance;
        Ray r = new Ray(transform.position, forward);
        point = r.GetPoint(distance);
        Debug.DrawRay(transform.position, forward, Color.green);
    }
}
