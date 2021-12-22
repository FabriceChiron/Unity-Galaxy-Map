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

    public RectTransform crosshairTexture;

    [SerializeField]
    private float _timeToMaxSpeed = 3f;
    private float _resetTimeToMaxSpeed;

    float speed;
    float rotationZTmp;
    Rigidbody r;
    Quaternion lookRotation;
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

            ChangeAudioClip(speed);

            //Camera follow
            rearCamera.transform.position = Vector3.Lerp(rearCamera.transform.position, rearCameraPosition.position, Time.deltaTime * cameraSmooth);
            rearCamera.transform.rotation = Quaternion.Lerp(rearCamera.transform.rotation, rearCameraPosition.rotation, Time.deltaTime * cameraSmooth);

            //Rotation
            rotationZTmp = Input.GetAxis("Horizontal") * -1f;

            mouseXSmooth = Mathf.Lerp(mouseXSmooth, Input.GetAxis("Mouse X") * rotationSpeed, Time.deltaTime * cameraSmooth);
            mouseYSmooth = Mathf.Lerp(mouseYSmooth, Input.GetAxis("Mouse Y") * rotationSpeed, Time.deltaTime * cameraSmooth);
            Quaternion localRotation = Quaternion.Euler(-mouseYSmooth, mouseXSmooth, rotationZTmp * rotationSpeed);
            lookRotation = lookRotation * localRotation;

            if (Input.GetMouseButton(1) && StarShipSetup.ActiveCamera.name == "Cockpit Camera")
            {
                StarShipSetup.ActiveCamera.transform.rotation = lookRotation;

                if (Input.GetMouseButtonUp(1) && StarShipSetup.ActiveCamera.name == "Cockpit Camera")
                {
                    Debug.Log("go back to normal");
                    StarShipSetup.ActiveCamera.transform.rotation = Quaternion.Lerp(lookRotation, Quaternion.identity, Time.deltaTime * cameraSmooth);
                    //StarShipSetup.ActiveCamera.transform.rotation = Quaternion.identity;
                }

            }

            else
            {
                transform.rotation = lookRotation;
                rotationZ -= mouseXSmooth;
                rotationZ = Mathf.Clamp(rotationZ, -45, 45);
                spaceshipRoot.transform.localEulerAngles = new Vector3(defaultShipRotation.x, defaultShipRotation.y, rotationZ);
                rotationZ = Mathf.Lerp(rotationZ, defaultShipRotation.z, Time.deltaTime * cameraSmooth);
            }



            //Update crosshair texture
            if (crosshairTexture)
            {
                crosshairTexture.position = rearCamera.WorldToScreenPoint(transform.position + transform.forward * 100);
            }
        }


    }

    private void ApplyThrust()
    {



        if (verticalAxis != 0)
        {
            float maxSpeed = goToSpeed(speed, IsBoosting ? accelerationSpeed : normalSpeed, 0.1f);

            _timeToMaxSpeed -= Time.deltaTime;

            speed += verticalAxis * Time.deltaTime * Mathf.Max(speed, _wasBoosting ? 10f : 1f);

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

    private void ChangeAudioClip(float speed)
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
}