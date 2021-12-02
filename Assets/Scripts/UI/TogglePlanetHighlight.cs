using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TogglePlanetHighlight : MonoBehaviour
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
        if (!PlayerPrefs.HasKey("HighlightPlanetsPosition"))
        {
            PlayerPrefs.SetInt("HighlightPlanetsPosition", 0);
        }

        _toggle.isOn = (PlayerPrefs.GetInt("HighlightPlanetsPosition") != 0) ? true : false;
        SetTogglePlanetHighlight();
    }

    public void SetTogglePlanetHighlight()
    {
        PlayerPrefs.SetInt("HighlightPlanetsPosition", (Toggle.isOn) ? 1 : 0);
        foreach(Planet planet in FindObjectsOfType<Planet>())
        {
            if(planet.IsCreated)
            {
                planet.PlanetButton.GetComponent<Image>().enabled = PlayerPrefs.GetInt("HighlightPlanetsPosition") != 0;
            }
        }
    }
}
