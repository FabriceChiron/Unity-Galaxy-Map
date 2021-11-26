using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _transform;
    private Transform _cameraTarget;

    private UITest _UITest;

    [SerializeField]
    private float _sensitivity = 3f;

    [SerializeField] 
    private string _startingPoint;

    private float _scrollWheelChange;

    [SerializeField]
    private float _speed = 3f;

    private bool _mouseOnUI;

    private Transform _star, _cameraAnchor;
    private Planet _cameraAnchorPlanet;

    public string StartingPoint { get => _startingPoint; set => _startingPoint = value; }
    public float Sensitivity { get => _sensitivity; set => _sensitivity = value; }

    public Transform Star { get => _star; set => _star = value; }
    public Transform CameraTarget { get => _cameraTarget; set => _cameraTarget = value; }
    public UITest UITest { get => _UITest; set => _UITest = value; }
    public bool MouseOnUI { get => _mouseOnUI; set => _mouseOnUI = value; }
    public Transform CameraAnchor { get => _cameraAnchor; set => _cameraAnchor = value; }
    public Planet CameraAnchorPlanet { get => _cameraAnchorPlanet; set => _cameraAnchorPlanet = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _transform = GetComponent<Transform>();

        Star = GameObject.FindGameObjectWithTag("Star").transform;

        UITest = GameObject.FindGameObjectWithTag("StellarSystem").GetComponent<UITest>();

        CameraTarget = Star;
    }

    // Update is called once per frame
    void Update()
    {
        MouseOnUI = UITest.IsPointerOverUIElement();
        if (!MouseOnUI)
        {
            ZoomCamera();
        }

        //_transform.LookAt(_cameraTarget.position);

        if(CameraTarget != Star)
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
        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(CameraTarget.transform.position, Vector3.up, mouseHorizontal * Sensitivity); //use transform.Rotate(transform.up * mouseHorizontal * Sensitivity);
            transform.RotateAround(CameraTarget.transform.position, -Vector3.right, mouseVertical * Sensitivity);

        }

        if (Input.GetMouseButton(2))
        {
            transform.position = new Vector3(mouseHorizontal * Sensitivity, mouseVertical * Sensitivity, transform.position.z);

            //transform.Translate(transform.up * mouseVertical * Sensitivity);
            //transform.Translate(transform.right * mouseHorizontal * Sensitivity);
        }

        Debug.Log(CameraAnchor);
        if(CameraAnchor != null && CameraAnchorPlanet != null)
        {
            FocusOnTarget();
        }
    }

    public void ChangeTarget(Transform newCameraTarget)
    {
        //Debug.Log(newCameraTarget.name);
        CameraTarget = newCameraTarget;
    }

    public void ChangeTarget(string PlanetName)
    {
        Debug.Log(PlanetName);
        Debug.Log(GameObject.Find($"{PlanetName}"));
        CameraTarget = GameObject.Find($"{PlanetName}").transform;
    }

    public void FocusOnTarget()
    {
        //Debug.Log("Lerping");
        //Vector3 newPos = Vector3.Lerp(_transform.position, _cameraAnchor.position, 5f * Time.deltaTime);
        Vector3 newPos = Vector3.Lerp(_transform.position, CameraAnchor.position, 5f * Time.deltaTime);
        // On applique la nouvelle position
        _transform.position = newPos;
        Debug.Log(Vector3.Distance(_transform.position, CameraAnchor.position));
        if (Vector3.Distance(_transform.position, CameraAnchor.position) <= CameraAnchorPlanet.ObjectSize * 0.5f)
        {
            CameraAnchor = null;
            CameraAnchorPlanet = null;
        }

    }

    private void ZoomCamera()
    {
        _scrollWheelChange = Input.GetAxis("Mouse ScrollWheel");

        if(_scrollWheelChange != 0f)
        {
            //_transform.position += _transform.forward * _scrollWheelChange;
            _transform.position += _transform.forward * _scrollWheelChange * Vector3.Distance(_transform.position, CameraTarget ? CameraTarget.position : Star.position) / 10f;
        }
    }
}
