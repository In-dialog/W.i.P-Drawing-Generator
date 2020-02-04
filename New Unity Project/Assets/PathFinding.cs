using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PathFinding : MonoBehaviour
{
    public  List<Vector3> satelite = new List<Vector3>();
    public List<Vector3> pointFound = new List<Vector3>();
    Vector3[] bestObstions;
    List<Vector3> originalSatelite = new List<Vector3>();


    public LineRenderer lr;
    int count,count2;
    float wight =-1;
    public float pastWaight =-1;
    Vector2 Vector;



    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            satelite.Add(new Vector3(UnityEngine.Random.Range(-500, 500), UnityEngine.Random.Range(-500, 500), 0));
            originalSatelite.Add(satelite[i]);
        }
        pointFound.Add(this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        if (count2 >= originalSatelite.Count | originalSatelite.Count<1)
        {
            return;
        }
        if (satelite.Count < 1)
        {
            if (pastWaight < wight)
            {
                bestObstions = new Vector3[lr.positionCount];
                lr.GetPositions(bestObstions);
                pastWaight = wight;
            }

            pointFound.Clear();
            if (count2 == originalSatelite.Count-1)
            {
                lr.positionCount = bestObstions.Length-1;
                lr.SetPositions(bestObstions);
            }

            pointFound.Add(originalSatelite[count2]);
            count = 0;
            wight = 0;
            satelite = new List<Vector3>(originalSatelite);
            count2++;
        }
        else
        {
            Vector3 temp = CheckDistance(satelite, pointFound[pointFound.Count - 1]);
            pointFound.Add(temp);
            satelite.Remove(temp);
            for (int i = 0; i < pointFound.Count - 3; i += 2)
            {
                //if()
                if (LineSegmentsIntersection.Math2d.LineSegmentsIntersection(pointFound[pointFound.Count - 2], pointFound[pointFound.Count - 1], pointFound[i], pointFound[i + 1], out Vector))
                {
                    wight -= Vector3.Distance(pointFound[i], pointFound[i + 1])/100;
                    wight -= 1f;
                    Debug.Log("intersect");
                }

            }


            lr.positionCount = count + 1;
            lr.SetPosition(count, temp);

            count++;
        }
    }
    Vector3 CheckDistance(List<Vector3> allPoints, Vector3 thePoint)
    {
        List<float> distanceList = new List<float>();
        foreach (var item in allPoints)
        {
            distanceList.Add(Vector3.Distance(thePoint, item));
        
        }
        float minVal = distanceList.Min();
        return allPoints[distanceList.IndexOf(minVal)];
    }

    public List<Vector3> SetPoints
    {
        set
        {
            satelite = new List<Vector3>(value);
            originalSatelite = new List<Vector3>(value);
            //wight = originalSatelite.Count;
        }
    }


}

