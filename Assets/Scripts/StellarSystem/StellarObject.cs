using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class StellarObject : MonoBehaviour
{
    [SerializeField]
    private LoopLists _loopLists;

    [SerializeField]
    private Scales scales;

    [SerializeField]
    private Scales scalesStarship;

    [SerializeField]
    private Mesh _androidMesh;

    private Scales _currentScales;

    [SerializeField]
    private string _parentStellarObject;

    [SerializeField]
    private PlanetData _planetData;

    [SerializeField]
    private string _objectType;

    [SerializeField]
    private Transform _cameraAnchor, _displayOrbitCircle, _clouds;

    [SerializeField]
    private TrailRenderer _objectTrail;

    [SerializeField]
    private TextMeshProUGUI _UIName;

    [SerializeField]
    private PlanetButton _planetButton;

    [SerializeField]
    private Image _UIDetails, _UIDetailsLandscape;

    [SerializeField]
    private Controller _controller;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _widthThreshold;

    [SerializeField]
    private float _generatedObjectSize;

    [SerializeField]
    private SphereCollider[] _sphereCollidersForCamera;

    [SerializeField]
    private SphereCollider _gasCollider;
    private PostProcessVolume _gasPPVolume;

    private CameraFollow _camera;

    private Star _star;

    private float _revolutionTime, _rotationTime, _objectSize, _orbitSize, _bodyTiltAngle, _orbitTiltAngle, _revolutionDegreesPerSecond, rotationDegreesPerSecond, _trailStartTime, _angularSpeed, _travelSpeed;

    private Transform _stellarBody, _stellarAnchor, _orbit, _orbitAnchor;

    private bool _isHovered;

    public PlanetData PlanetData { get => _planetData; set => _planetData = value; }
    public Transform StellarBody { get => _stellarBody; set => _stellarBody = value; }
    public Transform StellarAnchor { get => _stellarAnchor; set => _stellarAnchor = value; }
    public Transform Orbit { get => _orbit; set => _orbit = value; }
    public Transform OrbitAnchor { get => _orbitAnchor; set => _orbitAnchor = value; }
    public Transform CameraAnchor { get => _cameraAnchor; set => _cameraAnchor = value; }
    public Transform DisplayOrbitCircle { get => _displayOrbitCircle; set => _displayOrbitCircle = value; }
    public Transform Clouds { get => _clouds; set => _clouds = value; }
    public string ObjectType { get => _objectType; set => _objectType = value; }
    public LoopLists LoopLists { get => _loopLists; set => _loopLists = value; }
    public float RevolutionTime { get => _revolutionTime; set => _revolutionTime = value; }
    public float RotationTime { get => _rotationTime; set => _rotationTime = value; }
    public float ObjectSize { get => _objectSize; set => _objectSize = value; }
    public float OrbitSize { get => _orbitSize; set => _orbitSize = value; }
    public float BodyTiltAngle { get => _bodyTiltAngle; set => _bodyTiltAngle = value; }
    public float OrbitTiltAngle { get => _orbitTiltAngle; set => _orbitTiltAngle = value; }
    public string ParentStellarObject { get => _parentStellarObject; set => _parentStellarObject = value; }
    public float TrailStartTime { get => _trailStartTime; set => _trailStartTime = value; }
    public TrailRenderer ObjectTrail { get => _objectTrail; set => _objectTrail = value; }
    public TextMeshProUGUI UIName { get => _UIName; set => _UIName = value; }
    public PlanetButton PlanetButton { get => _planetButton; set => _planetButton = value; }
    public Image UIDetails { get => _UIDetails; set => _UIDetails = value; }
    public Image UIDetailsLandscape { get => _UIDetailsLandscape; set => _UIDetailsLandscape = value; }
    public Controller Controller { get => _controller; set => _controller = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public CameraFollow Camera { get => _camera; set => _camera = value; }
    public bool IsHovered { get => _isHovered; set => _isHovered = value; }
    public Star Star { get => _star; set => _star = value; }
    public float GeneratedObjectSize { get => _generatedObjectSize; set => _generatedObjectSize = value; }
    public float AngularSpeed { get => _angularSpeed; set => _angularSpeed = value; }
    public float TravelSpeed { get => _travelSpeed; set => _travelSpeed = value; }
    public Scales CurrentScales { get => _currentScales; set => _currentScales = value; }
    public SphereCollider GasCollider { get => _gasCollider; set => _gasCollider = value; }

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GetComponent<MeshFilter>().mesh = _androidMesh;
        }

        Controller = LoopLists.GetComponent<Controller>();

        CurrentScales = Controller.HasPlayer ? scalesStarship : scales;

        Camera = UnityEngine.Camera.main.GetComponent<CameraFollow>();

        Star = GameObject.FindObjectOfType<Star>();

        StellarBody = transform;
        StellarAnchor = StellarBody.parent;

        Orbit = StellarAnchor.parent;
        OrbitAnchor = Orbit.parent;

        if(_gasCollider != null)
        {
            _gasPPVolume = _gasCollider.GetComponent<PostProcessVolume>();
        }

        //Debug.Log($"OrbitAnchor Name: {OrbitAnchor.name}");

        ObjectTrail.enabled = false;

        FillUIElements();

        SwitchUIDetails();

        CreateStellarObject();

        //SetScales();
        InitElemsByPlayerPrefs();
    }


    // Update is called once per frame
    void Update()
    {
        SwitchUIDetails();

        if (!Controller.IsPaused)
        {
            PlanetRevolution();
            PlanetRotation();
        }

/*        if (Controller.IsVR)
        {
            transform.parent.GetComponentInChildren<Canvas>().worldCamera = GameObject.FindObjectOfType<StarShipSetup>().ActiveCamera;
        }*/

        Controller.StickToObject(PlanetButton.transform, StellarBody, 0f);
        Controller.StickToObject(UIName.transform.parent, StellarBody, 10f);

        CameraAnchor.parent.LookAt(Star.transform.position);

        if (PlayerPrefs.GetInt("ShowNames") == 1)
        {
            //UIName.gameObject.SetActive(true);
            Animator.SetBool("ShowName", true);
        }
        else if (!IsHovered)
        {
            Animator.SetBool("ShowName", false);
        }
    }

    private void InitElemsByPlayerPrefs()
    {
        DisplayOrbitCircle.gameObject.SetActive(PlayerPrefs.GetInt("ShowOrbitCircles") != 0);
        PlanetButton.GetComponent<Image>().enabled = PlayerPrefs.GetInt("HighlightPlanetsPosition") != 0;
        ObjectTrail.enabled = PlayerPrefs.GetInt("ShowTrails") != 0;
    }

    private void FillUIElements()
    {
/*        if (Controller.IsVR)
        {
            transform.parent.GetComponentInChildren<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            transform.parent.GetComponentInChildren<Canvas>().worldCamera = GameObject.FindObjectOfType<StarShipSetup>().ActiveCamera;
        }*/

        name = PlanetData.Name;
        UIName.text = PlanetData.name;
        UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[0].text = PlanetData.name;
        UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[1].text =
            $"<b>Orbit</b>: {PlanetData.Orbit} AU\n" +
            $"<b>Radius</b>: {PlanetData.Size * 6378f}kms ({PlanetData.Size} of Earth's)\n" +
            $"<b>Orbital Period</b>: {PlanetData.YearLength} Earth year(s)\n" +
            $"<b>Rotation Period</b>: {(PlanetData.TidallyLocked ? "Tidally Locked" : $"{((float.IsNaN(PlanetData.DayLength)) ? "Unknown" : $"{PlanetData.DayLength} Earth day(s)")}") }\n\n" +
            $"{PlanetData.Details}";
        UIDetailsLandscape.GetComponentsInChildren<TextMeshProUGUI>(true)[0].text = PlanetData.name;
        UIDetailsLandscape.GetComponentsInChildren<TextMeshProUGUI>(true)[1].text =
            $"<b>Orbit</b>: {PlanetData.Orbit} AU\n" +
            $"<b>Radius</b>: {PlanetData.Size * 6378f}kms ({PlanetData.Size} of Earth's)\n" +
            $"<b>Orbital Period</b>: {PlanetData.YearLength} Earth year(s)\n" +
            $"<b>Rotation Period</b>: {(PlanetData.TidallyLocked ? "Tidally Locked" : $"{((float.IsNaN(PlanetData.DayLength)) ? "Unknown" : $"{PlanetData.DayLength} Earth day(s)")}") }\n\n" +
            $"{PlanetData.Details}";

        //$"<b>Rotation Period</b>: {(PlanetData.TidallyLocked ? "Tidally Locked" : $"{((PlanetData.DayLength == float.NaN && PlanetData.Name != "Earth") ? "Unknown" : $"{PlanetData.DayLength} Earth day(s)")}") }\n\n"
    }

    private void SwitchUIDetails()
    {
        if (Screen.width > Screen.height && Screen.width >= _widthThreshold)
        {
            if (UIDetails)
            {
                UIDetails.gameObject.SetActive(false);
            }
            if (UIDetailsLandscape)
            {
                UIDetailsLandscape.gameObject.SetActive(true);
            }
        }
        else
        {
            if (UIDetailsLandscape)
            {
                UIDetailsLandscape.gameObject.SetActive(false);
            }
            if (UIDetails)
            {
                UIDetails.gameObject.SetActive(true);
            }
        }
    }

    private void PlanetRevolution()
    {
        RevolutionTime = Mathf.Max(PlanetData.YearLength, 0.5f) * CurrentScales.Year;

        Controller.RotateObject(StellarAnchor, RevolutionTime, true);

        Controller.RotateObject(Orbit, RevolutionTime, false);
        
        if(ObjectTrail != null &&  ObjectTrail.enabled)
        {
            ObjectTrail.time = RevolutionTime * .75f;
        }
    }

    public void PlanetRotation()
    {
        bool inverted;
        if (PlanetData.TidallyLocked)
        {
            inverted = Mathf.Abs(PlanetData.YearLength) != PlanetData.YearLength;
            RotationTime = Mathf.Abs(Mathf.Max(PlanetData.YearLength, 0.5f)) * CurrentScales.Year;
        }
        else
        {
            float _dayLength = PlanetData.DayLength;
            if(float.IsNaN(PlanetData.DayLength))
            {
                _dayLength = 1f;
            }
            inverted = Mathf.Abs(_dayLength) != _dayLength;
            RotationTime = Mathf.Abs(_dayLength) * CurrentScales.Day;
        }

        Controller.RotateObject(StellarBody, RotationTime, inverted);

        if (PlanetData.Clouds)
        {
            Controller.RotateObject(Clouds, RotationTime / 0.2f, inverted);
        }

    }


    //Stick UI GameObject to the position of another GameObject, based on position on Screen

    //Creating stellar object (planet, moon)
    private void CreateStellarObject()
    {

        if (Controller.HasPlayer)
        {
            foreach(SphereCollider sphereCollider in _sphereCollidersForCamera)
            {
                sphereCollider.enabled = false;
            }
        }

        SetMass();

        SetMaterial();

        SetClouds();

        ManageRings();

        SetOrbit();

        SetUIElements();

        LoopLists.StellarObjectCount++;
    }

    private void SetMass()
    {
        GetComponent<Rigidbody>().mass = PlanetData.Mass;
        //GetComponent<Rigidbody>().mass = (4f / 3f) * Mathf.PI * Mathf.Pow(PlanetData.Size, 3) * 250f;
    }

    //Apply Material to stellar object
    private void SetMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = PlanetData.Material;
        
        //Make the texture emit light (to be visible even in the darkness of space)
        renderer.material.EnableKeyword("_EMISSION");
        renderer.material.SetTexture("_EmissionMap", renderer.material.mainTexture);
        renderer.material.SetColor("_EmissionColor", new Vector4(0.1f, 0.1f, 0.1f));
    }

    //Apply Cloud Material (if set in PlanetData)
    private void SetClouds()
    {
        if(PlanetData.CloudsMaterial)
        {
            Clouds.gameObject.SetActive(true);
            Clouds.GetComponent<Renderer>().material = PlanetData.CloudsMaterial;
        }
    }

    //If "Rings" is unchecked in PlanetData, destroy "Rings" GameObject
    private void ManageRings()
    {
        if(!PlanetData.Rings)
        {
            Destroy(StellarBody.Find("Rings").gameObject);
        }
    }

    //Set Orbit angles (tilt, and starting position)
    public void SetOrbit()
    {
        //Value of Orbit tilt angle;
        OrbitTiltAngle = PlanetData.OrbitTilt;

        //Set the orbit's plane, using the planet's coords
        Orbit.rotation = Quaternion.Euler(0f, Controller.GetOrbitOrientationStart(PlanetData.Coords), 0f);
        OrbitAnchor.rotation = Quaternion.Euler(OrbitTiltAngle, 0f, 0f);
    }

    //Get Starting point based on cardinal coords, if provided in PlanetData.
    //If not, set random Starting point
    
    private void SetUIElements()
    {
        if(ObjectType == "moon")
        {
            PlanetButton.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 10f);
            UIName.fontSize = 14;
            UIName.fontStyle = (FontStyles)FontStyle.Normal;
            GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    //Set scales according to "scales" scriptable object and PlanetData
    public void SetScales()
    {
        SetOrbitSize();

        DisplayScaleFactorInfos();

        SetStellarAnchor();

        SetObjectSize();

        SetCameraAnchor();
    }

    //Set Stellar Object size
    private void SetObjectSize()
    {
        ObjectSize = PlanetData.Size * CurrentScales.Planet;

        GeneratedObjectSize = ObjectSize;

        if (StellarBody)
        {
            StellarBody.localScale = new Vector3(ObjectSize, ObjectSize, ObjectSize);
            CameraAnchor.GetChild(0).localScale = new Vector3(ObjectSize * 10f, ObjectSize * 10f, ObjectSize * 10f);

            if (ObjectTrail.startWidth > ObjectSize)
            {
                ObjectTrail.startWidth = ObjectSize * .75f;
            }


            GetComponent<Light>().range = ObjectSize * 3f;

            if (PlanetData.Gaseous)
            {
                _gasCollider.gameObject.SetActive(true);
                //_gasPPVolume.blendDistance = ObjectSize * 2f;
                _gasPPVolume.blendDistance = ObjectSize * 0.25f;
                //_gasPPVolume.blendDistance = 100f;
            }
        }

    }

    //Set Orbit size
    private void SetOrbitSize()
    {
        //Calculate the size of the orbit, based on its real orbit size, the scale factor (if set), and if values are rationalized or not
        OrbitSize = PlanetData.Orbit * LoopLists.dimRet(CurrentScales.Orbit, 3.5f, CurrentScales.RationalizeValues) * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? LoopLists.StellarSystemData.ScaleFactor : 1f);

    }

    // Position the Stellar object Anchor point, based on its orbit size and the size of its parent
    // in order to avoid having a planet stuck in its star, or a moon stuck in its planet
    private void SetStellarAnchor()
    {
        if(StellarAnchor)
        {
            switch (ObjectType)
            {
                case "planet":
                    
                    if(ParentStellarObject != "")
                    {
                        StellarAnchor.localPosition = new Vector3(0f, 0f, GameObject.Find(ParentStellarObject).transform.localScale.z + OrbitSize);
                    }

                    else
                    {
                        foreach (Star star in FindObjectsOfType<Star>())
                        {
                            OrbitSize += star.transform.localScale.z * 0.5f;
                        }

                        if(FindObjectsOfType<Star>().Length > 1)
                        {
                            OrbitSize += Vector3.Distance(FindObjectsOfType<Star>()[0].transform.position, FindObjectsOfType<Star>()[1].transform.position);
                        }


                        StellarAnchor.localPosition = new Vector3(0f, 0f, OrbitSize);
                    }
                    

                    break;

                case "moon":
                    StellarAnchor.localPosition = new Vector3(0f, 0f, GameObject.Find(ParentStellarObject).transform.localScale.z + OrbitSize);
                    break;
            }


            DisplayOrbitCircle.localScale = new Vector3(StellarAnchor.localPosition.z / 5f, StellarAnchor.localPosition.z / 5f, StellarAnchor.localPosition.z / 5f);
        }
    }

    private void SetCameraAnchor()
    {
        CameraAnchor.localPosition = new Vector3(0f, ObjectSize * .5f, ObjectSize * (PlanetData.Size < 1f ? 3.5f : 3f));
    }

    //Show the applied scale factor (if set)
    private void DisplayScaleFactorInfos()
    {
        TextMeshProUGUI ScaleFactorInfo = GameObject.FindGameObjectWithTag("ScaleFactorInfo").GetComponent<TextMeshProUGUI>();

        if (PlayerPrefs.GetInt("ScaleFactor") != 0 && LoopLists.StellarSystemData.ScaleFactor != 1f && !_controller.HasPlayer)
        {
            ScaleFactorInfo.text = $"Orbits increased {LoopLists.StellarSystemData.ScaleFactor}x for better view";
        }
        else
        {
            ScaleFactorInfo.text = "";
        }
    }

}
