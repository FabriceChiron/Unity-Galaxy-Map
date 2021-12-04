using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleTrails : MonoBehaviour
{
    private Toggle _toggle;
    public Toggle Toggle { get => _toggle; set => _toggle = value; }
    private Controller _controller;

    private void Awake()
    {
        Toggle = GetComponent<Toggle>();
        _controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
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
        if (!PlayerPrefs.HasKey("ShowTrails"))
        {
            PlayerPrefs.SetInt("ShowTrails", 0);
        }

        _toggle.isOn = (PlayerPrefs.GetInt("ShowTrails") != 0) ? true : false;
    }

    public void SetToggleTrails()
    {

        PlayerPrefs.SetInt("ShowTrails", (Toggle.isOn) ? 1 : 0);

        foreach(Planet planet in FindObjectsOfType<Planet>())
        {
            if(planet.IsCreated && planet.TrailStartTime - Time.deltaTime <= Time.time)
            {
                planet.PlanetTrail.enabled = PlayerPrefs.GetInt("ShowTrails") != 0;
            }
        }
    }
}
