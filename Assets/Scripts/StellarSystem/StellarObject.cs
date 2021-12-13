using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StellarObject : MonoBehaviour
{
    [SerializeField]
    private LoopLists _loopLists;

    [SerializeField]
    private Scales scales;

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

    private CameraFollow _camera;

    private Star _star;

    private float _revolutionTime, _rotationTime, _objectSize, _orbitSize, _bodyTiltAngle, _orbitTiltAngle, _revolutionDegreesPerSecond, rotationDegreesPerSecond, _trailStartTime;

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

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Controller = LoopLists.GetComponent<Controller>();
        Camera = UnityEngine.Camera.main.GetComponent<CameraFollow>();
        Star = GameObject.FindObjectOfType<Star>();

        StellarBody = transform;
        StellarAnchor = StellarBody.parent;

        Orbit = StellarAnchor.parent;
        OrbitAnchor = Orbit.parent;

        //Debug.Log($"OrbitAnchor Name: {OrbitAnchor.name}");

        ObjectTrail.enabled = false;

        FillUIElements();

        SwitchUIDetails();

        CreateStellarObject();

        SetScales();
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

        StickToObject(PlanetButton.transform, StellarBody);
        StickToObject(UIName.transform, StellarBody);

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
        name = PlanetData.Name;
        UIName.text = PlanetData.name;
        UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[0].text = PlanetData.name;
        UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[1].text =
            $"<b>Orbit</b>: {PlanetData.Orbit} AU\n" +
            $"<b>Radius</b>: {PlanetData.Size * 6378f}kms ({PlanetData.Size} of Earth's)\n" +
            $"<b>Orbital Period</b>: {PlanetData.YearLength} Earth year(s)\n" +
            $"<b>Rotation Period</b>: {(PlanetData.TidallyLocked ? "Tidally Locked" : $"{((PlanetData.DayLength == float.NaN) ? "Unknown" : $"{PlanetData.DayLength} Earth day(s)")}") }\n\n" +
            $"{PlanetData.Details}";
        UIDetailsLandscape.GetComponentsInChildren<TextMeshProUGUI>(true)[0].text = PlanetData.name;
        UIDetailsLandscape.GetComponentsInChildren<TextMeshProUGUI>(true)[1].text =
            $"<b>Orbit</b>: {PlanetData.Orbit} AU\n" +
            $"<b>Radius</b>: {PlanetData.Size * 6378f}kms ({PlanetData.Size} of Earth's)\n" +
            $"<b>Orbital Period</b>: {PlanetData.YearLength} Earth year(s)\n" +
            $"<b>Rotation Period</b>: {(PlanetData.TidallyLocked ? "Tidally Locked" : $"{((PlanetData.DayLength == float.NaN) ? "Unknown" : $"{PlanetData.DayLength} Earth day(s)")}") }\n\n" +
            $"{PlanetData.Details}";

        //$"<b>Rotation Period</b>: {(PlanetData.TidallyLocked ? "Tidally Locked" : $"{((PlanetData.DayLength == float.NaN && PlanetData.Name != "Earth") ? "Unknown" : $"{PlanetData.DayLength} Earth day(s)")}") }\n\n"
    }

    private void SwitchUIDetails()
    {
        if (Screen.width > Screen.height && Screen.width >= _widthThreshold)
        {
            UIDetails.gameObject.SetActive(false);
            UIDetailsLandscape.gameObject.SetActive(true);
        }
        else
        {
            UIDetailsLandscape.gameObject.SetActive(false);
            UIDetails.gameObject.SetActive(true);
        }
    }

    private void PlanetRevolution()
    {
        RevolutionTime = Mathf.Max(PlanetData.YearLength, 0.5f) * scales.Year;

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
            RotationTime = Mathf.Abs(Mathf.Max(PlanetData.YearLength, 0.5f)) * scales.Year;
        }
        else
        {
            float _dayLength = PlanetData.DayLength;
            if(PlanetData.DayLength == float.NaN)
            {
                _dayLength = 1f;
            }
            inverted = Mathf.Abs(_dayLength) != _dayLength;
            RotationTime = Mathf.Abs(_dayLength) * scales.Day;
        }

        Controller.RotateObject(StellarBody, RotationTime, inverted);

        if (PlanetData.Clouds)
        {
            Controller.RotateObject(Clouds, RotationTime / 0.2f, inverted);
        }

    }


    //Stick UI GameObject to the position of another GameObject, based on position on Screen
    public void StickToObject(Transform elemTransform, Transform targetTransform)
    {
        elemTransform.position = UnityEngine.Camera.main.WorldToScreenPoint(targetTransform.position);
    }

    //Creating stellar object (planet, moon)
    private void CreateStellarObject()
    {
        SetMaterial();

        SetClouds();

        ManageRings();

        SetOrbit();

        SetUIElements();
    }

    //Apply Material to stellar object
    private void SetMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = PlanetData.Material;
        
        //Make the texture emit light (to be visible even in the darkness of space)
        renderer.material.EnableKeyword("_EMISSION");
        renderer.material.SetTexture("_EmissionMap", renderer.material.mainTexture);
        renderer.material.SetColor("_EmissionColor", new Vector4(0.05f, 0.05f, 0.05f));
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
        OrbitAnchor.rotation = Quaternion.Euler(OrbitTiltAngle, Controller.GetOrbitOrientationStart(PlanetData.Coords), 0f);
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
        ObjectSize = PlanetData.Size * scales.Planet;

        StellarBody.localScale = new Vector3(ObjectSize, ObjectSize, ObjectSize);
    }

    //Set Orbit size
    private void SetOrbitSize()
    {
        //Calculate the size of the orbit, based on its real orbit size, the scale factor (if set), and if values are rationalized or not
        OrbitSize = PlanetData.Orbit * LoopLists.dimRet(scales.Orbit, 3.5f, scales.RationalizeValues) * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? LoopLists.StellarSystemData.ScaleFactor : 1f);
    }

    // Position the Stellar object Anchor point, based on its orbit size and the size of its parent
    // in order to avoid having a planet stuck in its star, or a moon stuck in its planet
    private void SetStellarAnchor()
    {
        switch (ObjectType)
        {
            case "planet":
                StellarAnchor.localPosition = new Vector3(0f, 0f, (LoopLists.NewStar.transform.localScale.z * 0.5f) + OrbitSize);
                break;

            case "moon":
                StellarAnchor.localPosition = new Vector3(0f, 0f, GameObject.Find(ParentStellarObject).transform.localScale.z + OrbitSize);
                break;
        }

        if(StellarAnchor)
        {
            DisplayOrbitCircle.localScale = new Vector3(StellarAnchor.localPosition.z / 5f, StellarAnchor.localPosition.z / 5f, StellarAnchor.localPosition.z / 5f);
        }
    }

    private void SetCameraAnchor()
    {
        CameraAnchor.localPosition = new Vector3(0f, ObjectSize * .5f, ObjectSize * 2f);
    }

    //Show the applied scale factor (if set)
    private void DisplayScaleFactorInfos()
    {
        TextMeshProUGUI ScaleFactorInfo = GameObject.FindGameObjectWithTag("ScaleFactorInfo").GetComponent<TextMeshProUGUI>();

        if (PlayerPrefs.GetInt("ScaleFactor") != 0 && LoopLists.StellarSystemData.ScaleFactor != 1f)
        {
            ScaleFactorInfo.text = $"Orbits increased {LoopLists.StellarSystemData.ScaleFactor}x for better view";
        }
        else
        {
            ScaleFactorInfo.text = "";
        }
    }

    private void DetectClick()
    {

        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

        // Visualize Ray on Scene (no impact on Game view)
        Debug.DrawRay(ray.origin, ray.direction * 20f);

        RaycastHit hit;

        //if the mouse is on the sun and and is clicked
        if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Star" && Input.GetMouseButton(0))
        {
            Camera.ChangeTarget(hit.transform);
        }


        //else
        //if the mouse is on a planet or moon...
        else if (Physics.Raycast(ray, out hit) && (hit.transform == StellarBody))
        {
            IsHovered = true;
            //and is clicked
            if (Input.GetMouseButtonDown(0))
            {

                /*//if the camera is alreay focused on the planet or moon
                if (Camera.CameraTarget == hit.transform)
                {
                    Debug.Log($"Should show description of {hit.transform.name}");

                    //Set the animator boolean to true, which will start the animation to show the details
                    //UIDetails.GetComponentsInChildren<TextMeshProUGUI>(true)[1].GetComponent<RectTransform>().position = Vector3.zero;
                    Animator.SetBool("ShowDetails", !Animator.GetBool("ShowDetails"));

                    //And hide the name
                    Animator.SetBool("ShowName", false);
                    Camera.CameraAnchor = CameraAnchor;
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
            else //if (!Animator.GetBool("ShowDetails"))
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

            /*Animator.SetBool("ShowDetails", false);
            if (PlayerPrefs.GetInt("ShowNames") == 0)
            {
                //UIName.gameObject.SetActive(false);
                Animator.SetBool("ShowName", false);
            }*/
            Animator.SetBool("ShowName", false);
        }
    }
}
