using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public enum InputType
{
    UNSET,
    BOTH,
    TOUCH,
    MOUSE,
}

public class Controller : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private bool _hasPlayer;

    [SerializeField]
    private TextMeshProUGUI _deviceInfo;

    [SerializeField]
    private InputType _inputType;

    [SerializeField]
    private AudioMixer _audioMixer;

    [SerializeField]
    private AudioSource _travelSound;

    [SerializeField]
    private float _fadeTime = 1f;

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
    public InputType InputType { get => _inputType; set => _inputType = value; }
    public TextMeshProUGUI DeviceInfo { get => _deviceInfo; set => _deviceInfo = value; }
    public AudioMixer AudioMixer { get => _audioMixer; set => _audioMixer = value; }
    public AudioSource TravelSound { get => _travelSound; set => _travelSound = value; }
    public float FadeTime { get => _fadeTime; set => _fadeTime = value; }
    public GameObject Player { get => _player; set => _player = value; }
    public bool HasPlayer { get => _hasPlayer; set => _hasPlayer = value; }
    public Camera MainCamera { get => _mainCamera; set => _mainCamera = value; }

    private void Awake()
    {


        Camera = MainCamera.GetComponent<CameraFollow>();
        
        if (HasPlayer)
        {

            //Camera.transform.parent = Player.transform;
        }
        
        UITest = GetComponent<UITest>();
        LoopLists = GetComponent<LoopLists>();
        IsPaused = false;


        float value;
        bool result = AudioMixer.GetFloat("MusicVolume", out value);

        if(result)
        {
            Debug.Log($"Music Volume: {value}");
        }

        //DeviceInfo.text = $"{SystemInfo.deviceType}";

        /*if (GameObject.FindObjectOfType<DetectMobile>().isMobile())
        {
            InputType = InputType.TOUCH;
        }*/

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            InputType = InputType.TOUCH;
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            InputType = InputType.BOTH;
        }
        else
        {
            //InputType = InputType.BOTH;
        }


    }

    // Start is called before the first frame update
    void Start()
    {
/*        Player.SetActive(true);
        Camera.gameObject.SetActive(false);*/
    }

    // Update is called once per frame
    void Update()
    {
        MouseOnUI = UITest.IsPointerOverUIElement();

        if (!HasPlayer)
        {
            if (InputType == InputType.MOUSE || InputType == InputType.BOTH)
            {
                if (!MouseOnUI)
                {
                    DetectMouseClick();
                }

            }

            if(InputType == InputType.TOUCH || InputType == InputType.BOTH)
            {
                if(Input.touchCount > 0)
                {
                    DetectTouchClick();
                }
            }
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
        foreach (Star starObject in FindObjectsOfType<Star>())
        {
            starObject.DisplayOrbitCircle.gameObject.SetActive(PlayerPrefs.GetInt("ShowOrbitCircles") != 0);
        }
    
        foreach (StellarObject stellarObject in FindObjectsOfType<StellarObject>())
        {
            stellarObject.DisplayOrbitCircle.gameObject.SetActive(PlayerPrefs.GetInt("ShowOrbitCircles") != 0);
        }
    }
    public void TogglePlanetsHighlight()
    {
        foreach (Star starObject in FindObjectsOfType<Star>())
        {
            starObject.PlanetButton.GetComponent<Image>().enabled = PlayerPrefs.GetInt("HighlightPlanetsPosition") != 0;
        }

        foreach (StellarObject stellarObject in FindObjectsOfType<StellarObject>())
        {
            stellarObject.PlanetButton.GetComponent<Image>().enabled = PlayerPrefs.GetInt("HighlightPlanetsPosition") != 0;
        }
    }
    public void ToggleTrails()
    {
        ClearTrails();

        foreach (Star starObject in FindObjectsOfType<Star>())
        {
            starObject.ObjectTrail.enabled = PlayerPrefs.GetInt("ShowTrails") != 0;
        }

        foreach (StellarObject stellarObject in FindObjectsOfType<StellarObject>())
        {
            stellarObject.ObjectTrail.enabled = PlayerPrefs.GetInt("ShowTrails") != 0;
        }
    }

    public void ClearTrails()
    {
        foreach (Star starObject in FindObjectsOfType<Star>())
        {
            starObject.ObjectTrail.Clear();
        }

        foreach (StellarObject stellarObject in FindObjectsOfType<StellarObject>())
        {
            stellarObject.ObjectTrail.Clear();
        }
    }

    public void TriggerSetScales(string message)
    {
        Debug.Log(message);
        SetScales();
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

    private void DetectTouchClick()
    {

        Ray ray = MainCamera.ScreenPointToRay(Input.GetTouch(0).position);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (hit.transform.parent.GetComponent<Star>() != null) {
                    Camera.ChangeTarget(hit.transform.parent);
                }
                else if(hit.transform.GetComponent<StellarObject>() != null)
                {
                    Camera.ChangeTarget(hit.transform);
                }
            }
        }

        else
        {

        }
    }

    private void DetectMouseClick()
    {
        Vector2 _pointerPosition;

        _pointerPosition = Input.mousePosition;

        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        // Visualize Ray on Scene (no impact on Game view)
        //Debug.DrawRay(ray.origin, ray.direction * 20f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //If mouse is on the star
            if (hit.transform.parent.GetComponent<Star>() != null)
            {
                Star star = hit.transform.parent.GetComponent<Star>();

                star.IsHovered = true;

                if (Input.GetMouseButton(0))
                {
                    Camera.ChangeTarget(star.transform);
                }

                else
                {
                    star.Animator.SetBool("ShowName", true);
                }
            }

            //If mouse is on a planet
            else if (hit.transform.GetComponent<StellarObject>() != null)
            {
                StellarObject stellarObject = hit.transform.GetComponent<StellarObject>();

                stellarObject.IsHovered = true;

                //and is clicked
                if (Input.GetMouseButtonDown(0))
                {
                    Camera.ChangeTarget(hit.transform);
                }

                else
                {
                    stellarObject.Animator.SetBool("ShowName", true);
                }
            }
        }
        else
        {

            
            foreach (StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
            {
                stellarObject.IsHovered = false;

                //stellarObject.Animator.SetBool("ShowDetails", false);
                if (PlayerPrefs.GetInt("ShowNames") == 0)
                {
                    //UIName.gameObject.SetActive(false);
                    stellarObject.Animator.SetBool("ShowName", false);
                }
            }

            foreach (Star star in GameObject.FindObjectsOfType<Star>())
            {
                star.IsHovered = false;

                //stellarObject.Animator.SetBool("ShowDetails", false);
                if (PlayerPrefs.GetInt("ShowNames") == 0)
                {
                    //UIName.gameObject.SetActive(false);
                    star.Animator.SetBool("ShowName", false);
                }
            }
        }    
    }

    public void StickToObject(Transform elemTransform, Transform targetTransform, float decalLeft)
    {
        Vector3 newPos = UnityEngine.Camera.main.WorldToScreenPoint(targetTransform.position);
        
        elemTransform.position = new Vector3(newPos.x + decalLeft, newPos.y, newPos.z);
        
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

        //RotationTime = Mathf.Max(RotationTime, 0.01f);
        rotationDegreesPerSecond = 360f / RotationTime * (inverted ? 1f : -1f);
        objTransform.Rotate(new Vector3(0, rotationDegreesPerSecond * Time.deltaTime, 0));
    }
}
