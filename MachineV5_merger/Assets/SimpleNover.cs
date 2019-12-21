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
        Vector3 intPoint;

        // Start is called before the first frame update
        void Start()
        {
            CalculateTarget();
            pastLocation = this.transform;

        }

        // Update is called once per frame
        void Update()
        {
            //place.DestroyObjects();
            //place.PointsInstance();


            //if (poinTarget != null)
            //{
            //curentDist = Vector3.Distance(this.gameObject.transform.position, poinTarget.gameObject.transform.position);
            //float step1 = 10 * Time.deltaTime;

            //    if (curentDist < 5f)
            //    {
            //        pastTargets.Add(poinTarget.transform.position);

            //        CalculateTarget();
            //        pastDist = Vector3.Distance(pastTargets[pastTargets.Count-1], poinTarget.transform.position);
            //    }
            //    else
            //    {
            //        transform.position = Vector3.MoveTowards(this.gameObject.transform.position, poinTarget.transform.position, step1);
            //    }
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
                    //firstTime = false;

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

                    //Debug.Log("Did Hit");
                    pastLocation = this.transform ;
                    pastDistance = curentDist;
                    firstTime = true;

                    string arduinoString = "G01X" + Mathf.Round(this.transform.position.x).ToString() + "Y" + Mathf.Round(this.transform.position.z).ToString() + " F1500.000";
                    if (!sendToArduino.positionsToSend.Contains(arduinoString))
                        sendToArduino.positionsToSend.Add(arduinoString);
                }
                else
                {
                    if (firstTime)
                    {
                        Vector3 heading = poinTarget.transform.position - transform.position;
                        angle = AngleDir(transform.forward, heading, transform.up);
               
                        firstTime = false;
                    }
                    if (angle == -1)
                    {
                        intPoint = pastLocation.position + (-pastLocation.right) * pastDistance;
                        transform.RotateAround(intPoint, pastLocation.up, -maxSpeed * Time.deltaTime);
                        print("Object is to the left   AntiClock");

                    }
                    else if (angle == 1)
                    {
                        Debug.Log("Object is to the right    Clock");
                        maxRandom = maxSpeed;

                        intPoint = pastLocation.position + (pastLocation.right) * pastDistance;
                        transform.RotateAround(intPoint, pastLocation.up, maxSpeed * Time.deltaTime);
                    }
                    else
                        print("Object is directly ahead");

                    //intPoint = pastLocation.position + (-pastLocation.right) * pastDistance;
                    //transform.RotateAround(intPoint, pastLocation.up, -maxSpeed * Time.deltaTime);
                    //intPoint = temp;
                    //test.transform.position = intPoint;
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    //Debug.Log("Did not Hit");
                }
            

                //var q = Quaternion.LookRotation(poinTarget.transform.position - transform.position);
                ////transform.rotation = Quaternion.LookRotation(newDirection);
                //if (transform.rotation != q)
                //{
                //    b2 = (transform.position );
                //    if (firstTime == false)
                //    {
                //        a1 = (transform.position + transform.forward * -10000);
                //        a2 = (transform.position + transform.forward * 10000);
                //        firstTime = true;
                //    }
                //    //rottationSpeed = (int)(rottationSpeed - curentDist / 10);
                //    transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rottationSpeed * 10 * Time.deltaTime);

                //    //transform.LookAt(pastTargets[0]);
                //    transform.position += transform.forward * Time.deltaTime * rottationSpeed;
                //    string arduinoString = "G01X" + Mathf.Round(this.transform.position.x).ToString() + "Y" + Mathf.Round(this.transform.position.z).ToString() + " F1500.000";
                //    if (!sendToArduino.positionsToSend.Contains(arduinoString))
                //        sendToArduino.positionsToSend.Add(arduinoString);
                //    Debug.Log("ImRoating");
                //}
                //else
                //{
                //    firstTime = false;
                //    if (b2 != Vector3.zero)
                //    {
                //        //a1 = pastTargets[1];
                //        //b2 = transform.position;
                //        b1 = (transform.position + transform.forward * -10000);
                //        if (!Math2d.LineSegmentsIntersection(new Vector2(a1.x, a1.z), new Vector2(a2.x, a2.z), new Vector2(b1.x, b1.z), new Vector2(b2.x, b2.z), out intPoint)) intPoint = Vector2.zero;

                //        Debug.DrawLine(b1, b2);
                //        Debug.DrawLine(a1, a2);

                //        Debug.LogError("PointRecorde" + b2);
                //        b2 = Vector3.zero;
                //    }
                //    transform.position += transform.forward * Time.deltaTime * maxSpeed;
                //    string arduinoString = "G01X" + Mathf.Round(poinTarget.transform.position.x).ToString() + "Y" + Mathf.Round(poinTarget.transform.position.z).ToString() + " F1500.000";
                //    if (!sendToArduino.positionsToSend.Contains(arduinoString))
                //        sendToArduino.positionsToSend.Add(arduinoString);
                //    //transform.LookAt(poinTarget.transform);
                //    //a1 = transform.position;
                //    //a2 = (transform.position + transform.forward * -10000);

                //}

                //if(pastTargets.Count>1 & curentDist < 5f)
                //transform.RotateAround(pastTargets[1], Vector3.up, 100 * Time.deltaTime);

                //transform.position += transform.forward * Time.deltaTime * 10;

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