using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsInitializer : MonoBehaviour
{
    private bool _loadPrefsDone;

    private LoopLists _loopLists;
    private Controller _controller;

    public LoopLists LoopLists { get => _loopLists; set => _loopLists = value; }
    public Controller Controller { get => _controller; set => _controller = value; }

    void Start()
    {
        LoopLists = GetComponent<LoopLists>();
        Controller = GetComponent<Controller>();


        ToggleNames toggleNames = FindObjectOfType<ToggleNames>();
        toggleNames.LoadPrefs();


        ToggleOrbitCircles toggleOrbitCircles = FindObjectOfType<ToggleOrbitCircles>();
        toggleOrbitCircles.LoadPrefs();

        TogglePlanetHighlight togglePlanetHighlight = FindObjectOfType<TogglePlanetHighlight>();
        togglePlanetHighlight.LoadPrefs();
    }

    private void Update()
    {
        if(LoopLists.StellarSystemGenerated && !_loadPrefsDone)
        {
            Debug.Log($"Loading Prefs");

            ToggleSetting[] _toggles = FindObjectsOfType<ToggleSetting>(true);
            foreach (ToggleSetting toggleSetting in _toggles)
            {
                toggleSetting.LoadPrefs();
            }

            SliderSetting[] _sliders = FindObjectsOfType<SliderSetting>(true);
            foreach (SliderSetting sliderSetting in _sliders)
            {
                sliderSetting.LoadPrefs();
            }

            ToggleTrails toggleTrails = FindObjectOfType<ToggleTrails>();
            toggleTrails.LoadPrefs();

            Debug.Log("Prefs loaded");
            _loadPrefsDone = true;

        }
        else if(!LoopLists.StellarSystemGenerated)
        {
            Controller.ClearTrails();
        }
        else if (!_loadPrefsDone)
        {
            Controller.ClearTrails();
        }
    }
}
