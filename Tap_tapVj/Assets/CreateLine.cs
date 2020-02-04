using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreateLine : MonoBehaviour
{
   

    public static CreateLine Instance;
    public List<Vector3>knots= new List<Vector3>();
    public List<LineRenderer> _lines = new List<LineRenderer>();

    NoteIndicatorGroup _noteIndicatorGroup;
    
    void Awake()
    {
        GameObject children;
        for (int i = 0; i < 3; i++)
        {
            children = new GameObject("LineRender" + i);
            children.transform.SetParent(this.transform);
            _lines[i] = children.AddComponent<LineRenderer>();
            _lines[i].startWidth = 0.05f;
            _lines[i].endWidth = 0.005f;


        }
        _noteIndicatorGroup = GetComponent<NoteIndicatorGroup>();


    }
    public int SetKnot
    {

        set
        {
            //print("jj");
            if (!knots.Contains(_noteIndicatorGroup._lights[value].transform.position) & _noteIndicatorGroup._lights.Count >= 0)
            {
                knots.Add(_noteIndicatorGroup._lights[value].transform.position);
                //Triangle();
            }
        }
    }
    public int RemoveKnot
    {
        set
        {
            //print("jj2");
            if (knots.Contains(_noteIndicatorGroup._lights[value].transform.position) & _noteIndicatorGroup._lights.Count >= 0)
            {
                StartCoroutine(ClearElement(value, 0.5f));
                _lines[2].SetPositions(MakeStar(value));
            }
        }
    }
    IEnumerator ClearElement(int value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        knots.Remove(_noteIndicatorGroup._lights[value].transform.position);

    }
    void Triangle()
    {
        if (_noteIndicatorGroup._lights.Count > 3)
        {
            _lines[2].positionCount = 4;
            int[] seqance = new int[3];
            while (seqance[0] == seqance[1] | seqance[1] == seqance[2] | seqance[0] == seqance[2])
            {
                seqance = RandomSeqance(3, _noteIndicatorGroup._lights.Count);
                //Debug.Log(seqance);
            }
            for (int i = 0; i < 3; i++)
            {
                _lines[1].SetPosition(i, _noteIndicatorGroup._lights[seqance[i]].transform.position);
            }
            _lines[1].SetPosition(3, _lines[1].GetPosition(0));

        }
    }
    Vector3[] MakeStar(int value)
    {
        Vector3[] rValues = new Vector3[3];
        _lines[2].positionCount = 3;

        Vector3[] points = new Vector3[_noteIndicatorGroup._lights.Count];

        for (int i = 0; i < _noteIndicatorGroup._lights.Count; i++)
        {
            points[i] = _noteIndicatorGroup._lights[i].transform.position;
        }
        for (int i = 0; i < 3; i++)
        {
            KDTree kD = KDTree.MakeFromPoints(points);
            int temp2 = kD.FindNearest(points[value]);
            Vector3 temp = points[temp2];
            points[temp2] = Vector3.zero;
            rValues[i] = temp;
        }
        return rValues;

    }

    int [] RandomSeqance(int count , int max)
    {
        int[] seqance = new int[count];
        for (int i = 0; i < count; i++)
        {
            seqance[i] = Random.RandomRange(0, max);
        }
        return seqance;
    }

    // Update is called once per frame
    void Update()
    {
        //_lines[0].positionCount = knots.Count;
        //for (int i = 0; i < _lines[0].positionCount; i++)
        //{
        //    _lines[0].SetPosition(i,knots[i]);
        //}
    }
}
