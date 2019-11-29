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

    public int caseSwitch;
    int time;

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
        CreateList();
        TargetList();
        ItereteList();
        caseSwitch = 1;
    }

    void Update()
    {
        if (objects1.Count == 0)
        {
            SetDis = Random.Range(3, 20);
            caseSwitch++;
            Debug.Log("newList");
            randomMode ^= true;
            CreateList();
            TargetList();
        }
        if (caseSwitch == 4)
        {
            caseSwitch = 1;
        }
        if (target.Length < 1) return;
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
                    Move(2);
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
        objects1 = new List<GameObject>();
        target = new Transform[objects1.Count];
    }


    void ItereteList()
    {
        for (int i = 0; i < target.Length; ++i)
        {
            if (bestTarget == target[i])
            {
                object1 = objects1[i];
                float Dist = Vector3.Distance(this.gameObject.transform.position, object1.gameObject.transform.position);

                if (Dist < 0.15F && time > timeR)
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
                //linearMovment = false;
                float step = 10 * Time.deltaTime;
                pastRotation = this.transform.rotation;
                // Determine which direction to rotate towards
                Vector3 targetDirection = object1.transform.position - transform.position;
                // The step size is equal to speed times frame time.
                float singleStep = step / randomNumaber2;
                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.1F);
                // Calculate a rotation a step closer to the target and applies rotation to this object
                transform.rotation = Quaternion.LookRotation(newDirection);
                //Check if rotation has finished so we can increase the speed of the movmemnt
                if (pastRotation != Quaternion.LookRotation(newDirection))
                {
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