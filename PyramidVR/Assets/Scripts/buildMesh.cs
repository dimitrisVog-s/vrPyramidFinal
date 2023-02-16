using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class buildMesh : MonoBehaviour
{
    public GameObject s;
    public Camera cam;
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public GameObject rightController;
    public GameObject meshPyramid;

    public Vector3 vert2 = new Vector3(1, 1, 0);
    public Vector3 vert3= new Vector3(0, 1, 0);
    public Vector3 vert4 = new Vector3(0, 1, 1);
    public Vector3 vert5 = new Vector3(1, 1, 1);

    public float Volume;
    public float finalVolume;
    public Material Green;
    public Material Red;
    public Mesh mesh;

    private InputDevice targetDevice;
    private XRNode xrNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private Vector3 spherePosition;

    private rayRender ray;
    


    void getDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
    }

    void OnEnable()
    {
        getDevice();
    }

    void Start()
    {
        Debug.Log(GameObject.Find("CubeR"));
        ray = rightController.GetComponent<rayRender>();

       

        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
        
        meshInitiliazation(mesh);
        Volume = VolumeOfMesh(mesh);

    }

    // Update is called once per frame
    void Update()
    {

        getDevice();
        if(devices.Count > 0)
        {
            targetDevice = devices[0];
        }
        
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);

        

        if (Input.GetMouseButton(0) | primaryButtonValue)
        {

            Vector3 vec = new Vector3(ray.point.x, ray.point.y, ray.point.z);
            //Vector3 vec = new Vector3(s.transform.position.x, s.transform.position.y, s.transform.position.z);
            vert2 = Vector3.Lerp(vert2, vec, Time.deltaTime);
            vert3 = Vector3.Lerp(vert3, vec, Time.deltaTime);
            vert4 = Vector3.Lerp(vert4, vec, Time.deltaTime);
            vert5 = Vector3.Lerp(vert5, vec, Time.deltaTime);
            s.transform.position = Vector3.Lerp(s.transform.position, vec, Time.deltaTime);


            meshInitiliazation(mesh);
            finalVolume = VolumeOfMesh(mesh);
            if (Volume == finalVolume)
            {
                meshPyramid.GetComponent<MeshRenderer>().material = Green;
            }
            else
            {
                meshPyramid.GetComponent<MeshRenderer>().material = Red;
            }

            

        }
        
    }

    public void meshInitiliazation(Mesh mesh)
    {
        

        //Vertices
        Vector3[] vertices = new Vector3[]
        {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            vert2,
            vert3,
            vert4,
            vert5,
            new Vector3 (1, 0, 1),
            new Vector3 (0, 0, 1),
        };

        //Triangles 
        int[] tringles = new int[]
        {
            0, 2, 1, //face front
            0, 3, 2,
            2, 3, 4, //face top
            2, 4, 5,
            1, 2, 5, //face right
            1, 5, 6,
            0, 7, 4, //face left
            0, 4, 3,
            5, 4, 7, //face back
            5, 7, 6,
            0, 6, 7, //face bottom
            0, 1, 6

        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = tringles;
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        //volume = Mathf.Round(volume * 10.0f) * 0.1f;
        volume = Mathf.Floor(10 * volume) / 10;
        return Mathf.Abs(volume);
    }
}
