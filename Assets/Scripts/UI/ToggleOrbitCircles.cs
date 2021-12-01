using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleOrbitCircles : MonoBehaviour
{
    private Toggle _toggle;
    public Toggle Toggle { get => _toggle; set => _toggle = value; }

    private void Awake()
    {
        Toggle = GetComponent<Toggle>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPrefs()
    {
        if (!PlayerPrefs.HasKey("ShowOrbitCircles"))
        {
            PlayerPrefs.SetInt("ShowOrbitCircles", 0);
        }

        _toggle.isOn = (PlayerPrefs.GetInt("ShowOrbitCircles") != 0) ? true : false;
    }

    public void SetToggleOrbitCircles()
    {
        PlayerPrefs.SetInt("ShowOrbitCircles", (Toggle.isOn) ? 1 : 0);
        foreach(Planet planet in FindObjectsOfType<Planet>())
        {
            if(planet.IsCreated)
            {
                planet.DisplayOrbitCircle.gameObject.SetActive(PlayerPrefs.GetInt("ShowOrbitCircles") != 0);
            }
        }
    }
}
