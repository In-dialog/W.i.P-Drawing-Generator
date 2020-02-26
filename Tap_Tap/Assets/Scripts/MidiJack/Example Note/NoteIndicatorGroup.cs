using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NoteIndicatorGroup : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> _lights = new List<GameObject>();
    public bool isMouseDragging;
    GameObject target;
    Vector3 screenPosition;
    Vector3 offset;
    GameObject parent;
   

    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (var item in _lights)
            {
                item.GetComponent<NoteIndicator>().SaveData();
                PlayerPrefs.SetInt("NumberOfPoints", _lights.Count);
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var item in _lights)
            {
                item.GetComponent<NoteIndicator>().LoadData();
            }
            GetComponent<CreateLine>()._lines[1].positionCount = 0;
            GetComponent<CreateLine>().knots.Clear();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        }
        if (Input.GetKeyDown(KeyCode.R)& Input.GetKeyDown(KeyCode.T))
        {
            PlayerPrefs.SetInt("NumberOfPoints", 0);


        }
        if (_lights.Count <= 0)
        {
            int NumberOfPoints = PlayerPrefs.GetInt("NumberOfPoints", _lights.Count);
            if (NumberOfPoints == 0) NumberOfPoints = 1;
            CreateNotes(PlayerPrefs.GetInt("NumberOfPoints", NumberOfPoints));
        }
        if (Input.GetMouseButton(0)& Input.GetKeyDown(KeyCode.Space)) AddMote(_lights.Count);
        MoveObjcet();
    }

    void CreateNotes(int numberOfPoints)
    {
        if (parent == null) parent = new GameObject("Light");
        for (var i = 0; i < numberOfPoints; i++)
        {

            var go = Instantiate<GameObject>(prefab);
            go.transform.position = new Vector3(i % 12, i / 12, 0);
            go.GetComponent<NoteIndicator>().noteNumber = i;
            go.transform.SetParent(parent.transform);
            go.name = i.ToString();
            _lights.Add(go);
        }
    }
    void AddMote(int numberOfPoints)
    {
        if(parent==null) parent = new GameObject("Light");
        print("add");
        var go = Instantiate<GameObject>(prefab);
        go.transform.position = new Vector3(CalculateMousePosition().x, CalculateMousePosition().y, 0);
        go.GetComponent<NoteIndicator>().noteNumber = numberOfPoints;
        go.transform.SetParent(parent.transform);
        go.name = numberOfPoints.ToString();
        _lights.Add(go);

    }

    void MoveObjcet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);
            if (target != null)
            {
                isMouseDragging = true;
                Debug.Log("our target position :" + target.transform.position);
                //Here we Convert world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
                offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
        }

        if (isMouseDragging)
        {
            //tracking mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            //It will update target gameobject's current postion.
            target.transform.position = CalculateMousePosition();
        }

    }
    Vector3 CalculateMousePosition(){
        Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

        //convert screen position to world position with offset changes.
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
        return currentPosition; 
    }

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject targetObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            targetObject = hit.collider.gameObject;
        }
        return targetObject;
    }
}
