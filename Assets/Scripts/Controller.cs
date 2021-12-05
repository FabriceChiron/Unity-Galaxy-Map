using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{

    [SerializeField]
    private UITest _uiTest;

    public UITest UITest { get => _uiTest; set => _uiTest = value; }

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEscapePressed())
        {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
    bool IsEscapePressed()
    {
        #if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null ? Keyboard.current.escapeKey.isPressed : false; 
        #else
        return Input.GetKey(KeyCode.Escape);
        #endif
    }

    public void ToggleOrbitCircles()
    {
        foreach(Planet planet in FindObjectsOfType<Planet>())
        {
            planet.DisplayOrbitCircle.gameObject.SetActive(PlayerPrefs.GetInt("ShowOrbitCircles") != 0);
        }
    }
    public void TogglePlanetsHighlight()
    {
        foreach(Planet planet in FindObjectsOfType<Planet>())
        {
            planet.PlanetButton.GetComponent<Image>().enabled = PlayerPrefs.GetInt("HighlightPlanetsPosition") != 0;
        }
    }

    public void TogglePause(bool IsPaused)
    {
        foreach (Planet planet in FindObjectsOfType<Planet>())
        {
            planet.IsPaused = IsPaused;
        }
    }

    public void ClearTrails()
    {
        foreach (Planet planet in FindObjectsOfType<Planet>())
        {
            planet.PlanetTrail.Clear();
        }
    }

    public void SetScales()
    {

        //GameObject.FindGameObjectWithTag("Star").GetComponent<Star>().SetScales();

        ClearTrails();

        foreach (Planet planet in FindObjectsOfType<Planet>())
        {
            planet.SetScales();
        }

        ClearTrails();
    }
}
