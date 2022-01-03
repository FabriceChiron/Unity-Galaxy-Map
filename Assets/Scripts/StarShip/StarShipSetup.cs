using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarShipSetup : MonoBehaviour
{
    [SerializeField]
    private Camera[] _cameras;

    [SerializeField]
    private GameObject _toggleButtons;

    [SerializeField]
    private Controller _controller;

    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private float _health = 100;

    [SerializeField]
    private int _shield = 100;

    [SerializeField]
    private int _gasQuantity = 1000;

    [SerializeField]
    private GameObject _healthGauge, _shieldGauge, _gasGauge;

    private TextMesh _healthGaugeText, _shieldGaugeText, _gasGaugeText;
    private Material _healthGaugeCircle, _shieldGaugeCircle;

    [SerializeField]
    private Image healthDisplay;

    [SerializeField]
    private Image energyShieldDisplay;

    [SerializeField]
    private MeshRenderer[] _shields;

    [SerializeField]
    private Canvas _starShipUI;

    private Camera _activeCamera;

    private float _delayBetweenHits = 1f;

    private float _nextHitTime;


    public Controller Controller { get => _controller; set => _controller = value; }
    public Camera ActiveCamera { get => _activeCamera; set => _activeCamera = value; }
    public float Health { get => _health; set => _health = value; }
    public int Shield { get => _shield; set => _shield = value; }
    public int GasQuantity { get => _gasQuantity; set => _gasQuantity = value; }
    public Canvas StarShipUI { get => _starShipUI; set => _starShipUI = value; }

    private void Awake()
    {
        _cameras[0].gameObject.SetActive(true);
        ActiveCamera = _cameras[0];
        _toggleButtons.SetActive(false);

        _healthGaugeText = _healthGauge.transform.GetChild(0).GetComponent<TextMesh>();
        _shieldGaugeText = _shieldGauge.transform.GetChild(0).GetComponent<TextMesh>();
        _gasGaugeText = _gasGauge.GetComponent<TextMesh>();
        
        _healthGaugeCircle = _healthGauge.transform.GetChild(1).GetComponent<MeshRenderer>().material;
        _shieldGaugeCircle = _shieldGauge.transform.GetChild(1).GetComponent<MeshRenderer>().material;

    }

    // Start is called before the first frame update
    void Start()
    {
        _nextHitTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput.SwitchCameraButton)
        {

            SwitchCamera();
        }

        UpdateHealthDisplay();
        UpdateEnergyDisplay();
        UpdateGasDisplay();
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
            Shield,
            energyShieldDisplay.rectTransform.sizeDelta.y
            );

        _shieldGaugeText.text = Shield.ToString();
        _shieldGaugeCircle.color = new Vector4(_shieldGaugeCircle.color.r, _shieldGaugeCircle.color.g, _shieldGaugeCircle.color.b, Shield / 100f);
    }

    private void UpdateGasDisplay()
    {
        _gasGaugeText.text = GasQuantity.ToString();
    }

    private void SwitchCamera()
    {
        foreach(Camera camera in _cameras)
        {
            camera.gameObject.SetActive(!camera.gameObject.activeSelf);
            if (camera.gameObject.activeSelf)
            {

                ActiveCamera = camera;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"StarShipSetup OnCollisionEnter: {collision.transform.name}");

        if (Time.time >= _nextHitTime)
        {
            if (collision.transform.GetComponent<StellarObject>() != null 
                || collision.transform.name == "Rock")
            {
                ToggleShowShield(true);
                HitOnce();
            }
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        ToggleShowShield(false);
    }

    private void ToggleShowShield(bool action)
    {
        switch (action)
        {
            case true:
                if(Shield > 0)
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
                break;
        }
    }

    public void HitOnce()
    {
        if(Shield > 0)
        {
            Shield -= 10;
        }
        else
        {
            Health -= 10f;
        }

        _nextHitTime = Time.time + _delayBetweenHits;

    }
}
