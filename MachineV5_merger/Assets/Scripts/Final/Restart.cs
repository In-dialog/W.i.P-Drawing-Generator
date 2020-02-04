using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Restart : MonoBehaviour
{
   public  Slider[] sliders;
    public InputField maxPointsText;
	void Update ()
	{
		if( Input.GetKeyDown(KeyCode.R) )
		{
            TheRestart();
        }
        if (FindObjectOfType<PlacePoints>()._case == 3) maxPointsText.transform.gameObject.SetActive(false);
        else maxPointsText.transform.gameObject.SetActive(true);
    }

    private void Start()
    {
        LoadData();
        maxPointsText.text = FindObjectOfType<PlacePoints>().maxPoints.ToString();

    }



    public void TheRestart()
    {
        SaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SaveData()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            PlayerPrefs.SetFloat(i.ToString(), sliders[i].value);
            //if(sliders.Length-1 == i)
                
        }
        PlayerPrefs.SetInt("20", int.Parse(maxPointsText.text));
    }
    void LoadData()
    {
        FindObjectOfType<PlacePoints>().maxPoints = PlayerPrefs.GetInt("20", 0); 
        //sliders = FindObjectsOfType<Slider>();
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = PlayerPrefs.GetFloat(i.ToString(), sliders[i].value);

            if (sliders[i].name == "CaseSlider" & FindObjectOfType<PlacePoints>().enabled == true)
            {
                FindObjectOfType<PlacePoints>()._case = (int)sliders[i].value;
                sliders[i].GetComponentInChildren<Text>().text = sliders[i].value.ToString();
               

            }

            if (sliders[i].name == "MovingSpeed" & FindObjectOfType<SimpleNover>().enabled == true)
            {
                FindObjectOfType<SimpleNover>().maxSpeed = (int)sliders[i].value;
                sliders[i].GetComponentInChildren<Text>().text = sliders[i].value.ToString();
            }

            if (sliders[i].name == "RotateSpeed" & FindObjectOfType<SimpleNover>().enabled == true)
            {
                FindObjectOfType<SimpleNover>().maxSpeed = (int)sliders[i].value;
                sliders[i].GetComponentInChildren<Text>().text = sliders[i].value.ToString();
            }

            if (sliders[i].name == "MaxX" & FindObjectOfType<PlacePoints>().enabled == true)
            {
                FindObjectOfType<PlacePoints>().maxX = (int)sliders[i].value;
                sliders[i].GetComponentInChildren<Text>().text = sliders[i].value.ToString();

            }
            if (sliders[i].name == "MaxY" & FindObjectOfType<PlacePoints>().enabled == true)
            {
                FindObjectOfType<PlacePoints>().maxZ = (int)sliders[i].value;
                sliders[i].GetComponentInChildren<Text>().text = sliders[i].value.ToString();
            }
        }
    }

    public void ValueChange(Slider _sliders)
    {
        //SaveData();
        if (_sliders.name == "MovingSpeed" & FindObjectOfType<SimpleNover>().enabled == true)
        {
            FindObjectOfType<SimpleNover>().maxSpeed = (int)_sliders.value;
            _sliders.GetComponentInChildren<Text>().text = _sliders.value.ToString();
        }

        if (_sliders.name == "RotateSpeed" & FindObjectOfType<SimpleNover>().enabled == true)
        {
            FindObjectOfType<SimpleNover>().rottationSpeed = (int)_sliders.value;
            _sliders.GetComponentInChildren<Text>().text = _sliders.value.ToString();
        }

        if (_sliders.name == "MaxX" & FindObjectOfType<PlacePoints>().enabled == true)
        {
            FindObjectOfType<PlacePoints>().maxX = (int)_sliders.value;
            _sliders.GetComponentInChildren<Text>().text = _sliders.value.ToString();
        }

        if (_sliders.name == "MaxY" & FindObjectOfType<PlacePoints>().enabled == true)
        {
            FindObjectOfType<PlacePoints>().maxZ = (int)_sliders.value;
            _sliders.GetComponentInChildren<Text>().text = _sliders.value.ToString();
        }

        if (_sliders.name == "CaseSlider" & FindObjectOfType<PlacePoints>().enabled == true)
        {
            _sliders.GetComponentInChildren<Text>().text = _sliders.value.ToString();
        }
    }


    public void ValueChange(InputField inputField)
    {
            FindObjectOfType<PlacePoints>().maxPoints = int.Parse(inputField.text);
    }



}
