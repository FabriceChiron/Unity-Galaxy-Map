using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _deviceInfo;

    private UITest _uiTest;
    private bool _isPaused, _isStellarSystemCreated, _mouseOnUI;
    private CameraFollow _camera;
    private LoopLists _loopLists;

    private float rotationDegreesPerSecond;

    public UITest UITest { get => _uiTest; set => _uiTest = value; }
    public bool IsPaused { get => _isPaused; set => _isPaused = value; }
    public CameraFollow Camera { get => _camera; set => _camera = value; }
    public bool IsStellarSystemCreated { get => _isStellarSystemCreated; set => _isStellarSystemCreated = value; }
    public bool MouseOnUI { get => _mouseOnUI; set => _mouseOnUI = value; }
    public LoopLists LoopLists { get => _loopLists; set => _loopLists = value; }

    private void Awake()
    {
        Camera = UnityEngine.Camera.main.GetComponent<CameraFollow>();
        UITest = GetComponent<UITest>();
        LoopLists = GetComponent<LoopLists>();
        IsPaused = false;

        _deviceInfo.text = $"{SystemInfo.deviceType}";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MouseOnUI = UITest.IsPointerOverUIElement();

        if (!MouseOnUI)
        {
            DetectClick();
        }

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

        foreach(AsteroidBelt asteroidBelt in FindObjectsOfType<AsteroidBelt>())
        {
            asteroidBelt.SetScales();
        }

        ClearTrails();
    }

    private void DetectClick()
    {
        
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

        // Visualize Ray on Scene (no impact on Game view)
        Debug.DrawRay(ray.origin, ray.direction * 20f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //If mouse is on the star
            if(hit.transform.GetComponent<Star>() != null)
            {
                Star star = hit.transform.GetComponent<Star>();

                if (Input.GetMouseButton(0))
                {
                    Camera.ChangeTarget(star.transform);
                }
            }

            //If mouse is on a planet
            else if(hit.transform.GetComponent<StellarObject>() != null)
            {
                StellarObject stellarObject = hit.transform.GetComponent<StellarObject>();

                stellarObject.IsHovered = true;

                //and is clicked
                if (Input.GetMouseButtonDown(0))
                {

                    //if the camera is alreay focused on the planet or moon
                    /*if (Camera.CameraTarget == hit.transform)
                    {


                        //Set the animator boolean to true, which will start the animation to show the details
                        stellarObject.Animator.SetBool("ShowDetails", !stellarObject.Animator.GetBool("ShowDetails"));

                        //And hide the name
                        //UIName.gameObject.SetActive(false);
                        stellarObject.Animator.SetBool("ShowName", false);
                        Camera.CameraAnchor = stellarObject.CameraAnchor;
                        Camera.CameraAnchorObject = gameObject;
                    }

                    //else
                    else
                    {
                        //change the camera focus to the planet
                    }*/

                    Camera.ChangeTarget(hit.transform);

                }

                //and is not clicked and the details are not shown
                /*else if (!stellarObject.Animator.GetBool("ShowDetails"))
                {
                    stellarObject.Animator.SetBool("ShowName", true);
                }*/
                else
                {
                    stellarObject.Animator.SetBool("ShowName", true);
                }
            }
        }
        else
        {
            foreach(StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
            {
                stellarObject.IsHovered = false;

                //stellarObject.Animator.SetBool("ShowDetails", false);
                if (PlayerPrefs.GetInt("ShowNames") == 0)
                {
                    //UIName.gameObject.SetActive(false);
                    stellarObject.Animator.SetBool("ShowName", false);
                }
            }
        }
    }

    public float GetOrbitOrientationStart(int index, int arrayLength)
    {
        float OrientationStart = ((float)index / (float)arrayLength) * 360f;

        return OrientationStart;
    }

    public float GetOrbitOrientationStart(string Coords)
    {
        float OrientationStart;

        switch (Coords)
        {
            case "n":
                OrientationStart = 0f;
                break;

            case "ne":
                OrientationStart = 45f;
                break;

            case "e":
                OrientationStart = 90f;
                break;

            case "se":
                OrientationStart = 135f;
                break;

            case "s":
                OrientationStart = 180f;
                break;

            case "sw":
                OrientationStart = 225f;
                break;

            case "w":
                OrientationStart = 270f;
                break;

            case "nw":
                OrientationStart = 315f;
                break;

            default:
                OrientationStart = Random.value * 360f;
                break;
        }

        return OrientationStart;
    }

    public void RotateObject(Transform objTransform, float RotationTime, bool inverted)
    {

        RotationTime = Mathf.Max(RotationTime, 0.01f);
        rotationDegreesPerSecond = 360f / RotationTime * (inverted ? 1f : -1f);
        objTransform.Rotate(new Vector3(0, rotationDegreesPerSecond * Time.deltaTime, 0));
    }
}
