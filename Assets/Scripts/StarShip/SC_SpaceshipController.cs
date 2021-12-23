using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]

public class SC_SpaceshipController : MonoBehaviour
{
    public float normalSpeed = 25f;
    public float accelerationSpeed = 45f;

    [SerializeField]
    private float _maxSpeed;

    [SerializeField]
    private TextMeshProUGUI _displaySpeed;

    [SerializeField]
    private Transform rearCameraPosition;

    [SerializeField]
    private Camera rearCamera;

    [SerializeField]
    private StarShipSetup _starShipSetup;

    [SerializeField]
    private TrailRenderer[] _jetTrails;

    [SerializeField]
    private ParticleSystem[] _mainThrusters;

    [SerializeField]
    private AudioClip _engineIdle;

    [SerializeField]
    private AudioClip _engineSlow;

    [SerializeField]
    private AudioClip _engineOn;

    private AudioSource _audioSource;

    public Transform spaceshipRoot;
    public float rotationSpeed = 2.0f;
    public float cameraSmooth = 4f;
    private float verticalAxis;
    private bool _isBoosting;
    private bool _wasBoosting;
    private bool _freelook;
    private bool _isCameraAligned = true;

    public RectTransform crosshairTexture;

    [SerializeField]
    private float _timeToMaxSpeed = 3f;
    private float _resetTimeToMaxSpeed;

    float speed;
    float rotationZTmp;
    Rigidbody r;
    Quaternion lookRotation;
    Quaternion cameraLookRotation;
    float rotationZ = 0;
    float mouseXSmooth = 0;
    float mouseYSmooth = 0;
    Vector3 defaultShipRotation;

    float _normalAcceleration = 3f;
    float _boostAcceleration = 5f;

    public StarShipSetup StarShipSetup { get => _starShipSetup; set => _starShipSetup = value; }
    public TrailRenderer[] JetTrails { get => _jetTrails; set => _jetTrails = value; }
    public float TimeToMaxSpeed { get => _timeToMaxSpeed; set => _timeToMaxSpeed = value; }
    public bool IsBoosting { get => _isBoosting; set => _isBoosting = value; }
    public bool Freelook { get => _freelook; set => _freelook = value; }
    public bool IsCameraAligned { get => _isCameraAligned; set => _isCameraAligned = value; }

    private void Awake()
    {
        _resetTimeToMaxSpeed = _timeToMaxSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        lookRotation = transform.rotation;
        defaultShipRotation = spaceshipRoot.localEulerAngles;
        rotationZ = defaultShipRotation.z;

        _audioSource = GetComponent<AudioSource>();

        _audioSource.Play();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        IsBoosting = Input.GetButton("Boost");

        Cursor.lockState = StarShipSetup.Controller.UITest.IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = StarShipSetup.Controller.UITest.IsPaused;

        foreach(TrailRenderer jetTrail in JetTrails)
        {
            jetTrail.time = speed * .5f;
        }

        verticalAxis = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        //Press Right Mouse Button to accelerate
        /*if (Input.GetMouseButton(1))
        {
            speed = Mathf.Lerp(speed, accelerationSpeed, Time.deltaTime * 3);
        }
        else
        {
            speed = Mathf.Lerp(speed, normalSpeed, Time.deltaTime * 10);
        }*/

        if(!StarShipSetup.Controller.UITest.IsPaused)
        {

            ApplyThrust();

            ChangeAudioClip();

            Thrusters();

            //Camera follow
            /*rearCamera.transform.position = Vector3.Lerp(rearCamera.transform.position, rearCameraPosition.position, Time.deltaTime * cameraSmooth);
            rearCamera.transform.rotation = Quaternion.Lerp(rearCamera.transform.rotation, rearCameraPosition.rotation, Time.deltaTime * cameraSmooth);*/

            //Rotation
            if (!Freelook)
            {
                rotationZTmp = Input.GetAxis("Horizontal") * -1f;
            }

            mouseXSmooth = Mathf.Lerp(mouseXSmooth, Input.GetAxis("Mouse X") * rotationSpeed, Time.deltaTime * cameraSmooth);
            mouseYSmooth = Mathf.Lerp(mouseYSmooth, Input.GetAxis("Mouse Y") * rotationSpeed, Time.deltaTime * cameraSmooth);
            Quaternion localRotation = Quaternion.Euler(-mouseYSmooth, mouseXSmooth, rotationZTmp * rotationSpeed);
            lookRotation = lookRotation * localRotation;


            /*if (Input.GetMouseButton(1) && StarShipSetup.ActiveCamera.name == "Cockpit Camera")
            {
                StarShipSetup.ActiveCamera.transform.rotation = lookRotation;

                if (Input.GetMouseButtonUp(1) && StarShipSetup.ActiveCamera.name == "Cockpit Camera")
                {
                    Debug.Log("go back to normal");
                    StarShipSetup.ActiveCamera.transform.rotation = Quaternion.Lerp(lookRotation, transform.rotation, Time.deltaTime * cameraSmooth);
                    //StarShipSetup.ActiveCamera.transform.rotation = Quaternion.identity;
                }

            }*/

            if(StarShipSetup.ActiveCamera.name == "Cockpit Camera")
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("yo");
                    Freelook = true;
                    IsCameraAligned = false;
                }
                if (Input.GetMouseButtonUp(1))
                {
                    Freelook = false;
                    
                }

                //Debug.Log($"Freelook: {Freelook}");

                if(Freelook)
                {
                    cameraLookRotation = lookRotation;
                    StarShipSetup.ActiveCamera.transform.rotation = cameraLookRotation;
                }
                else
                {
                    AlignCamera();
                    if (IsCameraAligned)
                    {
                        RotateShip();
                    }
                }

            }

