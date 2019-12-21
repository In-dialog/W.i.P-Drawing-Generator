using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class DrawSimbol : MonoBehaviour
{
    public List<GameObject> objects1 = new List<GameObject>();
    public GameObject object1;
    public Transform[] target;
    public Transform bestTarget;
    Quaternion pastRotation;
    public Vector3 recPos;
    public PlacePoints placePoints;
    public int caseSwitch;
    double time, rTime;

    public float speed;
    public float rotationspeed = 50;
    static float rotationleft = 720;

    public bool ReadyToMove, linearMovment, newTarget ;

    float SetDis;
    float TargetDis;
    readonly int timeR = 22; /// minimum stime spend in one place

    int randomNumaber, randomNumaber2; // random numbers for offseting the path at each cycle 
    public bool randomMode; // shift order of interpolation

    void Start()
    {
        Clear();

        //caseSwitch = 2;
    }

    void Update()
    {


        if (objects1.Count == 0 )
        {
            placePoints.DestroyObjects();
            placePoints.PointsInstance();

            SetDis = Random.Range(3, 20);
            //if (caseSwitch != 3) caseSwitch++;
            //else caseSwitch = 1;
            Debug.Log("newList");
            //randomMode ^= true;
            Clear();
        } 

        if (objects1[0] == null)
        {
            Clear();
        }
        if (randomMode) bestTarget = target[randomNumaber];
        else GetClosestEnemy(target);
        if (bestTarget == null) return;

        ItereteList();
        TargetDis = Vector3.Distance(this.gameObject.transform.position, object1.gameObject.transform.position);

        switch (caseSwitch)
        {
            case 1:
                Move(1);
                break;
            case 2:
                Move(2);
                recPos = transform.position;
                break;
            case 3:
                if (TargetDis > SetDis)
                {
                    Move(1);
                    rotationleft = 360;
                }
                else
                {
                    if (ReadyToMove) Move(1);
                    else Move(3);
                }
                break;
        }
    }


    public void Clear()
    {
        objects1.Clear();
        //Debug.Log("Im cleaning!!!   " + objects1.Count);
        target = new Transform[objects1.Count];
        CreateList();
        TargetList();
        ItereteList();
    }


    void ItereteList()
    {
        for (int i = 0; i < target.Length; ++i)
        {
            if (bestTarget == target[i])
            {
                object1 = objects1[i];
                float Dist = Vector3.Distance(this.gameObject.transform.position, object1.gameObject.transform.position);

                if (Dist < 0.1F)
                {
                    ReadyToMove = false;
                    objects1.RemoveAt(i);
                    TargetList();
                    randomNumaber = Random.Range(0, target.Length);
                    randomNumaber2 = Random.Range(1, 10);
                    newTarget = true;
                    time = 0;
                }
                else
                {
                    time++;
                    newTarget = false;
                }
            }
        }
    }

    void Move(int Type)
    {
        float step1 = speed * Time.deltaTime;
        //Debug.Log(Type);
        switch (Type)
        {   
            case 1:
                transform.position = Vector3.MoveTowards(this.gameObject.transform.position, object1.transform.position, step1);
                linearMovment = true;
                break;

            case 2:
                linearMovment = false;
                float step = 10 * Time.deltaTime;
                pastRotation = this.transform.rotation;
                Vector3 targetDirection = object1.transform.position - transform.position;
                float singleStep = step / randomNumaber2;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.1F);
                transform.rotation = Quaternion.LookRotation(newDirection);

                if (pastRotation != Quaternion.LookRotation(newDirection))
                {
                    rTime+=Time.deltaTime;
                    //Debug.Log(rTime);
                    if (rTime > 10)
                    {
                        Debug.LogWarning("ToLong!!!");
                        transform.position = object1.transform.position;
                        //caseSwitch++;
                        rTime = 0;
                    }
                    transform.position += transform.forward * step;
                    linearMovment = false;
                    Debug.Log("Im rotating");
                }
                else
                {
                    float Dist = Vector3.Distance(this.gameObject.transform.position, object1.gameObject.transform.position);
                    if (Dist > 0.8f) transform.position += transform.forward * step1;
                    else transform.position += transform.forward * step;
                    linearMovment = true;
                    rTime = 0;

                    Debug.Log("Im  not rotating");
                }

                break;

            case 3:
                ///// Rorates around a point by x degrees 
                float rotation = rotationspeed * Time.deltaTime;
                linearMovment = false;
                if (rotationleft > rotation)
                {
                    ReadyToMove = false;
                    rotationleft -= rotation;
                }
                else
                {
                    rotation = rotationleft;
                    rotationleft = 0;
                    ReadyToMove = true;
                }
                transform.RotateAround(object1.transform.position, Vector3.up, rotation);
                //Debug.Log("Im circle with radious "+ SetDis+"  and center  "+ object1.transform.position);
                break;


        }
    }

    public void CreateList()
    {
        objects1 = new List<GameObject>();
        objects1 = GameObject.FindGameObjectsWithTag("FD").ToList();
    
    }


    public void TargetList()
    {
        target = new Transform[objects1.Count];
        for (int i = 0; i < objects1.Count; ++i)
        {
            target[i] = objects1[i].transform;
        }
    }

    Transform GetClosestEnemy (Transform[] enemies)
	{
		bestTarget = null;
        
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach(Transform potentialTarget in enemies)
		{
            //if (potentialTarget == null) return;
			Vector3 directionToTarget = potentialTarget.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
		}
        return bestTarget;
	}
	
}