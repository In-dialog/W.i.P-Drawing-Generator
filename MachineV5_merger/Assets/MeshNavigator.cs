using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MeshNavigator : MonoBehaviour
{
    public Transform _destination;
     NavMeshAgent _navMeshAgent;
    public List <Vector3>  wayPoint = new List<Vector3>();
    public Vector3 target;
    public float rottationSpeed;
    public GameObject @object;
    public List<GameObject> gameObjects = new List<GameObject>();
    public GameObject Player;
    public GameObject Mesh;


    NavMeshPath _path;
    bool firstTime = false;
    // Start is called before the first frame update
    void Start()
    {
        _path = new NavMeshPath();
        _navMeshAgent = Player.GetComponent<NavMeshAgent>();
        //_navMeshAgent.obstacleAvoidanceType

    }
    void InstancePoint()
    {
        if (!firstTime)
        {
            wayPoint = new List<Vector3>(_path.corners);
            for (int i = 0; i < _path.corners.Length; i++)
            {
                GameObject bject = Instantiate(@object, _path.corners[i], transform.rotation);
                if (i == _path.corners.Length - 1)
                {
                    firstTime = true;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (_destination != null)
        {
            Debug.Log(_path.status);
            Debug.Log(_path.corners.Length);

            float dist = Vector3.Distance(_destination.position, Player.transform.position);
            //float dist = _navMeshAgent.remainingDistance;
            //if (!_navMeshAgent.hasPath)
                //Debug.LogWarning(_navMeshAgent.hasPath);
            if (dist < 5 | _path.status == NavMeshPathStatus.PathPartial | _path.status == NavMeshPathStatus.PathInvalid | _path.corners.Length <= 2)
            {
                //_navMeshAgent.updatePosition = true;
                firstTime = false;
                _destination.transform.position = SelectRandomMeshPoints.GetRandomPointInsideConvex(Mesh.GetComponent<MeshFilter>().mesh);
                _destination.transform.position =  new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
            }
           
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.CalculatePath(targetVector, _path);

        if(dist>10 & _path.corners.Length > 2)
        {
            if (!firstTime)
            {
                InstancePoint();
            }
        }

            print(dist);






        }
        //    if (pastLocation == null) pastLocation = this.transform;
        //    target = wayPoint[1];


        //    Vector3 heading = target - transform.position;
        //    float angle = AngleDir(transform.forward, heading, transform.up);
        //    Debug.Log(angle - pastAngle);


        //    if (angle > 0.02F)
        //    {

        //        Vector3 center = (pastLocation.position + (pastLocation.right));
        //        float _angle = rottationSpeed * Time.deltaTime;
        //        transform.RotateAround(center, pastLocation.up, _angle);

        //    }
        //    else if (angle < -0.02F)
        //    {
        //        Vector3 center = (pastLocation.position + (-pastLocation.right));
        //        float _angle = -rottationSpeed * Time.deltaTime;
        //        transform.RotateAround(center, pastLocation.up, _angle);

        //    }
        //    else
        //    {

        //     float dist = Vector3.Distance(transform.position, target);
        //        if (dist>0.01f)
        //        {
        //            transform.position += transform.forward * Time.deltaTime * dist;
        //            pastLocation = this.transform;
        //        }
        //    }
        //    pastAngle = angle;

        //}

    }

        

    


    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir;
        return dir = Vector3.Dot(perp, up);

        //if (dir >= 1f)
        //{
        //    return 1;
        //}
        //else if (dir <= 1f)
        //{
        //    return -1;
        //}
        //else
        //{
        //    return 0;
        //}
        
    }

    private void OnDrawGizmos()
    {
        if (wayPoint.Count > 0)
            foreach (var item in wayPoint)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(new Vector3(item.x, 0, item.z), 1);
            }
        if (wayPoint.Count > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(wayPoint[1].x, 0, wayPoint[1].z), 1);
        }

    }
}
