using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CreateLine : MonoBehaviour
{

    public  VisualEffect vFX;
    public static CreateLine Instance;
    public List<Vector3> knots = new List<Vector3>();
    public List<LineRenderer> _lines = new List<LineRenderer>();
   public  GameObject _LineRender;
    NoteIndicatorGroup _noteIndicatorGroup;
    KDTree kD;
    Vector3 [] points;
    
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
                CreatePoints();
                knots.Add(_noteIndicatorGroup._lights[value].transform.position);
                //_lines[0].SetPositions(Triangle());
                //_lines[1].SetPositions(Poliline(points[value]));
                //_lines[2].SetPositions(Star(points[value]));

                vFX.SetFloat("burst", 12);



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
    void CreatePoints()
    {
        int lenth = _noteIndicatorGroup._lights.Count;
        points = new Vector3[lenth];
        
        for (int i = 0; i<lenth; i++)
        {
            points[i] = _noteIndicatorGroup._lights[i].transform.position;
        }

        kD = KDTree.MakeFromPoints(points);
    }

    Vector3[] Triangle()
    {
        /// createa a random trianggle between 3 points from list
        _lines[0].positionCount = 4;
        Vector3[] points = new Vector3[4];
        if (_noteIndicatorGroup._lights.Count > 3)
        {
            int[] seqance = new int[3];
            while (seqance[0] == seqance[1] | seqance[1] == seqance[2] | seqance[0] == seqance[2])
            {
                seqance = RandomSeqance(3, _noteIndicatorGroup._lights.Count);
            }
            for (int i = 0; i < 3; i++)
            {
                points[i] = _noteIndicatorGroup._lights[seqance[i]].transform.position;
            }
            points[points.Length - 1] = points[0];
        }
        return points;
    }

    Vector3[] Star(Vector3 startPoint)
    {
        _lines[2].positionCount = 12;
        Vector3[] rValues = new Vector3[12];

        for (int i = 0; i < 12; i+=2)
        {
            int temp2 = kD.FindNearestK(startPoint, i+1);
            rValues[i] = points[temp2];
            rValues[i+1] = points[kD.FindNearest(startPoint)];
        }
        return rValues;
    }


    Vector3[] Poliline(Vector3 startPoint)
    {
        int lenth = _noteIndicatorGroup._lights.Count;
        _lines[1].positionCount = lenth;

        Vector3[] rValues = new Vector3[lenth];
        for (int i = 0; i < lenth; i++)
        {
            int temp2 = kD.FindNearestK(startPoint, i);
            rValues[i] = points[temp2];
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
        if (knots.Count > 0)
        {
            vFX.SetVector3("attractor", knots[0]);
            vFX.SetFloat("burst", 1);
        }
    }
}
