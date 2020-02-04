using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


    public class SimpleNover : MonoBehaviour
    {
        List<GameObject> poinTargets = new List<GameObject>();
        List<Vector3> pastTargets = new List<Vector3>();
        List<Vector3> crvPoints = new List<Vector3>();
        List<GameObject> lineRender = new List<GameObject>();


        public SendToArduino sendToArduino;
        public GameObject poinTarget;
        public GameObject textField;

        float _angle;
        float time;
        Transform pastLocation;
        public float pastDistance;
        public float maxSpeed = 10;
        public int rottationSpeed = 100;
      
        public bool firstTime = true;
        float angle;
        Vector3 intPoint,p1,p2,center;


        public float SetLinearSpeed 
        {
            get
            {
                return maxSpeed;
            }
            set
            {
                value = maxSpeed;
            }
        }

        public float SetRotationSpeed
        {
            get
            {
                return rottationSpeed;
            }
            set
            {
                value = rottationSpeed;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            CalculateTarget();
            pastLocation = this.transform;
        }

        // Update is called once per frame
        void Update()
        {

            if (poinTargets.Count>0 )
            {
                CalculateTarget();
                if (poinTarget == null) return;
                foreach (GameObject item in poinTargets)
                {
                    if (item == null) poinTargets.Remove(item);
                }

                float curentDist = Vector3.Distance(this.gameObject.transform.position, poinTarget.gameObject.transform.position);

                if (curentDist < 4f)
                {
                    pastTargets.Add(poinTarget.transform.position);
                    if (pastTargets.Count > 3) pastTargets.RemoveAt(0);
                    Destroy(poinTarget);
                    poinTargets.Remove(poinTarget);
                    CalculateTarget();
                }

                int layerMask = 1 << 2;
                RaycastHit hit;
                var ray =  new Ray( transform.position, transform.TransformDirection(Vector3.forward));
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) )
                {
                    _angle = rottationSpeed * time;
                    time = 0;
                    if (!firstTime)
                    {
                        crvPoints.Add(center);
                        crvPoints.Add(this.transform.position);
                        FindObjectOfType<Graphics.Grafics>().TextAngles(crvPoints, _angle);
                        crvPoints.Clear();
                        pastDistance = curentDist;
                        firstTime = true;
                    }
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    transform.position += transform.forward * Time.deltaTime * (maxSpeed/10 * hit.distance);
                    //Debug.Log("Did Hit");
                }
                else
                {
                    Rotte();
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    p2 = transform.position;
                    //Debug.Log("Did not Hit");
                }
            }
            else
            {
                CalculateTarget();
            }
        }

        
        
        void Rotte()
        {
            time += Time.deltaTime;
            if (rottationSpeed * time > 360) transform.LookAt(poinTarget.transform);
            //print(time);
            
            if (pastDistance > 30) pastDistance = pastDistance / 10;

            if (firstTime)
            {
                crvPoints.Add(this.transform.position);
                Vector3 heading = poinTarget.transform.position - transform.position;
                angle = AngleDir(transform.forward, heading, transform.up);
                firstTime = false;
            }

            if (angle <= -1)
            {
                intPoint = (pastLocation.position + (-pastLocation.right) * pastDistance/2);
                center = intPoint;
                 _angle = -rottationSpeed * Time.deltaTime;
                transform.RotateAround(intPoint, pastLocation.up, _angle);
                print("Object is to the left   AntiClock");

            }
            else if (angle >= 1)
            {
                Debug.Log("Object is to the right    Clock");
                intPoint = (pastLocation.position + pastLocation.right* pastDistance/2);
                center = intPoint;
                 _angle = rottationSpeed * Time.deltaTime;
                transform.RotateAround(intPoint, pastLocation.up, _angle);
            }
            else
                print("Object is directly ahead");
        }


        float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);

            if (dir > 0f)
            {
                return 1f;
            }
            else if (dir < 0f)
            {
                return -1f;
            }
            else
            {
                return 0f;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(intPoint.x,0,intPoint.z), 1);
        }

        void CalculateTarget()
        {
   
                poinTargets = GameObject.FindGameObjectsWithTag("FD").ToList();
                poinTarget = GetClosestEnemy(poinTargets);
                if(poinTarget)
                poinTarget.layer = 2;

        }

        GameObject GetClosestEnemy(List<GameObject> enemies)
        {
            poinTarget = null;

            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (GameObject potentialTarget in enemies)
            {
                if (potentialTarget == null)
                {
                    poinTargets.Remove(potentialTarget);
                    return poinTarget;
                }
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    poinTarget = potentialTarget;
                }
                
            }
            return poinTarget;
        }
        public Vector3 RotatePointAroundPivot(Vector3 _finalPosition, Vector3 _pivotPosition, Quaternion _finalRotation)
        {
            return _pivotPosition + (_finalRotation * (_finalPosition - _pivotPosition)); // returns new position of the point;
        }

    }

