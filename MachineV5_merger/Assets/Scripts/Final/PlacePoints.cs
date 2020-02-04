using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;


public class PlacePoints : MonoBehaviour {
    public GameObject prefab;

    public bool isConvex = false;
    Vector3 center = new Vector3(0, 0, 0);
    MeshFilter mF;
    public int maxPoints = 10;
    public List < GameObject> objects = new List<GameObject>();
    public  Material color;
    public GameObject plane;
    public int _case;
    public int step = 10;
    public float maxX, maxZ;
    Vector3[] baseVertices ;
    float  zPos, xPos, stroed;
 
    public int SetStae 
    {
        get
        {
            return _case;
        }
        set
        {
            _case = value;
        }
    }


    GameObject @object;
    //int pl
    void Start() {
        @object = new GameObject("Cont");
        if (_case != 1)
        { 
           GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().enabled = false;
           GetComponent<MeshNavigator>().enabled = false;
        }

        if (_case == 1 | _case == 2)
        {
            if (plane == null)
            {
                plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                mF = plane.GetComponent<MeshFilter>();
                mF.mesh = UpdateMesh(mF.mesh, (maxX / 2 / 5), (maxZ / 2 / 5));
                //mF.transform.position = new Vector3(160,0,300);
                plane.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                mF = plane.GetComponent<MeshFilter>();
            }
            plane.transform.position = new Vector3(0, -3, 0);


        }
        else
        {
            maxZ /= 2;

        }
        zPos = -maxZ;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DestroyObjects();
            PointsInstance();
        }
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] == null) objects.Remove(objects[i]);
        }
        
        if (objects.Count == 0)
            PointsInstance();
    }

    public void  PointsInstance()
    {

        switch (_case)
        {
            case 1:
                for (int i = 0; i < maxPoints; i++)
                {
                    Vector3 point = mF.mesh.GetRandomPointOnSurface();
                    Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - point);
                    rot = new Quaternion(0, 0, 0, 0);
                    GameObject temp = Instantiate(prefab, point, rot);
                    temp.name = "point" + i;
                    temp.gameObject.layer = 1;
                    temp.transform.SetParent(@object.transform);
                    objects.Add(temp);
                }
                break;

            case 2:
                
                for (int i = 0; i < maxPoints; i++)
                {
                    Vector3 point = mF.mesh.GetRandomPointOnSurface();
                    Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - point);
                    rot = new Quaternion(0, 0, 0, 0);
                    GameObject temp = Instantiate(prefab, point, rot);
                    temp.name = "point" + i;
                    temp.tag = "FD";
                    temp.gameObject.layer = 4;
                    temp.transform.SetParent(@object.transform);
                    objects.Add(temp);
                }
                //FindObjectOfType<MeshNavigator>().enabled = false;

                break;

            case 3:

                zPos += Random.Range(1,step);
                if (zPos >= maxZ | zPos < -maxZ)
                {
                    zPos = -maxZ ;
                }
                float t = Time.deltaTime;
                double angle = zPos * t * System.Math.PI / 180;
                float x = Random.Range(-maxX, maxX);
                 //x = Random.Range(-x, maxX);
                 

                xPos = x;
                Vector3 point2 = new Vector3(xPos, 0, zPos);
                Quaternion rot2 = new Quaternion(0, 0, 0, 0);
                GameObject temp2 = Instantiate(prefab, point2, rot2);
                temp2.tag = "FD";
                objects.Add(temp2);
                //FindObjectOfType<MeshNavigator>().enabled = false;
                break;
        }
    }

   public void DestroyObjects()
    {
        objects = new List<GameObject>(maxPoints);
        for (var i = 0; i < objects.Count; i++) Destroy(objects[i]);
    }

    Mesh UpdateMesh(Mesh aMesh, float scaleX, float scaleZ)
    {
        if (baseVertices == null) baseVertices = aMesh.vertices;

        Vector3 [] vertices = new Vector3[baseVertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vertex = baseVertices[i];
            vertex.x = vertex.x * scaleX;
            vertex.z = vertex.z * scaleZ;
            vertices[i] = vertex;
        }
        aMesh.vertices = vertices;
        aMesh.RecalculateNormals();
        aMesh.RecalculateBounds();

        return aMesh;
    }




}


