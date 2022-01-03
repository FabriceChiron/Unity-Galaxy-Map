using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using TMPro;

[RequireComponent(typeof(Rigidbody))]

public class SC_SpaceshipController : MonoBehaviour
{
    public float normalSpeed = 25f;
    public float accelerationSpeed = 45f;
    public float warpSpeed = 1000f;

    [SerializeField]
    private float _maxSpeed;

    [SerializeField]
    private int _gas = 1000;

    [SerializeField]
    private TextMesh _displaySpeed;

    [SerializeField]
    private Transform rearCameraPosition;

    [SerializeField]
    private Transform _joystick, _throttleControl;

    [SerializeField]
    private Camera rearCamera;

    [SerializeField]
    private StarShipSetup _starShipSetup;

    [SerializeField]
    private ParticleSystem[] _mainThrusters, _backThrusters;

    [SerializeField]
    private AudioClip _engineIdle, _engineSlow, _engineOn, _engineWarp;

    [SerializeField]
    private Animator _animator;

    private AudioSource _audioSource;

    public Transform spaceshipRoot;
    public float rotationSpeed = 2.0f;
    public float cameraSmooth = 4f;
    private float throttleAmount;
    private float verticalAxis;
    private bool _isBoosting, _isWarping;
    private bool _wasBoosting, _wasWarping;
    private bool _freelook;
    private bool _isCameraAligned = true;

    //private Quaternion nullQuaternion = Quaternion.identity;

    public RectTransform crosshairTexture;

    [SerializeField]
    private float _timeToMaxSpeed = 3f;
    //private float _resetTimeToMaxSpeed;

    float speed;
    float rotationZTmp;
    Rigidbody r;
    Quaternion lookRotation;
    Quaternion cameraLookRotation;
    float rotationZ = 0;
    float mouseXSmooth = 0;
    float mouseYSmooth = 0;
    Vector3 defaultShipRotation;

    public StarShipSetup StarShipSetup { get => _starShipSetup; set => _starShipSetup = value; }
    public float TimeToMaxSpeed { get => _timeToMaxSpeed; set => _timeToMaxSpeed = value; }
    public bool IsBoosting { get => _isBoosting; set => _isBoosting = value; }
    public bool IsWarping { get => _isWarping; set => _isWarping = value; }
    public bool Freelook { get => _freelook; set => _freelook = value; }
    public bool IsCameraAligned { get => _isCameraAligned; set => _isCameraAligned = value; }
    public Animator Animator { get => _animator; set => _animator = value; }

    private void Awake()
    {
        //_resetTimeToMaxSpeed = _timeToMaxSpeed;
        Debug.Log($"XR Device: {XRSettings.isDeviceActive}");
        
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
        IsWarping = Input.GetButton("Warp");

        Cursor.lockState = StarShipSetup.Controller.UITest.IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = StarShipSetup.Controller.UITest.IsPaused;

        verticalAxis = Input.GetAxis("Vertical");

        if (XRSettings.isDeviceActive)
        {
            CheckOculusInputs();
        }
    }

