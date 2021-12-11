﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private ScaleSettings _scaleSettings;

    [SerializeField]
    private PlanetData _planetData;

    [SerializeField]
    private TextMeshPro _objectNameDisplay;

    [SerializeField]
    private Canvas _UIElements;

    [SerializeField]
    private Transform _clouds;

    [SerializeField]
    private Transform _cameraAnchor, _displayOrbitCircle;

    [SerializeField]
    private TrailRenderer _planetTrail;

    [SerializeField]
    private GeneratePlanets _generatePlanets;

    [SerializeField]
    private TextMeshProUGUI _UIName;

    [SerializeField]
    private PlanetButton _planetButton;

    [SerializeField]
    private Image _UIDetails;

    [SerializeField]
    private string _parentStellarObject, _objectType;

    [SerializeField]
    private float _calculatedOrbitSize, _calculatedObjectSize;

    private float _revolutionTime, _rotationTime, _objectSize, _orbitSize, _bodyTiltAngle, _orbitTiltAngle;

    private Texture _textureMaterial;

    private Material _material;

    private string _coords;

    private bool _gaseous, _hasClouds, _isTidallyLocked, _isCreated, _isPaused, _needsAdjust, _mouseOnUI, _isHovered, _isOnScreen;

    private float revolutionDegreesPerSecond, rotationDegreesPerSecond, _orientationStart, _orbitSizeAdjust, _trailStartTime;


    private Transform _stellarObject, _stellarAnchor, _orbit, _orbitAnchor, _cameraPosition, _star;

    private Collider _collider;
    private Renderer _renderer;

    private MeshRenderer _meshRenderer;

    private CameraFollow _camera;
    private Animator _animator;
    private UITest _UITest;
    private StellarSystemData _stellarSystemData;

    public float RevolutionTime { get => _revolutionTime; set => _revolutionTime = value; }
    public float RotationTime { get => _rotationTime; set => _rotationTime = value; }
    public float ObjectSize { get => _objectSize; set => _objectSize = value; }
    public float OrbitSize { get => _orbitSize; set => _orbitSize = value; }
    public float BodyTiltAngle { get => _bodyTiltAngle; set => _bodyTiltAngle = value; }
    public float OrbitTiltAngle { get => _orbitTiltAngle; set => _orbitTiltAngle = value; }
    public float OrientationStart { get => _orientationStart; set => _orientationStart = value; }

    

    public Transform StellarObject { get => _stellarObject; set => _stellarObject = value; }
    public Transform Orbit { get => _orbit; set => _orbit = value; }
    public Transform StellarAnchor { get => _stellarAnchor; set => _stellarAnchor = value; }
    public Transform OrbitAnchor { get => _orbitAnchor; set => _orbitAnchor = value; }
    public TextMeshProUGUI UIName { get => _UIName; set => _UIName = value; }
    public Image UIDetails { get => _UIDetails; set => _UIDetails = value; }
    public Renderer Renderer { get => _renderer; set => _renderer = value; }
    public Collider Collider { get => _collider; set => _collider = value; }
    public PlanetData PlanetData { get => _planetData; set => _planetData = value; }
    public Transform Star { get => _star; set => _star = value; }
    public Transform CameraAnchor { get => _cameraAnchor; set => _cameraAnchor = value; }
    public Transform DisplayOrbitCircle { get => _displayOrbitCircle; set => _displayOrbitCircle = value; }
    public GeneratePlanets GeneratePlanets { get => _generatePlanets; set => _generatePlanets = value; }
    public string ParentStellarObject { get => _parentStellarObject; set => _parentStellarObject = value; }
    public string ObjectType { get => _objectType; set => _objectType = value; }
    public float CalculatedOrbitSize { get => _calculatedOrbitSize; set => _calculatedOrbitSize = value; }
    public float CalculatedObjectSize { get => _calculatedObjectSize; set => _calculatedObjectSize = value; }
    public float OrbitSizeAdjust { get => _orbitSizeAdjust; set => _orbitSizeAdjust = value; }
    public ScaleSettings ScaleSettings { get => _scaleSettings; set => _scaleSettings = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public UITest UITest { get => _UITest; set => _UITest = value; }
    public bool MouseOnUI { get => _mouseOnUI; set => _mouseOnUI = value; }
    public bool IsCreated { get => _isCreated; set => _isCreated = value; }
    public bool IsPaused { get => _isPaused; set => _isPaused = value; }
    public bool IsHovered { get => _isHovered; set => _isHovered = value; }
    public bool IsOnScreen { get => _isOnScreen; set => _isOnScreen = value; }
    public TrailRenderer PlanetTrail { get => _planetTrail; set => _planetTrail = value; }
    public float TrailStartTime { get => _trailStartTime; set => _trailStartTime = value; }
    public CameraFollow Camera { get => _camera; set => _camera = value; }
    public PlanetButton PlanetButton { get => _planetButton; set => _planetButton = value; }
    public MeshRenderer MeshRenderer { get => _meshRenderer; set => _meshRenderer = value; }
    public StellarSystemData StellarSystemData { get => _stellarSystemData; set => _stellarSystemData = value; }

    private void Awake()
    {
        Star = GameObject.FindGameObjectWithTag("Star").transform;
        UITest = GameObject.FindGameObjectWithTag("StellarSystem").GetComponent<UITest>();
        TrailStartTime = 1.5f;
    }

    public void OnCreation()
    {
        
        Camera = UnityEngine.Camera.main.GetComponent<CameraFollow>();

        OrbitSizeAdjust = 1f;

        //Debug.Log($"_planetData.name: {_planetData.name}");

        name = _planetData.name;

        ScaleSettings = GetComponent<ScaleSettings>();

        //Debug.Log($"{name} has rings: {PlanetData.Rings}");

        if(!PlanetData.Rings)
        {
            Destroy(transform.Find("Rings").gameObject);
        }

        _cameraPosition = GameObject.FindGameObjectWithTag("CameraPosition").transform;

        StellarObject = GetComponent<Transform>(); //The planet
        Renderer = GetComponent<Renderer>();

        //UIName = _UIElements.FindObjectOfType<TextMeshProUGUI>(true);
        //UIDetails = _UIElements.transform.Find("Details").gameObject;

        UIName.text = PlanetData.name;
        UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[0].text = PlanetData.name;
        UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[1].text =
            $"<b>Orbit</b>: {PlanetData.Orbit} AU\n" +
            $"<b>Radius</b>: {PlanetData.Size * 6378f}kms ({PlanetData.Size} of Earth's)\n" +
            $"<b>Orbital Period</b>: {PlanetData.YearLength} Earth year(s)\n" +
            $"<b>Rotation Period</b>: {PlanetData.DayLength} Earth day(s)\n\n" +
            $"{PlanetData.Details}";

        UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[1].GetComponent<RectTransform>().position = Vector3.zero;


        //UIName.gameObject.SetActive(false);
        //UIDetails.gameObject.SetActive(false);
        //UIDetails.transform.Find("Scrollbar Vertical").gameObject.SetActive(true);



        //Define the collider for click detection
        Collider = GetComponent<Collider>();

        StellarAnchor = StellarObject.parent.transform; //"PlanetAnchor" GO.transform




        if (PlanetData.DayLength == 0)
        {
            _isTidallyLocked = true;
        }

        Orbit = StellarAnchor.parent.transform; //"Orbit" GO.transform

        OrbitAnchor = Orbit.parent.transform;  //"XX - OrbitAnchor" GO.transform;

        Animator = OrbitAnchor.GetComponent<Animator>();

        OrbitAnchor.localPosition = Vector3.zero;

        PlanetButton.GetComponent<Image>().enabled = PlayerPrefs.GetInt("HighlightPlanetsPosition") != 0;
        DisplayOrbitCircle.gameObject.SetActive(PlayerPrefs.GetInt("ShowOrbitCircles") != 0);
        PlanetTrail.enabled = PlayerPrefs.GetInt("ShowTrails") != 0;

        if (ObjectType == "moon")
        {
            PlanetButton.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 10f);
            UIName.fontSize = 14;
            UIName.fontStyle = (FontStyles)FontStyle.Normal;
            Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        else
        {
            Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        SetScales();
        SetOrbit();
        SetStellarObject();

        IsCreated = true;
    }

    // Start is called before the first frame update
/*    void Start()
    {
        
    }*/

    // Update is called once per frame
    void Update()
    {
        MouseOnUI = UITest.IsPointerOverUIElement();

        //Debug.Log(UITest.IsPaused);

        if (IsCreated)
        {
            /*if(TrailStartTime - Time.deltaTime <= Time.time)
            {
                PlanetTrail.enabled = PlayerPrefs.GetInt("ShowTrails") != 0;
            }*/

            if(!MouseOnUI)
            {
                DetectClick();
            }
            //AdjustOrbit();
            //SetScales();

            if (!IsPaused)
            {
                PlanetRevolution();
                PlanetRotation();
            }

            CameraAnchor.parent.LookAt(GameObject.FindGameObjectWithTag("Star").transform.position);

            CheckIfOnScreen();

        }

        PlanetButton.transform.position = UnityEngine.Camera.main.WorldToScreenPoint(_stellarObject.position);
 
        UIName.transform.position = UnityEngine.Camera.main.WorldToScreenPoint(_stellarObject.position);


        if(PlayerPrefs.GetInt("ShowNames") == 1 && IsOnScreen)
        {
            //UIName.gameObject.SetActive(true);
            Animator.SetBool("ShowName", true);
        }
        else if(!IsHovered)
        {
            Animator.SetBool("ShowName", false);
        }


    }

    private void CheckIfOnScreen()
    {
        Vector3 screenPoint = UnityEngine.Camera.main.WorldToViewportPoint(StellarObject.position);
        IsOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    private void PlanetRevolution()
    {

        RevolutionTime = Mathf.Max(PlanetData.YearLength, 0.5f) * ScaleSettings.Year;        

        RevolutionTime = Mathf.Max(RevolutionTime, 0.01f);
        revolutionDegreesPerSecond = 360 / RevolutionTime * -1f;

        StellarAnchor.Rotate(new Vector3(0, -revolutionDegreesPerSecond * Time.deltaTime, 0));

        Orbit.Rotate(new Vector3(0, revolutionDegreesPerSecond * Time.deltaTime, 0));
        PlanetTrail.time = RevolutionTime * .75f;
    }

    public void PlanetRotation()
    {
        bool inverted;
        if(_isTidallyLocked)
        {
            inverted = Mathf.Abs(PlanetData.YearLength) != PlanetData.YearLength;
            RotationTime = Mathf.Abs(Mathf.Max(PlanetData.YearLength, 0.5f)) * ScaleSettings.Year;
        }
        else
        {
            inverted = Mathf.Abs(PlanetData.DayLength) != PlanetData.DayLength;
            RotationTime = Mathf.Abs(PlanetData.DayLength) * ScaleSettings.Day;
        }

        RotateObject(StellarObject, RotationTime, inverted);
        

        
        if (_hasClouds)
        {
            RotateObject(_clouds, RotationTime / 0.2f, inverted);
        }

    }

    private void RotateObject(Transform objTransform, float RotationTime, bool inverted)
    {

        RotationTime = Mathf.Max(RotationTime, 0.01f);
        rotationDegreesPerSecond = 360f / RotationTime * (inverted ? 1f : -1f);
        objTransform.Rotate(new Vector3(0, rotationDegreesPerSecond * Time.deltaTime, 0));
    }

    public void SetOrbit()
    {
        OrbitTiltAngle = PlanetData.OrbitTilt;

        GetOrbitOrientationStart(PlanetData.Coords);

        //Set the orbit's plane
        OrbitAnchor.rotation = Quaternion.Euler(OrbitTiltAngle, OrientationStart, 0f);

        /*Orbit.localScale *= OrbitSize;
        StellarAnchor.localScale /= OrbitSize;*/

    }

    private void GetOrbitOrientationStart(string Coords)
    {
        switch(Coords)
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

        //OrientationStart = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"{name} collides with {collision.transform.name}");
        if (ParentStellarObject == collision.transform.name)
        {
            Debug.Log($"{name} collides with {collision.transform.name}");
            _needsAdjust = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (ParentStellarObject == collision.transform.name)
        {
            //Debug.Log($"{name} is out of {collision.transform.name}");
            _needsAdjust = false;
        }
    }

    private void AdjustOrbit()
    {
        if (_needsAdjust) { 
            OrbitSizeAdjust *= ScaleSettings.stellarScales.Orbit * 0.25f;
        }
    }

    public void SetScales()
    {
        /*        if (!ScaleSettings.stellarScales.RationalizeValues)
                {
                    GameObject.FindGameObjectWithTag("Star").transform.localScale = new Vector3(50f, 50f, 50f);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Star").transform.localScale = new Vector3(5f, 5f, 5f);
                }*/

        //Debug.Log($"{name}: {transform.parent.GetComponentInParent<GeneratePlanets>().name}");

        StellarSystemData = GameObject.FindGameObjectWithTag("StellarSystem").GetComponent<GeneratePlanets>().StellarSystemData;

        //Debug.Log(StellarSystemData.StarSize);

        if (!ScaleSettings.stellarScales.RationalizeValues)
        {
            GameObject.FindGameObjectWithTag("Star").transform.localScale = new Vector3(StellarSystemData.StarSize * ScaleSettings.stellarScales.Planet, StellarSystemData.StarSize * ScaleSettings.stellarScales.Planet, StellarSystemData.StarSize * ScaleSettings.stellarScales.Planet);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Star").transform.localScale = new Vector3(5f, 5f, 5f);
        }


        OrbitSize = PlanetData.Orbit * ScaleSettings.dimRet(ScaleSettings.stellarScales.Orbit, 3.5f, ScaleSettings.stellarScales.RationalizeValues) * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? StellarSystemData.ScaleFactor : 1f);

        TextMeshProUGUI ScaleFactorInfo = GameObject.FindGameObjectWithTag("ScaleFactorInfo").GetComponent<TextMeshProUGUI>();
        
        if(PlayerPrefs.GetInt("ScaleFactor") != 0 && StellarSystemData.ScaleFactor != 1f)
        {
            ScaleFactorInfo.text = $"Orbits increased {StellarSystemData.ScaleFactor}x for better view";
        }
        else
        {
            ScaleFactorInfo.text = "";
        }

        CalculatedOrbitSize = OrbitSize;

        if (ObjectType == "planet")
        {

            //Debug.Log($"{name}: {GameObject.FindGameObjectWithTag("Star").transform.localScale.z}");

            StellarAnchor.localPosition = new Vector3(0f, 0f, (GameObject.FindGameObjectWithTag("Star").transform.localScale.z) + OrbitSize);
        }
        else if (ObjectType == "moon")
        {
            if (StellarAnchor != null)
            {
                StellarAnchor.localPosition = new Vector3(0f, 0f, GameObject.Find(ParentStellarObject).transform.localScale.z + OrbitSize);
            }
        }


        DisplayOrbitCircle.localScale = new Vector3(StellarAnchor.localPosition.z / 5f, StellarAnchor.localPosition.z / 5f, StellarAnchor.localPosition.z / 5f);


        ObjectSize = PlanetData.Size * ScaleSettings.stellarScales.Planet;
        //PlanetTrail.startWidth = ObjectSize * .5f;
        CalculatedObjectSize = ObjectSize;

        StellarObject.localScale = new Vector3(ObjectSize, ObjectSize, ObjectSize);

        SetCameraAnchor();
    }

    private void CheckIfObjectIsStuckInside()
    {
        Collider[] colliders;

        colliders = Physics.OverlapSphere(transform.position, 0.0f);
        if(colliders.Length > 0)
        {
            foreach(Collider collided in colliders)
            {
                Debug.Log($"{collided.name} is stuck");
            }
        }

    }

    private void SetCameraAnchor()
    {
        CameraAnchor.localPosition = new Vector3(0f, ObjectSize * .5f, ObjectSize * 2f);
    }

    public void SetStellarObject()
    {
        //ObjectTexture = Resources.Load($"Textures/Planets/{_planetData.Texture}", typeof(Material)) as Material;
        _hasClouds = PlanetData.Clouds;
        _textureMaterial = PlanetData.Texture;
        _material = PlanetData.Material;
        _gaseous = PlanetData.Gaseous;

        if (_hasClouds)
        {
            _clouds.GetComponent<MeshRenderer>().material = PlanetData.CloudsMaterial;
            _clouds.gameObject.SetActive(true);
        }
        else
        {
            Destroy(_clouds.gameObject);
        }

        if (_material)
        {
            Renderer.material = _material;
        }
        
        if(_textureMaterial)
        {
            Renderer.material.mainTexture = _textureMaterial;
            Renderer.material.EnableKeyword("_EMISSION");
            Renderer.material.SetTexture("_EmissionMap", _textureMaterial);
            Renderer.material.SetColor("_EmissionColor", new Vector4(0.05f, 0.05f, 0.05f));
        }

        BodyTiltAngle = PlanetData.BodyTilt;

        StellarObject.rotation = Quaternion.Euler(BodyTiltAngle, 0, 0);
    }

    private void DetectClick()
    {

        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

        // On peut visualiser le rayon dans la sc�ne pour debugger (n'influe en rien sur le jeu)
        Debug.DrawRay(ray.origin, ray.direction * 20f);

        RaycastHit hit;

        //if the mouse is on the sun and and is clicked
        if(Physics.Raycast(ray, out hit) && hit.transform.tag == "Star" && Input.GetMouseButton(0))
        {
            Camera.ChangeTarget(hit.transform);
        }


        //else
        //if the mouse is on a planet or moon...
        else if (Physics.Raycast(ray, out hit) && (hit.transform == _stellarObject))
        {
            IsHovered = true;
            //and is clicked
            if (Input.GetMouseButtonDown(0))
            {

                //if the camera is alreay focused on the planet or moon
                if(Camera.CameraTarget == hit.transform)
                {
                    /*if(IsPointerOverUIObject())
                    {

                    }*/

                    Debug.Log($"Should show description of {hit.transform.name}");

                    //Set the animator boolean to true, which will start the animation to show the details
                    UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[1].GetComponent<RectTransform>().position = Vector3.zero;
                    Animator.SetBool("ShowDetails", !Animator.GetBool("ShowDetails"));
                    
                    //And hide the name
                    //UIName.gameObject.SetActive(false);
                    Animator.SetBool("ShowName", false);
                    Camera.CameraAnchor = CameraAnchor;
                    Camera.CameraAnchorObject = gameObject;
                }

                //else
                else
                {
                    //change the camera focus to the planet
                    Camera.ChangeTarget(hit.transform);
                }


            }

            //and is not clicked and the details are not shown
            else if (!Animator.GetBool("ShowDetails"))
            {
                //Display the name
                //UIName.gameObject.SetActive(true);
                Animator.SetBool("ShowName", true);
                //Position the name on the planet or moon
                //UIName.transform.position = Camera.main.WorldToScreenPoint(_stellarObject.position);
            }
        }

        //If the mouse is not on a planet or moon
        else
        {
            IsHovered = true;

            Animator.SetBool("ShowDetails", false);
            if (PlayerPrefs.GetInt("ShowNames") == 0)
            {
                //UIName.gameObject.SetActive(false);
                Animator.SetBool("ShowName", false);
            }
        }
    }
}