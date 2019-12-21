using UnityEngine;

public class PlacePoints : MonoBehaviour {
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
    public GameObject plane;
    public int _case;
    public int step = 10;
    public float maxX, maxZ;
    Vector3[] baseVertices ;
    public int zPos, xPos, stroed;
    public int [] zpos = new int[0];
   public  int m1, m2, m3;
    public int pM1, pM2, pM3;


    //int pl
    void Awake() {
        zpos = new int[(int)step];
        for (int i = 0; i < step; i++)
        {
            if(i==0) zpos[i] =(int)-(step *5);
            if (i >= 1) zpos[i] = zpos[i-1] + (int)Random.Range(10,15);

        }


        if (_case == 1)
        {
            if (plane == null)
            {
                plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                mF = plane.GetComponent<MeshFilter>();
                mF.mesh = UpdateMesh(mF.mesh, (16 / 2 / 5) * 10, (30 / 2 / 5) * 10);
                //mF.transform.position = new Vector3(160,0,300);
                plane.GetComponent<MeshRenderer>().sharedMaterial = color;
            }
            else
            {
                mF = plane.GetComponent<MeshFilter>();
            }
            //PointsInstance();
            plane.transform.position = new Vector3(0, -1, 0);
        }
        else
        {
            //zPos = (int)-maxZ;
            //PointsInstance();
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DestroyObjects();
            PointsInstance();
        }
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
                    temp.name = "point";
                }
                break;

            case 2:


                //zPos ++;
                if (zPos >= maxZ | zPos < -maxZ)
                {
                    stroed += 1;
                    xPos = stroed;
                    zPos = 0;
                }
                zPos = zPos+(int)step;
                //xPos= (int)Mathf.Sign(xPos * Time.deltaTime);
                //xPos += (int)Mathf.Cos(xPos) + (int)Mathf.Sign(xPos) + (int)Mathf.Sign(zPos) / (int)Mathf.Sign(xPos * Time.deltaTime);

                //xPos = (int)Mathf.Cos(xPos) - (int)Mathf.Sin(xPos * zPos * stroed) + xPos;
                //xPos += (int)Mathf.Sin(zpos[zPos]) + (int)Mathf.Cos(xPos * zpos[zPos] * stroed) * (int)Mathf.Atan(xPos);

                //m1 += Random.Range(pM3, step);
                //m2 = Random.Range((int)-maxX,(int)maxX);
                //m3 = Random.Range(pM1,(int)maxX/17);
                //if (m1 > maxZ)
                //{
                //    m1 = 0;
                //    m3 = 0;
                //}
                //Vector3 point1 = new Vector3(m2,0, m1 + m3);

                //pM1 = m1;
                //pM2 = m2;
                //pM3 = m3;
                xPos = (int)Mathf.Cos(zPos) + (int)Mathf.Cos(xPos) * (int)Time.timeSinceLevelLoad;
                //xPos += 1; 
                //xPos = (int)Random.Range(-maxX, maxX);
                Vector3 point2 = new Vector3(xPos, 0, zPos);
                Quaternion rot2 = new Quaternion(0, 0, 0, 0);
                GameObject temp2 = Instantiate(prefab, point2, rot2);
                //temp.name = "point";
                
                break;



        }




    }
        



   public  void DestroyObjects()
    {
        objects = new GameObject[maxPoints];
        objects = GameObject.FindGameObjectsWithTag("FD");
        for (var i = 0; i < objects.Length; i++) Destroy(objects[i]);
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


