using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour
{


    public static CreateLine Instance;
    public List<Vector3> knots = new List<Vector3>();
    public List<LineRenderer> _lines = new List<LineRenderer>();
   public  GameObject _LineRender;
    NoteIndicatorGroup _noteIndicatorGroup;

    public List<Material> mat = new List<Material>();

    void Awake()
    {
        GameObject children;
        for (int i = 0; i < 4; i++)
        {
            children = Instantiate(_LineRender);
            children.name ="LineRender" + i;
            children.transform.SetParent(this.transform);

            _lines[i] = children.GetComponent<LineRenderer>();
            _lines[i].material = mat[i];
            //_lines[i].startWidth = i/10;
            //_lines[i].endWidth =i/100;


            //_lines[i].positionCount = 4;

        }
        _lines[1].startWidth = 0.009f;
        _lines[1].endWidth = 0.004f;
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
                _lines[0].SetPositions(Triangle());
                _lines[1].SetPositions(Poliline(value));
                //_lines[2].SetPositions(Star(value));



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
            }
        }
    }
    IEnumerator ClearElement(int value, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        knots.Remove(_noteIndicatorGroup._lights[value].transform.position);

    }
    Vector3[] Triangle()
    {
        /// createa a random trianggle between 3 points from list
        _lines[0].positionCount = 4;
        Vector3[] points = new Vector3[4];
        if (_noteIndicatorGroup._lights.Count > 3)
        {
            //_lines[1].positionCount = 4;
            int[] seqance = new int[3];
            while (seqance[0] == seqance[1] | seqance[1] == seqance[2] | seqance[0] == seqance[2])
            {
                seqance = RandomSeqance(3, _noteIndicatorGroup._lights.Count);
            }
            for (int i = 0; i < 3; i++)
            {
                points[i] = _noteIndicatorGroup._lights[seqance[i]].transform.position;
                //_lines[1].SetPosition(i, _noteIndicatorGroup._lights[seqance[i]].transform.position);
            }
            points[points.Length - 1] = points[0];
            //_lines[1].SetPosition(3, _lines[1].GetPosition(0));
        }
        return points;
    }

    Vector3[] Star(int value)
    {
        int lenth = _noteIndicatorGroup._lights.Count;
        _lines[2].positionCount = 12;

        Vector3[] rValues = new Vector3[12];
        Vector3[] points = new Vector3[lenth];

        for (int i = 0; i < lenth; i++)
        {
            points[i] = _noteIndicatorGroup._lights[i].transform.position;
        }
    
            KDTree kD = KDTree.MakeFromPoints(points);

        for (int i = 0; i < 12; i+=2)
        {
            int temp2 = kD.FindNearestK(points[value], i+1);
            rValues[i] = points[temp2];
            rValues[i+1] = points[kD.FindNearest(points[value])];

        }

        //    int temp2 = kD.FindNearestK(points[value], 1);
        //    rValues[0] = points[temp2];

        //     temp2 = kD.FindNearestK(points[value], 2);
        //     rValues[1] = points[temp2];

        //temp2 = kD.FindNearestK(points[value], 3);
        //rValues[2] = points[temp2];
        //points[temp2] = Vector3.zero;
        //rValues[i] = temp;

        return rValues;
    }


    Vector3[] Poliline(int value)
    {
        int lenth = _noteIndicatorGroup._lights.Count;
        print(lenth+"   s");
        _lines[1].positionCount = lenth;

        Vector3[] rValues = new Vector3[lenth];
        Vector3[] points = new Vector3[lenth];

        for (int i = 0; i < lenth; i++)
        {
            points[i] = _noteIndicatorGroup._lights[i].transform.position;
        }
        KDTree kD = KDTree.MakeFromPoints(points);

        for (int i = 0; i < lenth; i++)
        {
            int temp2 = kD.FindNearestK(points[value],i);
            Debug.Log("index found "+temp2 );
            Debug.Log("curent  index " + i);

            if(temp2>0)
            rValues[i] = points[temp2];
      
            //points[temp2] = Vector3.zero;
            //rValues[i] = temp;
        }
        return rValues;

    }

    int[] RandomSeqance(int count, int max)
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
        //_lines[3].positionCount = knots.Count;
        //for (int i = 0; i < _lines[2].positionCount; i++)
        //{
        //    _lines[3].SetPosition(i, knots[i]);
        //}
    }
}
