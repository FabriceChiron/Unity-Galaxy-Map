using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraFollow : MonoBehaviour
{
    private Transform _transform;
    private Transform _cameraTarget;

    private UITest _UITest;

    [SerializeField]
    private float _sensitivity = 3f;

    [SerializeField] 
    private string _startingPoint;

    [SerializeField]
    private Controller _controller;

    private float _scrollWheelChange;

    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private TMP_Dropdown PlanetListDropdown;

    private bool _mouseOnUI;

    float mouseHorizontal;
    float mouseVertical;

    private Transform _star, _cameraAnchor;
    private GameObject _cameraAnchorObject;

    private Vector3 _initPosition;
    private Quaternion _initRotation;

    public Vector2 startPos;
    public Vector2 direction;

    public string StartingPoint { get => _startingPoint; set => _startingPoint = value; }
    public float Sensitivity { get => _sensitivity; set => _sensitivity = value; }

    public Transform Star { get => _star; set => _star = value; }
    public Transform CameraTarget { get => _cameraTarget; set => _cameraTarget = value; }
    public UITest UITest { get => _UITest; set => _UITest = value; }
    public bool MouseOnUI { get => _mouseOnUI; set => _mouseOnUI = value; }
    public Transform CameraAnchor { get => _cameraAnchor; set => _cameraAnchor = value; }
    public GameObject CameraAnchorObject { get => _cameraAnchorObject; set => _cameraAnchorObject = value; }
    public Vector3 InitPosition { get => _initPosition; set => _initPosition = value; }
    public Quaternion InitRotation { get => _initRotation; set => _initRotation = value; }
    public Controller Controller { get => _controller; set => _controller = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _transform = GetComponent<Transform>();

        InitPosition = _transform.position;
        InitRotation = _transform.rotation;
        
        if(GameObject.FindGameObjectWithTag("Star") != null)
        {
            ResetCameraTarget();
        }

        UITest = Controller.UITest;
    }

    public void InitCamera()
    {
        transform.parent = null;
        transform.position = InitPosition;
        transform.rotation = InitRotation;
    }

    public void ResetCameraTarget()
    {
        InitCamera();

        Star = Controller.LoopLists.NewStar.transform;
        
        CameraTarget = Star;
        ChangeTarget(Star);
    }

    // Update is called once per frame
    void Update()
    {
        if(UITest != null)
        {
            MouseOnUI = UITest.IsPointerOverUIElement();
        }
        if (!MouseOnUI)
        {
            ZoomCamera();
        }

        if(CameraTarget != Star && CameraTarget != null)
        {
            _transform.parent = CameraTarget.parent;
        }
        else
        {
            _transform.parent = null;
        }

        if (CameraTarget)
        {
            Vector3 lookDirection = CameraTarget.position - _transform.position;
            lookDirection.Normalize();

            _transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(lookDirection), _speed * Time.deltaTime);
        }

    }

    private void FixedUpdate()
    {
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    startPos = touch.position;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    direction = touch.position - startPos;
                    //TouchToWorldPosition(touch);
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    break;
            }
        }

        if (Input.GetMouseButton(1))
        {
            RotateAroundObject();
        }

        if (Input.GetMouseButton(2))
        {
            CameraTarget = null;
            Vector3 NewPosition = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
            Vector3 pos = transform.localPosition;
            if (NewPosition.x > 0.0f)
            {
                pos += transform.right;
            }
            else if (NewPosition.x < 0.0f)
            {
                pos -= transform.right;
            }
            if (NewPosition.z > 0.0f)
            {
                pos += transform.forward;
            }
            if (NewPosition.z < 0.0f)
            {
                pos -= transform.forward;
            }
            pos.y = transform.localPosition.y;
            transform.localPosition = pos;
            //transform.localPosition = Vector3.Lerp(_transform.position, pos, 50f * Time.deltaTime);

            //transform.Translate(transform.up * mouseVertical * Sensitivity);
            //transform.Translate(transform.right * mouseHorizontal * Sensitivity);
        }

        //Debug.Log(CameraAnchor);
        if(CameraAnchor != null)
        {
            if(CameraAnchorObject.GetComponent<Planet>() != null)
            {
                FocusOnTarget("Planet");
            }
            else if(CameraAnchorObject.GetComponent<Galaxy>() != null)
            {
                FocusOnTarget("Galaxy");
            }
            
        }
    }

    public void RotateAroundObject()
    {
        transform.RotateAround(CameraTarget == null ? transform.position : CameraTarget.transform.position, Vector3.up, mouseHorizontal * Sensitivity); //use transform.Rotate(transform.up * mouseHorizontal * Sensitivity);
        transform.RotateAround(CameraTarget == null ? transform.position : CameraTarget.transform.position, -Vector3.right, mouseVertical * Sensitivity);
    }

    public void ChangeTarget(Transform newCameraTarget)
    {
        CameraTarget = newCameraTarget;

        ChangeSelectionInDropdown(newCameraTarget.name);
    }

    public void ChangeTarget(string PlanetName)
    {
        string StrippedPlanetName = PlanetName.Replace("    ", "").Replace("<b>", "").Replace("</b>", "");

        CameraTarget = GameObject.Find($"{StrippedPlanetName}").transform;

        ChangeSelectionInDropdown(StrippedPlanetName);
    }

    private void ChangeSelectionInDropdown(string newCameraTarget)
    {
        for (int i = 0; i < PlanetListDropdown.options.Count; i++)
        {
            string StrippedCameraTarget = PlanetListDropdown.options[i].text.Replace("    ", "").Replace("<b>", "").Replace("</b>", "");

            if (newCameraTarget == StrippedCameraTarget)
            {
                //Debug.Log($"Setting dropdown to {PlanetListDropdown.options[i].text.Replace("     ", "")}");
                PlanetListDropdown.value = i;
            }
        }
    }

    public void FocusOnTarget(string componentType)
    {
        //Debug.Log("Lerping");
        //Vector3 newPos = Vector3.Lerp(_transform.position, _cameraAnchor.position, 5f * Time.deltaTime);
        Vector3 newPos = Vector3.Lerp(_transform.position, CameraAnchor.position, 5f * Time.deltaTime);
        // On applique la nouvelle position
        _transform.position = newPos;

        float targetThreshold = 0.1f;

        switch (componentType)
        {
            case "Planet":
                targetThreshold = CameraAnchorObject.GetComponent<Planet>().ObjectSize * 0.5f;
                break;
            
            case "Galaxy":
                targetThreshold = 0.5f;
                break;

            case "Star":
                targetThreshold = 0.5f;
                break;
        }
        
        if (Vector3.Distance(_transform.position, CameraAnchor.position) <= targetThreshold)
        {
            CameraAnchor = null;
            CameraAnchorObject = null;
        }
        

    }

    private void ZoomCamera()
    {
        _scrollWheelChange = Input.GetAxis("Mouse ScrollWheel");

        if(_scrollWheelChange != 0f)
        {
            //_transform.position += _transform.forward * _scrollWheelChange;
            if (CameraTarget || Star)
            {
                _transform.position += _transform.forward * _scrollWheelChange * Vector3.Distance(_transform.position, CameraTarget ? CameraTarget.position : Star.position) / 10f;
            }
            else
            {
                _transform.position += _transform.forward * _scrollWheelChange * 10f;
            }
        }
    }
}
