using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Explorer : MonoBehaviour
{
    Transform pastLocation;
    Vector3 center;
    float rottationSpeed=5;
    int   active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 2;
        for (int i = -3; i < 3; i++)
        {
            RaycastHit hit;
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            direction = Quaternion.AngleAxis(i * 30, Vector3.up) * direction * 20;
            var ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out hit, 10))
            {
                int p = -1;
                //if (pastLocation == null) pastLocation = transform;
                if (i < 0)
                {
                    p = 1;
                }
                Debug.Log("Hit");
                float _angle = p * rottationSpeed * Time.deltaTime * hit.distance;
                transform.RotateAround(center, pastLocation.up, _angle);
            }
            
            Debug.DrawRay(transform.position, direction, Color.yellow);

        }
    
        pastLocation = this.transform;
        center = (pastLocation.position + (pastLocation.right));
        transform.position += transform.forward * Time.deltaTime * 10f;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(center.x, 0, center.z), 1);
    }
}
