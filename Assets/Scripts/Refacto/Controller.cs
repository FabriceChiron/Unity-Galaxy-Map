using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{
    private UITest _uiTest;

    private bool _isPaused, _isStellarSystemCreated;

    private CameraFollow _camera;
    public UITest UITest { get => _uiTest; set => _uiTest = value; }
    public bool IsPaused { get => _isPaused; set => _isPaused = value; }
    public CameraFollow Camera { get => _camera; set => _camera = value; }
    public bool IsStellarSystemCreated { get => _isStellarSystemCreated; set => _isStellarSystemCreated = value; }

    private void Awake()
    {
        Camera = UnityEngine.Camera.main.GetComponent<CameraFollow>();
        UITest = GetComponent<UITest>();
        IsPaused = false;
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
        foreach (StellarObject stellarObject in FindObjectsOfType<StellarObject>())
        {
            stellarObject.DisplayOrbitCircle.gameObject.SetActive(PlayerPrefs.GetInt("ShowOrbitCircles") != 0);
        }
    }
    public void TogglePlanetsHighlight()
    {
        foreach (StellarObject stellarObject in FindObjectsOfType<StellarObject>())
        {
            stellarObject.PlanetButton.GetComponent<Image>().enabled = PlayerPrefs.GetInt("HighlightPlanetsPosition") != 0;
        }
    }
    public void ToggleTrails()
    {
        ClearTrails();

        foreach (StellarObject stellarObject in FindObjectsOfType<StellarObject>())
        {
            stellarObject.ObjectTrail.enabled = PlayerPrefs.GetInt("ShowTrails") != 0;
        }
    }

    public void ClearTrails()
    {
        foreach (StellarObject stellarObject in FindObjectsOfType<StellarObject>())
        {
            stellarObject.ObjectTrail.Clear();
        }
    }

    public void SetScales()
    {
        ClearTrails();

        foreach (Star star in FindObjectsOfType<Star>())
        {
            star.SetScales();
        }

        foreach (StellarObject stellarObject in FindObjectsOfType<StellarObject>())
        {
            stellarObject.SetScales();
        }

        ClearTrails();
    }
}
