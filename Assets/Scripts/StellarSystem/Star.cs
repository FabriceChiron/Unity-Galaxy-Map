using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Star : MonoBehaviour
{
    [SerializeField]
    private LoopLists _loopLists;
    
    [SerializeField]
    private Scales scales;

    [SerializeField]
    private Scales scalesStarship;

    [SerializeField]
    private StarData _starData;

    [SerializeField]
    private Transform _cameraAnchor, _displayOrbitCircle;

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
    private StarType starType;


    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _widthThreshold;

    [SerializeField]
    private float _generatedObjectSize;

    private Material _material;

    private CameraFollow _camera;

    private float _revolutionTime, _rotationTime, _objectSize, _orbitSize, _bodyTiltAngle, _orbitTiltAngle, _revolutionDegreesPerSecond, rotationDegreesPerSecond, _trailStartTime;

    private Transform _starBody, _starAnchor, _orbit, _orbitAnchor;

    private bool _isHovered;

    public CameraFollow Camera { get => _camera; set => _camera = value; }
    public Transform StarBody { get => _starBody; set => _starBody = value; }
    public Transform StarAnchor { get => _starAnchor; set => _starAnchor = value; }
    public Transform Orbit { get => _orbit; set => _orbit = value; }
    public Transform OrbitAnchor { get => _orbitAnchor; set => _orbitAnchor = value; }
    public float RevolutionTime { get => _revolutionTime; set => _revolutionTime = value; }
    public float RotationTime { get => _rotationTime; set => _rotationTime = value; }
    public float ObjectSize { get => _objectSize; set => _objectSize = value; }
    public float OrbitSize { get => _orbitSize; set => _orbitSize = value; }
    public float BodyTiltAngle { get => _bodyTiltAngle; set => _bodyTiltAngle = value; }
    public float OrbitTiltAngle { get => _orbitTiltAngle; set => _orbitTiltAngle = value; }
    public float RevolutionDegreesPerSecond { get => _revolutionDegreesPerSecond; set => _revolutionDegreesPerSecond = value; }
    public float RotationDegreesPerSecond { get => rotationDegreesPerSecond; set => rotationDegreesPerSecond = value; }
    public float TrailStartTime { get => _trailStartTime; set => _trailStartTime = value; }
    public Transform CameraAnchor { get => _cameraAnchor; set => _cameraAnchor = value; }
    public Transform DisplayOrbitCircle { get => _displayOrbitCircle; set => _displayOrbitCircle = value; }
    public TrailRenderer ObjectTrail { get => _objectTrail; set => _objectTrail = value; }
    public LoopLists LoopLists { get => _loopLists; set => _loopLists = value; }
    public Controller Controller { get => _controller; set => _controller = value; }
    public TextMeshProUGUI UIName { get => _UIName; set => _UIName = value; }
    public Image UIDetails { get => _UIDetails; set => _UIDetails = value; }
    public Image UIDetailsLandscape { get => _UIDetailsLandscape; set => _UIDetailsLandscape = value; }
    public bool IsHovered { get => _isHovered; set => _isHovered = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public PlanetButton PlanetButton { get => _planetButton; set => _planetButton = value; }
    public StarData StarData { get => _starData; set => _starData = value; }
    public float GeneratedObjectSize { get => _generatedObjectSize; set => _generatedObjectSize = value; }

    // Start is called before the first frame update
    void Start()
    {

        Controller = LoopLists.GetComponent<Controller>();
        Camera = UnityEngine.Camera.main.GetComponent<CameraFollow>();

        starType = StarData.starType;
        
        Debug.Log(starType);

        StarBody = transform;
        StarAnchor = StarBody.parent;

        Orbit = StarAnchor.parent;
        OrbitAnchor = Orbit.parent;

        ObjectTrail.enabled = false;

        FillUIElements();

        SwitchUIDetails();

        CreateStar();

        //SetScales();

        InitElemsByPlayerPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        //DetectClick();
        SwitchUIDetails();

        if (!Controller.IsPaused)
        {
            StarRevolution();
        }



        Controller.StickToObject(PlanetButton.transform, StarBody, 0f);
        Controller.StickToObject(UIName.transform.parent, StarBody, 10f);

        CameraAnchor.LookAt(transform.position);

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
        string starDescription = $"{(StarData.Orbit > 0 ? $"<b>Orbit</b>: {StarData.Orbit} AU\n" : "")}" +
            $"<b>Radius</b>: {StarData.Size * 6378f}kms {(StarData.Name == "Sun" ? "" : $"({StarData.Size / 109} of the Sun)")} \n" +
            $"{(StarData.YearLength > 0 ? $"<b>Orbital Period</b>: {StarData.YearLength} Earth year(s)\n" : "")}\n" +
            $"{StarData.StarDescription}";

        name = StarData.Name;
        UIName.text = StarData.Name;
        UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[0].text = StarData.Name;
        UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[1].text = starDescription;
        UIDetailsLandscape.GetComponentsInChildren<TextMeshProUGUI>(true)[0].text = StarData.Name;
        UIDetailsLandscape.GetComponentsInChildren<TextMeshProUGUI>(true)[1].text = starDescription;

        //$"<b>Rotation Period</b>: {(PlanetData.TidallyLocked ? "Tidally Locked" : $"{((PlanetData.DayLength == float.NaN && PlanetData.Name != "Earth") ? "Unknown" : $"{PlanetData.DayLength} Earth day(s)")}") }\n\n"
    }

    private void SwitchUIDetails()
    {
        if (Screen.width > Screen.height && Screen.width >= _widthThreshold)
        {
            if(UIDetails)
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

    private void StarRevolution()
    {
        RevolutionTime = Mathf.Max(StarData.YearLength, 0.5f) * scales.Year;

        Controller.RotateObject(StarAnchor, RevolutionTime, true);

        Controller.RotateObject(Orbit, RevolutionTime, false);

        if (ObjectTrail != null && ObjectTrail.enabled)
        {
            ObjectTrail.time = RevolutionTime * .75f;
        }
    }

    private void CreateStar()
    {
        SetMaterial();

        SetOrbit();

        LoopLists.StarCount++;
    }


    //Apply material set in StellarSystemData 
    public void SetMaterial()
    {
        _material = StarData.Material;

        Renderer renderer= GetComponent<MeshRenderer>();
        renderer.material = _material;

        if (!renderer.material.name.Contains("sun-texture"))
        {
            GetComponent<Light>().color = Color.Lerp(Color.white, renderer.material.GetColor("_EmissionColor"), 0.1f);
        }
    }

    public void SetOrbit()
    {
        //Value of Orbit tilt angle;
        //OrbitTiltAngle = PlanetData.OrbitTilt;

        //Set the orbit's plane, using the planet's coords
        OrbitAnchor.rotation = Quaternion.Euler(OrbitTiltAngle, Controller.GetOrbitOrientationStart(StarData.Coords), 0f);
    }

    //Set scales according to "scales" scriptable object and StellarSystemData (for star size)
    public void SetScales()
    {
        //if the scales are not rationalized
        if (!scales.RationalizeValues)
        {
            //star scale is calculated with the star size (in Earth size) and the scales applied to planets
            transform.localScale = new Vector3(StarData.Size * scales.Planet, StarData.Size * scales.Planet, StarData.Size * scales.Planet);
        }
        //else, set a default size for the star (multiplied by the scales applied to planets
        else
        {
            float starSize = 0;
            switch (StarData.starType)
            {
                case StarType.BlackHole:
                    starSize = 0.1f;
                    break;
                case StarType.HotBlue:
                    starSize = 100f;
                    break;
                case StarType.NeutronStar:
                    starSize = 0.1f;
                    break;
                case StarType.OrangeDwarf:
                    starSize = 4f;
                    break;
                case StarType.Pulsar:
                    starSize = 1f;
                    break;
                case StarType.RedDwarf:
                    starSize = 2.5f;
                    break;
                case StarType.RedGiant:
                    starSize = 1000f;
                    break;
                case StarType.WhiteDwarf:
                    starSize = 0.1f;
                    break;
                case StarType.SunLike:
                default:
                    starSize = 5f;
                    break;
            }
            transform.localScale = new Vector3(starSize * scales.Planet, starSize * scales.Planet, starSize * scales.Planet);
        }

        SetOrbitSize();

        SetStarAnchor();

        SetObjectSize();

        SetCameraAnchor();
    }

    private void SetObjectSize()
    {
        ObjectSize = transform.localScale.z;

        GeneratedObjectSize = ObjectSize;

        CameraAnchor.GetChild(0).localScale = new Vector3(ObjectSize * 10f, ObjectSize * 10f, ObjectSize * 10f);

        //StarBody.localScale = new Vector3(ObjectSize, ObjectSize, ObjectSize);
    }

    private void SetOrbitSize()
    {
        //Calculate the size of the orbit, based on its real orbit size, the scale factor (if set), and if values are rationalized or not
        OrbitSize = StarData.Orbit * scales.Orbit * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? LoopLists.StellarSystemData.ScaleFactor : 1f);

        if(FindObjectsOfType<Star>().Length > 1)
        {
            foreach(Star star in FindObjectsOfType<Star>())
            {
                OrbitSize += star.transform.localScale.z * 0.5f;
            }
        }
    }

    private void SetStarAnchor()
    {

        if (StarAnchor)
        {
            StarAnchor.localPosition = new Vector3(0f, 0f, OrbitSize);
            DisplayOrbitCircle.localScale = new Vector3(StarAnchor.localPosition.z / 5f, StarAnchor.localPosition.z / 5f, StarAnchor.localPosition.z / 5f);
        }
    }

    private void SetCameraAnchor()
    {
        CameraAnchor.localPosition = new Vector3(0f, ObjectSize * .5f, ObjectSize * 2f);
    }

    private void DetectClick()
    {

        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

        // Visualize Ray on Scene (no impact on Game view)
        Debug.DrawRay(ray.origin, ray.direction * 20f);

        RaycastHit hit;

        //if the mouse is on the sun and and is clicked
        if (Physics.Raycast(ray, out hit) && hit.transform == transform && Input.GetMouseButton(0))
        {
            Camera.ChangeTarget(hit.transform);
        }
    }
}
