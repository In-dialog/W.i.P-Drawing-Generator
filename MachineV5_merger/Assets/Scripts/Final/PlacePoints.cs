using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlacePoints : MonoBehaviour {
    public DrawSimbol drawSimbol;
	public GameObject prefab;
    public bool isConvex = false;
    //int t;
    Vector3 center = new Vector3(0, 0, 0);
    //public Transform center;
    MeshFilter mF;
    Mesh mesh;
    public int maxPoints = 10;
    GameObject[] objects;
   public  Material color;


    Vector3[] baseVertices ;

    void Awake() {

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        mF = plane.GetComponent<MeshFilter>();

        mF.mesh = UpdateMesh(mF.mesh,(16/2/5)*10,(30/2/5)*10);
        //plane.transform.localScale = new Vector3(15,1,20);
        plane.GetComponent<MeshRenderer>().sharedMaterial = color;
        PointsInstance(maxPoints);
        plane.transform.position = new Vector3(0, -1, 0);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DestroyObjects();
            PointsInstance(maxPoints);
            drawSimbol.Clear();

        }
    }
    void  PointsInstance(int _maxPoints)
    {
        for (int i = 0; i < maxPoints; i++)
        {
            Vector3 point = mF.mesh.GetRandomPointOnSurface();
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - point);
            rot = new Quaternion(0, 0, 0, 0);


            GameObject temp = Instantiate(prefab, point, rot);
            temp.name = "point";
        }

    }
        



   public  void DestroyObjects()
    {
        objects = new GameObject[maxPoints];
        objects = GameObject.FindGameObjectsWithTag("FD");
        for (var i = 0; i < objects.Length; i++)
        Destroy(objects[i]);
    }



    Mesh UpdateMesh(Mesh aMesh, float scaleX, float scaleZ)
    {
        if (baseVertices == null) baseVertices = aMesh.vertices;

        Vector3 [] vertices = new Vector3[baseVertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vertex = baseVertices[i];
            vertex.x = vertex.x * scaleX;
            //vertex.y = vertex.y; 
            vertex.z = vertex.z * scaleZ;
            vertices[i] = vertex;
        }
        aMesh.vertices = vertices;
        aMesh.RecalculateNormals();
        aMesh.RecalculateBounds();

        return aMesh;
    }




}


