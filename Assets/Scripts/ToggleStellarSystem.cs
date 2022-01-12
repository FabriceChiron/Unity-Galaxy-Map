using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleStellarSystem : MonoBehaviour
{
    [SerializeField]
    private Scales scales;

    [SerializeField]
    private ToggleSetting _toggleValues;

    [SerializeField]
    private TextMeshProUGUI _infos;

    [SerializeField]
    private float _initDeployDuration = 1f;
    private float _currentDeployDuration;
    private float _timeToScale;
    private float _stellarSystemOriginalScale;
    private float _stellarSystemCurrentScale;
    private float _stellarSystemTargetScale;

    private float velocity;

    private Animator _animator;
    private bool _isAnimating;
    private bool _isScaleChanging;

    private Controller _controller;
    private Detector _detector;

    public Animator Animator { get => _animator; set => _animator = value; }
    public bool IsAnimating { get => _isAnimating; set => _isAnimating = value; }
    public float StellarSystemOriginalScale { get => _stellarSystemOriginalScale; set => _stellarSystemOriginalScale = value; }
    public float StellarSystemCurrentScale { get => _stellarSystemCurrentScale; set => _stellarSystemCurrentScale = value; }
    public float StellarSystemTargetScale { get => _stellarSystemTargetScale; set => _stellarSystemTargetScale = value; }
    public bool IsScaleChanging { get => _isScaleChanging; set => _isScaleChanging = value; }
    public Detector Detector { get => _detector; set => _detector = value; }

    private Rigidbody _playerRB;
    private SC_SpaceshipController _spaceshipController;
    private StarShipSetup _starshipSetup;





    private void Awake()
    {
        _timeToScale = 0;
        _toggleValues = GameObject.FindObjectOfType<ToggleSetting>();

        _infos = GameObject.Find("DeviceInfo").GetComponent<TextMeshProUGUI>();

        _controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        Animator = GetComponent<Animator>();

        _currentDeployDuration = _initDeployDuration;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_controller.HasPlayer)
        {
            _spaceshipController = _controller.Player.GetComponent<SC_SpaceshipController>();
            _starshipSetup = _controller.Player.GetComponent<StarShipSetup>();
        }

        GetTargetScale();

        StellarSystemCurrentScale = 0f;

        _toggleValues.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            IsScaleChanging = true;
            GetTargetScale();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (_starshipSetup != null) 
        {
            _starshipSetup.IsInvincible = IsScaleChanging;
        } 

        if (Animator.GetBool("IsAnimating"))
        {
            _controller.ClearTrails();
        }

        _currentDeployDuration = StellarSystemTargetScale > 0f ? _initDeployDuration : _initDeployDuration * .5f;
        
        if (IsScaleChanging)
        {
            _controller.ClearTrails();
            if(StellarSystemTargetScale > 0)
            {
                //_controller.TriggerSetScales("ToggleStellarSystem");
                //Camera.main.GetComponent<CameraFollow>().ResetCameraTarget(false);
            }

            StellarSystemCurrentScale = Mathf.SmoothDamp(StellarSystemCurrentScale, StellarSystemTargetScale, ref velocity, _currentDeployDuration);

            Animator.SetFloat("Scale", StellarSystemCurrentScale);

            if(Mathf.Abs(StellarSystemTargetScale - StellarSystemCurrentScale) <= 0.01f) {
                StellarSystemCurrentScale = StellarSystemTargetScale;
                Animator.SetFloat("Scale", StellarSystemTargetScale);
                IsScaleChanging = false;

                if(StellarSystemTargetScale == 0 && !_controller.HasPlayer)
                {
                    _controller.MainCamera.GetComponent<CameraFollow>().InitCamera();
                }
            }

        }

        
    }

    public void toggleIsAnimating()
    {
        //Animator.SetBool("IsAnimating", !Animator.GetBool("IsAnimating"));
    }

    public void ActivateUIDetails()
    {
        //Camera.main.GetComponent<CameraFollow>().ResetCameraTarget();

        foreach(StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
        {
            stellarObject.UIDetails.gameObject.SetActive(true);
        }
    }

    public void DeployStellarSystem()
    {
        //Animator.SetBool("IsDeployed", true);

        IsScaleChanging = true;

        GetTargetScale();

        StartCoroutine(AudioHelper.FadeOut(_controller.TravelSound, _controller.FadeTime));
        
        _controller.TriggerSetScales("ToggleStellarSystem");

        if (!_controller.HasPlayer)
        {
            _controller.MainCamera.GetComponent<CameraFollow>().ResetCameraTarget(false);
        }


    }

    public void GetTargetScale()
    {
        //StellarSystemTargetScale = scales.RationalizeValues ? 1f : 0.1f;
        StellarSystemTargetScale = 1f;
        StellarSystemOriginalScale = StellarSystemTargetScale;
        _timeToScale = _initDeployDuration;
    }

    public void FoldStellarSystem()
    {
        foreach(GameObject UIDetails in GameObject.FindGameObjectsWithTag("UI - Details"))
        {
            Destroy(UIDetails);
        }

        foreach(StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
        {
            stellarObject.Animator.SetBool("ShowName", false);
        }

        if (!_controller.HasPlayer)
        {
            Camera.main.GetComponent<CameraFollow>().ResetCameraTarget(false);
        }


        IsScaleChanging = true;
        StellarSystemTargetScale = 0;
        //Animator.SetBool("IsDeployed", false);

        StartCoroutine(AudioHelper.FadeIn(_controller.TravelSound, _controller.FadeTime));
    }
}
