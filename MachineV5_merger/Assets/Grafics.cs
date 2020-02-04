using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphics
{
    public class Grafics : MonoBehaviour
    {
        List<GameObject> lineRender = new List<GameObject>();

        public GameObject textField;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (lineRender.Count > 20)
            {
                Debug.LogWarning("Celaning");
                Destroy(lineRender[1]);
                lineRender.Remove(lineRender[1]);
            }
        }

        public void TextAngles(List<Vector3> crvPoints, float _angle)
        {

            GameObject line = new GameObject();
            line = Instantiate(line, transform.position, Quaternion.identity);
            lineRender.Add(line);
            LineRenderer _line1 = line.AddComponent<LineRenderer>();
            _line1.material = new Material(Shader.Find("Sprites/Default"));
            _line1.material.color = Color.red;
            _line1.widthMultiplier = 0.1f;
            _line1.positionCount = 3;
            line.transform.SetParent(this.transform);

            for (int i = 0; i < 3; i++)
            {
                _line1.SetPosition(i, crvPoints[i]);
            }

            GameObject @object = Instantiate(textField, crvPoints[1], textField.transform.rotation);
            @object.transform.parent = line.transform;
            @object.GetComponent<TextMesh>().text = _angle.ToString();
            @object.GetComponent<TextMesh>().color = Color.red;

        }
    }
}   