            else
            {
                RotateShip();
            }



            //Update crosshair texture
            if (crosshairTexture)
            {
                crosshairTexture.position = rearCamera.WorldToScreenPoint(transform.position + transform.forward * 100);
            }
        }


    }

    private void AlignCamera()
    {
        if(!IsCameraAligned)
        {
            Quaternion cameraRotation = StarShipSetup.ActiveCamera.transform.rotation;
            Debug.Log($"Camera: {cameraRotation}\n" +
                $"Starship: {transform.rotation}");
            StarShipSetup.ActiveCamera.transform.rotation = Quaternion.Lerp(cameraRotation, transform.rotation, Time.deltaTime * cameraSmooth);

            if(cameraRotation == transform.rotation)
            {
                IsCameraAligned = true;
                lookRotation = transform.rotation;
            }
        }
    }

    private void RotateShip()
    {
        transform.rotation = lookRotation;
        rotationZ -= mouseXSmooth;
        rotationZ = Mathf.Clamp(rotationZ, -45, 45);
        spaceshipRoot.transform.localEulerAngles = new Vector3(defaultShipRotation.x, defaultShipRotation.y, rotationZ);
        rotationZ = Mathf.Lerp(rotationZ, defaultShipRotation.z, Time.deltaTime * cameraSmooth);
    }

    private void ApplyThrust()
    {



        if (verticalAxis != 0)
        {
            float maxSpeed = goToSpeed(speed, IsBoosting ? accelerationSpeed : normalSpeed, 3f);

            _timeToMaxSpeed -= Time.deltaTime;

            speed += verticalAxis * Time.deltaTime * Mathf.Max(speed, _wasBoosting ? 10f : 5f);

            if (speed >= maxSpeed)
            {
                speed = maxSpeed;
            }

        }
        else
        {
            speed = goToSpeed(speed, 0f, 3f);
        }

        speed = Mathf.Round(speed * 100f) / 100f;

        _displaySpeed.text = $"Speed: {speed}\nRotationZ: {rotationZTmp}";

        //Set moveDirection to the vertical axis (up and down keys) * speed
        Vector3 moveDirection = new Vector3(0, 0, speed);

        //Debug.Log($"{moveDirection} - {transform.TransformDirection(moveDirection)}");

        //Transform the vector3 to local space
        moveDirection = transform.TransformDirection(moveDirection);
        //Set the velocity, so you can move
        r.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

        _wasBoosting = IsBoosting && speed > normalSpeed;
    }

    private float goToSpeed(float thisSpeed, float targetSpeed, float thrust)
    {
        if (Mathf.Abs(thisSpeed) - targetSpeed <= 0.1)
        {
            thisSpeed = targetSpeed;
        }
        else
        {
            thisSpeed = Mathf.Lerp(speed, targetSpeed, Time.deltaTime * thrust);
        }

        return thisSpeed;
    }

    private void ChangeAudioClip()
    {
        AudioClip currentAudioClip = _audioSource.clip;
        AudioClip newAudioClip;


        if (verticalAxis == 0f)
        {
            newAudioClip = _engineIdle;
        }
        else
        {
            if(verticalAxis > 0)
            {
                newAudioClip = IsBoosting ? _engineOn : _engineSlow;
            }

            else
            {
                newAudioClip = _engineSlow;
            }
        }


        if(newAudioClip != currentAudioClip)
        {
            _audioSource.clip = newAudioClip;
            _audioSource.Play();
        }
    }

    private void Thrusters()
    {

        foreach(ParticleSystem _thruster in _mainThrusters)
        {
            var main = _thruster.main;

            Vector3 _thrusterSCale;
            Color _thrusterColor;
            Color _defaultColor = new Color(255, 162, 0, 255);
            if (verticalAxis == 0f)
            {
                _thrusterColor = new Color(255, 162, 0, 255);
                //var main =_thruster.main.startColor = new Color(255, 162, 0, 255);
                _thrusterSCale = new Vector3(1f, 1f, 0f);
            }
            else
            {
                if (verticalAxis > 0)
                {
                    _thrusterColor = IsBoosting ? new Color(0, 138, 255, 255) : new Color(255, 162, 0, 255);
                    //_thrusterSCale = new Vector3(IsBoosting ? 1.5f : 1f, IsBoosting ? 1.5f : 1f, IsBoosting ? 1.5f : 1f);
                    _thrusterSCale = new Vector3(1f, 1f, IsBoosting ? 1.5f : 1f);
                }

                else
                {
                    _thrusterColor = new Color(255, 162, 0, 255);
                    _thrusterSCale = new Vector3(1f, 1f, 0.1f);
                }
            }

            main.startColor = Color.Lerp(_defaultColor, _thrusterColor, Time.deltaTime * 6f);
            _thruster.transform.localScale = Vector3.Lerp(_thruster.transform.localScale, _thrusterSCale, Time.deltaTime * 6f);
        }

    }
}