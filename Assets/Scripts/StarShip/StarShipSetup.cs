using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR.LegacyInputHelpers;

public class StarShipSetup : MonoBehaviour
{
    [SerializeField]
    private Camera[] _cameras;

    [SerializeField]
    private GameObject[] _gameObjectsToDeactivate;

    [SerializeField]
    private Controller _controller;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private GameObject _VRControllers;

    [SerializeField]
    private ParticleSystem _explosion;

    [SerializeField]
    private float _health = 100, _timeBeforeRecharge = 5f;

    private float _timeWithoutDamage;

    [SerializeField]
    private float _shield = 100f;
    private float _maxShield;

    [SerializeField]
    private float _hydrogen = 1000;

    [SerializeField]
    private GameObject _healthGauge, _shieldGauge, _gasGauge, _starShipModel;

    private TextMesh _healthGaugeText, _shieldGaugeText, _gasGaugeText;
    private Material _healthGaugeCircle, _shieldGaugeCircle;

    [SerializeField]
    private Image healthDisplay;

    [SerializeField]
    private Image energyShieldDisplay;

    [SerializeField]
    private MeshRenderer[] _shields;
    
    //private float _shieldFadeTime, _shieldOpacity = 0.48235f, _shieldEmissionOpacity = 1f;

    //private float _baseAlphaStart, _baseAphaEnd, _emissionAlphaStart, _emissionAphaEnd;

    //private float _fadeSpeed;

    [SerializeField]
    private Canvas _menu, _gameOverMenu;

    [SerializeField]
    private GameObject _gameOverMenuVROverlay;

    [SerializeField]
    private Image[] _menuImages;

    [SerializeField]
    private MeshRenderer _crosshair;

    [SerializeField]
    private GameObject _stellarSystemSelection;

    [SerializeField]
    private Canvas _starShipUI;

    [SerializeField]
    private AudioClip _lightSpeedJump, _blasterHitShield, _blasterHitShip;

    private Camera _activeCamera;

    private float _delayBetweenHits = 1f;

    private float _nextHitTime, _timeToTurnShieldOff;

    private bool _isShieldHit, _isDead, _isInvincible;

    private AudioSource _audioSource;

    public Controller Controller { get => _controller; set => _controller = value; }
    public Camera ActiveCamera { get => _activeCamera; set => _activeCamera = value; }
    public float Health { get => _health; set => _health = value; }
    public float Shield { get => _shield; set => _shield = value; }
    public float Hydrogen { get => _hydrogen; set => _hydrogen = value; }
    public Canvas StarShipUI { get => _starShipUI; set => _starShipUI = value; }
    public AudioClip LightSpeedJump { get => _lightSpeedJump; set => _lightSpeedJump = value; }
    public ParticleSystem Explosion { get => _explosion; set => _explosion = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }
    public bool IsInvincible { get => _isInvincible; set => _isInvincible = value; }
    public Canvas GameOverMenu { get => _gameOverMenu; set => _gameOverMenu = value; }

    private void Awake()
    {
        _cameras[0].gameObject.SetActive(true);
        ActiveCamera = _cameras[0];

        SetVRControllersParent(ActiveCamera);
        
        foreach(GameObject gameObjectToDeactivate in _gameObjectsToDeactivate)
        {
            gameObjectToDeactivate.SetActive(false);
        }

        _healthGaugeText = _healthGauge.transform.GetChild(0).GetComponent<TextMesh>();
        _shieldGaugeText = _shieldGauge.transform.GetChild(0).GetComponent<TextMesh>();
        _gasGaugeText = _gasGauge.GetComponent<TextMesh>();
        
        _healthGaugeCircle = _healthGauge.transform.GetChild(1).GetComponent<MeshRenderer>().material;
        _shieldGaugeCircle = _shieldGauge.transform.GetChild(1).GetComponent<MeshRenderer>().material;

        _maxShield = Shield;

/*        _fadeSpeed = 0f;

        _baseAlphaStart = _shieldOpacity;
        _baseAphaEnd = 0f;
        _emissionAlphaStart = _shieldEmissionOpacity;
        _emissionAphaEnd = 0f;
        _fadeSpeed = 5f;*/

        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _nextHitTime = Time.time;

        //FadeShieldOpacity(2f);

        //AttachMenuToStarShip();
        AttachStellarSystemSelectionToStarShip();


        LinkMenuToCamera();
    }


