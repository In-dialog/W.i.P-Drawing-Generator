using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace LineSegmentsIntersection
{
    public class SimpleNover : MonoBehaviour
    {
        Transform _pivotPosition;//parented to this transform
        Transform _finalPosition;//I want to move this transform to here
        public List<GameObject> poinTargets = new List<GameObject>();
        public List<Vector3> pastTargets = new List<Vector3>();
        public SendToArduino sendToArduino;
        public GameObject poinTarget;
        Transform pastLocation;

        public PlacePoints place;
        public float pastDistance;
        //public float pastDist, curentDist,stDist;
        int maxRotation = 20;
        public int maxSpeed = 10;
        int maxRandom = 10;
        public int rottationSpeed = 100;
        bool firstTime = true;
       public float angle;
       public  Vector3 intPoint,p1,p2,center;

        // Start is called before the first frame update
        void Start()
        {
            CalculateTarget();
            pastLocation = this.transform;

        }

        // Update is called once per frame
        void Update()
        {
         
            if (poinTarget != null)
            {
                float curentDist = Vector3.Distance(this.gameObject.transform.position, poinTarget.gameObject.transform.position);

                if (curentDist < 5f)
                {
                    pastTargets.Add(poinTarget.transform.position);
                    if (pastTargets.Count > 3) pastTargets.RemoveAt(0);

                    place.DestroyObjects();
                    place.PointsInstance();
                    maxRotation = Random.Range(1, 5);
                    maxRandom = Random.Range(2, 100);

                }


                // Bit shift the index of the layer (8) to get a bit mask
                int layerMask = 1 << 8;

                // This would cast rays only against colliders in layer 8.
                // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                layerMask = ~layerMask;

                RaycastHit hit;
                var ray =  new Ray( transform.position, transform.TransformDirection(Vector3.forward));
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    transform.position += transform.forward * Time.deltaTime * maxSpeed;

                    pastLocation = this.transform;
                    pastDistance = curentDist;

                    if (!firstTime )
                    {
                        float j =  -1*(p1.z - center.z);
                        Debug.Log(j + "  j is  !!!!!");
                        float i = -1 * (p1.x - center.x);
                        Debug.Log(i + "  i is  !!!!!");
                        Debug.Log(p2 + "  p2 is  ");
                        if (j + i == 0) return;
                        //float j = p1.z - offSet;
                        //offSet = p1.x - center.x;
                        //float i = p1.x - offSet;
                        if (angle == 1)
                        {
                            string arduinoString = "G02X" + p2.x + "Y"+p2.z + "I" + i + "J" + j;
                            if (!sendToArduino.positionsToSend.Contains(arduinoString))
                                sendToArduino.positionsToSend.Add(arduinoString);
                        }
                        if (angle == -1)
                        {
                            //string arduinoString = "G03X" + Mathf.Round(hit.transform.position.x).ToString() + "Y" + Mathf.Round(hit.transform.position.z).ToString();
                            //if (!sendToArduino.positionsToSend.Contains(arduinoString))
                            //    sendToArduino.positionsToSend.Add(arduinoString);
                        }

                    }
                    else
                    {
                        string arduinoString = "G01X" + Mathf.Round(hit.transform.position.x).ToString() + "Y" + Mathf.Round(hit.transform.position.z).ToString();
                        if (!sendToArduino.positionsToSend.Contains(arduinoString))
                            sendToArduino.positionsToSend.Add(arduinoString);
                    }
                    firstTime = true;
                    //Debug.Log("Did Hit");

                }
                else
                {
                    if (pastDistance == 0) pastDistance = 2;
                    if (firstTime)
                    {
                        Vector3 heading = poinTarget.transform.position - transform.position;
                        angle = AngleDir(transform.forward, heading, transform.up);
                        p1 = transform.position;
                        firstTime = false;
                    }
                    if (angle == -1)
                    {
                        intPoint = pastLocation.position + (-pastLocation.right) * pastDistance;
                        center = intPoint;
                        transform.RotateAround(intPoint, pastLocation.up, -rottationSpeed * Time.deltaTime);
                        print("Object is to the left   AntiClock");

                    }
                    else if (angle == 1)
                    {
                        Debug.Log("Object is to the right    Clock");
                        maxRandom = maxSpeed;
                        intPoint = pastLocation.position + (pastLocation.right) * pastDistance;
                        center = intPoint;
                        transform.RotateAround(intPoint, pastLocation.up, rottationSpeed * Time.deltaTime);
                    }
                    else
                        print("Object is directly ahead");

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
            if (poinTargets.Count > 0)
            {
                poinTarget = GetClosestEnemy(poinTargets);

            }
            else
            {
                poinTargets = GameObject.FindGameObjectsWithTag("FD").ToList();
                poinTarget = GetClosestEnemy(poinTargets);

            }

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
                    //potentialTarget.gameObject.tag = "Player";
                    //pastDistance = Vector3.Distance(poinTarget.transform.position, pastTargets[pastTargets.Count-2]);
                }
            }
            return poinTarget;
        }
        public Vector3 RotatePointAroundPivot(Vector3 _finalPosition, Vector3 _pivotPosition, Quaternion _finalRotation)
        {
            return _pivotPosition + (_finalRotation * (_finalPosition - _pivotPosition)); // returns new position of the point;
        }

    }
}