using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShipSetup : MonoBehaviour
{
    [SerializeField]
    private Camera[] _cameras;

    [SerializeField]
    private GameObject _toggleButtons;

    [SerializeField]
    private Controller _controller;

    private Camera _activeCamera;


    public Controller Controller { get => _controller; set => _controller = value; }
    public Camera ActiveCamera { get => _activeCamera; set => _activeCamera = value; }


    private void Awake()
    {
        _cameras[0].gameObject.SetActive(true);
        ActiveCamera = _cameras[0];
        _toggleButtons.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {

            SwitchCamera();
        }

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
}