    void FixedUpdate()
    {

        Animator.SetFloat("Veering", Input.GetAxis("Horizontal") != 0 ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X"));

        if(!StarShipSetup.Controller.UITest.IsPaused)
        {

            ApplyThrust();

            ChangeAudioClip();

            Thrusters();

            //Rotation
            if (!Freelook)
            {
                rotationZTmp = Input.GetAxis("Horizontal") * -1f;
                MoveJoystick();
                MoveThrottleControl();
            }

            mouseXSmooth = Mathf.Lerp(mouseXSmooth, Input.GetAxis("Mouse X") * rotationSpeed, Time.deltaTime * cameraSmooth);
            mouseYSmooth = Mathf.Lerp(mouseYSmooth, Input.GetAxis("Mouse Y") * rotationSpeed, Time.deltaTime * cameraSmooth);
            Quaternion localRotation = Quaternion.Euler(-mouseYSmooth, mouseXSmooth, rotationZTmp * rotationSpeed);
            lookRotation = lookRotation * localRotation;

            if(StarShipSetup.ActiveCamera.name == "Cockpit Camera")
            {
                if (Input.GetMouseButtonDown(1))
                {
                    //Debug.Log("yo");
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
                Input.GetAxis("Mouse X");
                RotateShip();
            }



            //Update crosshair texture
            if (crosshairTexture)
            {
                crosshairTexture.position = rearCamera.WorldToScreenPoint(transform.position + transform.forward * 100);
            }
        }


    }

    public float GetMovementX()
    {
        return Input.GetAxis("Mouse X");
    }

    private void CheckOculusInputs()
    {
        string message = "";

        /* AXIS*/
        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis1") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis1 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis1")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis2") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis2 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis2")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis3") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis3 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis3")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis4") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis4 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis4")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis5") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis5 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis5")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis6") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis6 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis6")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis7") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis7 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis7")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis8") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis8 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis8")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis9") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis9 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis9")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis10") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis10 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis10")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis11") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis11 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis11")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis12") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis12 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis12")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis13") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis13 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis13")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis14") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis14 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis14")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis15") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis15 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis15")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis16") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis16 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis16")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis17") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis17 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis17")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis18") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis18 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis18")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis19") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis19 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis19")}";
        }

        if (Input.GetAxis("Tilia.Input.UnityInputManager_Axis20") != 0)
        {
            message = $"Tilia.Input.UnityInputManager_Axis20 - {Input.GetAxis("Tilia.Input.UnityInputManager_Axis20")}";
        }

        /*BUTTONS*/
        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis1"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis1 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis1")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis2"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis2 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis2")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis3"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis3 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis3")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis4"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis4 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis4")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis5"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis5 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis5")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis6"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis6 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis6")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis7"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis7 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis7")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis8"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis8 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis8")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis9"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis9 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis9")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis10"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis10 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis10")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis11"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis11 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis11")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis12"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis12 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis12")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis13"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis13 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis13")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis14"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis14 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis14")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis15"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis15 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis15")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis16"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis16 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis16")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis17"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis17 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis17")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis18"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis18 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis18")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis19"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis19 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis19")}";
        }

        if (Input.GetButton("Tilia.Input.UnityInputManager_Axis20"))
        {
            message = $"Tilia.Input.UnityInputManager_Axis20 - {Input.GetButton("Tilia.Input.UnityInputManager_Axis20")}";
        }


        if (message != "")
        {
            //_displaySpeed.text = message;
            Debug.Log(message);
        }

    }

    private void MoveJoystick()
    {
        
        _joystick.localRotation = Quaternion.Euler(Mathf.Clamp(-1f, mouseYSmooth, 1f) * -45f, _joystick.localRotation.y, Mathf.Clamp(-1f, mouseXSmooth, 1f) * -45f);
    }

    private void MoveThrottleControl()
    {
        throttleAmount = Mathf.Lerp(throttleAmount, IsBoosting ? verticalAxis : verticalAxis * 0.5f, Time.deltaTime * cameraSmooth);

        _throttleControl.localRotation = Quaternion.Euler(throttleAmount * 60f, _throttleControl.localRotation.y, _throttleControl.localRotation.z);
    }

    private void AlignCamera()
    {
        if(!IsCameraAligned)
        {
            Quaternion cameraRotation = StarShipSetup.ActiveCamera.transform.rotation;
            //Debug.Log($"Camera: {cameraRotation}\n" +
            //    $"Starship: {transform.rotation}");
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

        Debug.Log($"rotationZ: {rotationZ}");
    }

    private void ApplyThrust()
    {



        if (verticalAxis != 0)
        {
            float maxSpeed = GoToSpeed(
                speed,
                IsWarping ?
                    warpSpeed :
                    IsBoosting ?
                        accelerationSpeed :
                        normalSpeed,
                3f);

            _timeToMaxSpeed -= Time.deltaTime;


            speed += verticalAxis *
                Time.deltaTime *
                Mathf.Max(
                    speed,
                    _wasWarping ?
                        warpSpeed :
                        _wasBoosting ?
                            accelerationSpeed :
                            normalSpeed);

            if(speed <= 50f)
            {
                speed = 50f;
            }
            if (speed >= maxSpeed)
            {
                speed = maxSpeed;
            }

        }
        else
        {
            speed = GoToSpeed(speed, 0f, 3f);
        }

        
        speed = Mathf.Round(speed * 100f) / 100f;

        _displaySpeed.text = Mathf.RoundToInt(speed).ToString();

        //_displaySpeed.text = $"Speed: {speed}\nRotationZ: {rotationZTmp}";

        //Set moveDirection to the vertical axis (up and down keys) * speed
        Vector3 moveDirection = new Vector3(0, 0, speed);

        //Debug.Log($"{moveDirection} - {transform.TransformDirection(moveDirection)}");

        //Transform the vector3 to local space
        moveDirection = transform.TransformDirection(moveDirection);
        //Set the velocity, so you can move
        r.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

        _wasBoosting = IsBoosting && speed > normalSpeed;
        _wasWarping = IsWarping && speed > accelerationSpeed;
    }

    private float GoToSpeed(float thisSpeed, float targetSpeed, float thrust)
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
                newAudioClip = IsWarping ? _engineWarp : IsBoosting ? _engineOn : _engineSlow;
            }

            else if(verticalAxis < 0)
            {
                newAudioClip = _engineOn;
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
        foreach(ParticleSystem _backThruster in _backThrusters)
        {
            var main = _backThruster.main;

            Vector3 _backThrusterScale;

            if(verticalAxis < 0f)
            {
                _backThrusterScale = new Vector3(0.5f, 1f, 1f);
            }
            else
            {
                _backThrusterScale = new Vector3(0.5f, 1f, 0f);
            }
            
            _backThruster.transform.localScale = Vector3.Lerp(_backThruster.transform.localScale, _backThrusterScale, Time.deltaTime * 6f);
        }

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
                    _thrusterColor = (IsBoosting || IsWarping) ? new Color(0, 138, 255, 255) : new Color(255, 162, 0, 255);
                    //_thrusterSCale = new Vector3(IsBoosting ? 1.5f : 1f, IsBoosting ? 1.5f : 1f, IsBoosting ? 1.5f : 1f);
                    _thrusterSCale = new Vector3(
                        1f,
                        1f,
                        IsWarping ?
                            2.0f :
                            IsBoosting ?
                                1.5f :
                                1f);
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