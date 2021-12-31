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
    private float _health = 100;

    [SerializeField]
    private Image healthDisplay;

    private Camera _activeCamera;

    private float _delayBetweenHits = 1f;

    private float _nextHitTime;


    public Controller Controller { get => _controller; set => _controller = value; }
    public Camera ActiveCamera { get => _activeCamera; set => _activeCamera = value; }
    public float Health { get => _health; set => _health = value; }

    private void Awake()
    {
        _cameras[0].gameObject.SetActive(true);
        ActiveCamera = _cameras[0];
        _toggleButtons.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        _nextHitTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {

            SwitchCamera();
        }

        UpdateHealthDisplay();

    }

    private void UpdateHealthDisplay()
    {
        healthDisplay.rectTransform.sizeDelta = new Vector2(
            Health,
            healthDisplay.rectTransform.sizeDelta.y
            );
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
            if (collision.transform.GetComponent<StellarObject>() != null)
            {
                HitOnce();
            }

            if(collision.transform.name == "Rock")
            {
                HitOnce();
            }

        }
        
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //}

    public void HitOnce()
    {

        Health -= 10f;
        _nextHitTime = Time.time + _delayBetweenHits;

    }
}