    // Update is called once per frame
    void Update()
    {
        _menu.enabled = _controller.IsPaused;
        _crosshair.enabled = !_controller.IsPaused;

        if (_playerInput.SwitchCameraButton)
        {
            SwitchCamera();
        }

        if (!IsDead)
        {
            UpdateHealthDisplay();
            UpdateEnergyDisplay();


            UpdateGasDisplay();

            _timeWithoutDamage += Time.deltaTime;


            TurnShieldOff();
            RechargeShield();
        }
    }

    private void AttachMenuToStarShip()
    {
        _menu.renderMode = RenderMode.WorldSpace;

/*        _menu.transform.parent = transform;*/
        _menu.transform.SetParent(transform, false);
        _menu.transform.localPosition = new Vector3(0f, 0f, 10f);
        _menu.transform.localRotation = Quaternion.identity;
        _menu.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        _menu.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 300);
    }

    private void AttachStellarSystemSelectionToStarShip()
    {
        _stellarSystemSelection.transform.SetParent(_menu.transform, false);
        RectTransform rt = _stellarSystemSelection.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 0.5f);
        _stellarSystemSelection.transform.localPosition = Vector3.zero;

        /*
        foreach (Image img in _menuImages)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        }
        */
    }

    private void LinkMenuToCamera()
    {
        _menu.transform.SetParent(ActiveCamera.transform.parent);
        _menu.transform.localPosition = new Vector3(0f,0f,0.6f);
        _menu.worldCamera = ActiveCamera;

        GameOverMenu.transform.SetParent(ActiveCamera.transform.parent);
        GameOverMenu.transform.localPosition = new Vector3(0f,0f,0.6f);
        GameOverMenu.worldCamera = ActiveCamera;

        _gameOverMenuVROverlay.transform.SetParent(ActiveCamera.transform.parent);
        _gameOverMenuVROverlay.transform.localPosition = new Vector3(0f, 0f, 0.6f);

    }

    private void UpdateHealthDisplay()
    {
        healthDisplay.rectTransform.sizeDelta = new Vector2(
            Health,
            healthDisplay.rectTransform.sizeDelta.y
            );

        _healthGaugeText.text = Health.ToString();
        _healthGaugeCircle.color = new Vector4(_healthGaugeCircle.color.r, _healthGaugeCircle.color.g, _healthGaugeCircle.color.b, Health / 100f);

    }

    private void UpdateEnergyDisplay()
    {
        energyShieldDisplay.rectTransform.sizeDelta = new Vector2(
            Mathf.RoundToInt(Shield),
            energyShieldDisplay.rectTransform.sizeDelta.y
            );


        _shieldGaugeText.text = Mathf.RoundToInt(Shield).ToString();
        _shieldGaugeCircle.color = new Vector4(_shieldGaugeCircle.color.r, _shieldGaugeCircle.color.g, _shieldGaugeCircle.color.b, Mathf.RoundToInt(Shield) / 100f);

    }

    private void RechargeShield()
    {

        if (_timeWithoutDamage >= _timeBeforeRecharge && _shield <= _maxShield)
        {
            Shield += Time.deltaTime * 3f * _timeWithoutDamage;
            if(Shield > _maxShield)
            {
                Shield = _maxShield;
            }
        }
    }

    private void UpdateGasDisplay()
    {
        _gasGaugeText.text = Mathf.RoundToInt(Hydrogen).ToString();
    }

    private void SwitchCamera()
    {
        foreach (Camera camera in _cameras)
        {
            camera.gameObject.SetActive(!camera.gameObject.activeSelf);
            if (camera.gameObject.activeSelf)
            {
                ActiveCamera = camera;
            }
        }

        SetVRControllersParent(ActiveCamera);

        LinkMenuToCamera();
    }

    private void SetVRControllersParent(Camera activeCamera)
    {
        if (XRSettings.isDeviceActive)
        {
            _VRControllers.transform.parent = activeCamera.transform.parent;
            _VRControllers.transform.localPosition = activeCamera.transform.localPosition;
            _VRControllers.transform.localRotation = Quaternion.identity;
            _VRControllers.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void TurnShieldOff()
    {
        if (_timeToTurnShieldOff >= 0f && _isShieldHit)
        {
            _timeToTurnShieldOff -= Time.deltaTime;
        }

        if (_timeToTurnShieldOff <= 0f)
        {
            ToggleShowShield(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        BlasterShot blasterShot = other.GetComponent<BlasterShot>();

        if(blasterShot != null && blasterShot.Origin != transform)
        {
            HitOnce(blasterShot);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (Time.time >= _nextHitTime)
        {
            if (collision.transform.name == "Rock")
            {
                //Debug.Log($"StarShipSetup OnCollisionEnter: {collision.transform.name}");
                if(Shield > 0f)
                {
                    foreach (MeshRenderer shield in _shields)
                    {
                        shield.enabled = true;
                    }
                }
                //_fadeSpeed = 0.5f;
                //ToggleShowShield(true);
                HitOnce(null);
            }

            if(collision.transform.GetComponent<StellarObject>() != null && !IsInvincible)
            {
                Die();
            }
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log($"StarShipSetup OnCollisionExit: {collision.transform.name}");
        ToggleShowShield(false);
    }

/*    private void FadeShieldOpacity()
    {
        Debug.Log($"_baseAlphaStart: {_baseAlphaStart}, _baseAphaEnd: {_baseAphaEnd}");

        foreach (MeshRenderer shield in _shields)
        {
            *//*Debug.Log($"{shield.material.GetColor("_Color")}\n{shield.material.GetColor("_EmissionColor")}");
            Debug.Log($"{new Color(0.54533f, 0.90027f, 0.95283f, _shieldOpacity)}");*//*
            shield.enabled = true;

            Color baseColor = shield.material.GetColor("_Color");
            Color emissionColor = shield.material.GetColor("_EmissionColor");

            Debug.Log($"Before: {shield.material.GetColor("_Color")}");

            baseColor.a = Mathf.Lerp(_baseAlphaStart, _baseAphaEnd, Time.deltaTime);
            emissionColor.a = Mathf.Lerp(_emissionAlphaStart, _emissionAphaEnd, Time.deltaTime);
            Debug.Log($"After: {shield.material.GetColor("_Color")}");

            //Color newBaseColor = new Color(baseColor.r, baseColor.g, baseColor.g, baseColor.a);
            //Color newEmissionColor = new Color(emissionColor.r, emissionColor.g, emissionColor.g, emissionColor.a);

            shield.material.SetColor("_Color", new Color(baseColor.r, baseColor.g, baseColor.g, baseColor.a));
            shield.material.SetColor("_EmissionColor", new Color(emissionColor.r, emissionColor.g, emissionColor.b, emissionColor.a));

        }
    }*/

    public void ToggleShowShield(bool action)
    {
        //_animator.SetBool("IsShieldOn", action);


        switch (action)
        {
            case true:
                //Debug.Log($"ToggleShowShield: {action}");
                if (Shield > 0)
                {
                    foreach (MeshRenderer shield in _shields)
                    {
                        shield.enabled = true;
                    }
                }
                break;

            case false:
                foreach (MeshRenderer shield in _shields)
                {
                    shield.enabled = false;
                }
                _isShieldHit = false;
                break;
        }
    }

    public void HitOnce(BlasterShot blasterShot)
    {
        if (!IsInvincible)
        {

        }
        _timeWithoutDamage = 0f;

        ToggleShowShield(true);

        if (Shield > 0)
        {
            Shield -= 10;
            if(blasterShot != null)
            {
                _audioSource.PlayOneShot(_blasterHitShield);
                
                _isShieldHit = true;
                _timeToTurnShieldOff = 0.5f;
            }
        }
        else
        {
            Shield = 0;
            Health -= 10f;
            foreach (MeshRenderer shield in _shields)
            {
                shield.enabled = false;
            }

            if (blasterShot != null)
            {
                _audioSource.PlayOneShot(_blasterHitShip);
            }

            if(Health <= 0f)
            {
                Die();
            }
        }

        _nextHitTime = Time.time + _delayBetweenHits;

    }

    public void Die()
    {
        if (!IsDead)
        {
            Health = 0;
            Explosion.Play();
            AudioSource audioSourceExplosion = Explosion.GetComponent<AudioSource>();
            audioSourceExplosion.PlayOneShot(audioSourceExplosion.clip);
            Destroy(_starShipModel);
            Destroy(_shields[0].gameObject);
            IsDead = true;

            GameOverMenu.gameObject.SetActive(true);
            _gameOverMenuVROverlay.gameObject.SetActive(true);
            _VRControllers.SetActive(true);
        }
    }
}
