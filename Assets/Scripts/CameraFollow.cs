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
    private TMP_Dropdown PlanetListDropdown;

    private bool _mouseOnUI;
    private bool _isRotating;
    private bool _gettingLongClick;
    private bool _isFocusing;

    private float mouseHorizontal;
    private float mouseVertical;
    //private float _timeBeforeRotate = 0.2f;
    //private float _totalClickTime;

    private Vector3 velocity = Vector3.zero;
    private Quaternion nullQuaternion = Quaternion.identity;

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
    public bool IsRotating { get => _isRotating; set => _isRotating = value; }
    public bool GettingLongClick { get => _gettingLongClick; set => _gettingLongClick = value; }
    public bool IsFocusing { get => _isFocusing; set => _isFocusing = value; }

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
        
        /*if(GameObject.FindGameObjectWithTag("Star") != null)
        {
            ResetCameraTarget();
        }*/

        UITest = Controller.UITest;
    }

    public void InitCamera()
    {
        transform.parent = null;
        CameraTarget = null;
        /*transform.position = InitPosition;
        transform.rotation = InitRotation;*/
    }

    public void ResetCameraTarget(bool init)
    {
        
        Star = null;

        foreach (StarData starData in Controller.LoopLists.StellarSystemData.StarsItem)
        {
            if(starData.ChildrenItem.Length > 0)
            {
                Star = GameObject.Find(starData.Name).transform;
            }
        }

        if(Star == null)
        {
            Star = GameObject.Find(Controller.LoopLists.StellarSystemData.StarsItem[0].Name).transform;
        }
        
        ChangeTarget(Star);

        CameraAnchor = Star.GetComponent<Star>().CameraAnchor;
        
        if (init)
        {
            InitCamera();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (UITest != null)
        {
            //Detect if mouse if over UI Elements
            MouseOnUI = UITest.IsPointerOverUIElement();
        }
        if (!MouseOnUI)
        {
            //Allow zoomin/zoomout only when mouse is not over UI Elements
            ZoomCamera();
        }

        //Set camera parent (when a stellar object is clicked)
        if(CameraTarget != Star && CameraTarget != null)
        {
            _transform.parent = CameraTarget.parent;
        }
        else
        {
            _transform.parent = null;
        };

        //Rotate Camera towards targeted stellar object
        if (CameraTarget)
        {
            Vector3 lookDirection = CameraTarget.position - _transform.position;
            lookDirection.Normalize();

            _transform.rotation = SmoothDamp(_transform.rotation, Quaternion.LookRotation(lookDirection), ref nullQuaternion, 0.1f);
        }

        //Set cursor visibility and lockstate depending on camera rotation by mouse/touch inputs
        Cursor.visible = !IsRotating;
        Cursor.lockState = IsRotating ? CursorLockMode.Locked : CursorLockMode.None;

    }

    private void FixedUpdate()
    {
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");

        _scrollWheelChange = Input.GetAxis("Mouse ScrollWheel");


        //Separate Mouse/Touch input functions to avoid conflicts (hopefully)
        if (Controller.InputType == InputType.MOUSE || Controller.InputType == InputType.BOTH)
        {
            MouseInteractions();
        }

        if (Controller.InputType == InputType.TOUCH || Controller.InputType == InputType.BOTH)
        {
            if(Input.touchCount > 0)
            {
                TouchInteractions();
            }
        }

        //If "Focus" is toggled ON, fire FocusOnTarget function
        if(IsFocusing && CameraTarget != null)
        {
            if (CameraTarget.GetComponent<StellarObject>() != null)
            {
                FocusOnTarget("Planet");
                
            }
            else if (CameraTarget.GetComponent<Star>() != null)
            {
                FocusOnTarget("Star");
            }
            else if (CameraAnchorObject.GetComponent<Galaxy>() != null)
            {
                FocusOnTarget("Galaxy");
            }
        }
    }

    private void TouchInteractions()
    {
        // Si au moins un doigt est utilisé
        if (Input.touchCount > 0)
        {
            Debug.Log($"TouchInteractions");
            // On récupère l'objet Touch pour le premier doigt
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {
                // Si le doigt vient d'être posé
                if (touch.phase == TouchPhase.Began)
                {
                    OnStartSwipe(touch);
                }
                // Si le doigt vient de bouger
                else if (touch.phase == TouchPhase.Moved)
                {
                    OnMoveSwipe(touch);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    OnEndSwipe(touch);
                }
            }

            if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Debug.Log($"{touch0.position} - {touch1.position}");

                if (touch.phase == TouchPhase.Began)
                {
                    OnStartPinch(touch0, touch1);
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    OnMovePinch(touch0, touch1);
                }
            }

        }
    }

    private void MouseInteractions()
    {
        if (Input.GetMouseButtonDown(1) && !MouseOnUI)
        {
            IsRotating = true;
            //_totalClickTime = 0;
            //GettingLongClick = true;
        }

        /*if (Input.GetMouseButton(1) && GettingLongClick)
        {
            _totalClickTime += Time.deltaTime;

            if (_totalClickTime >= _timeBeforeRotate)
            {
                IsRotating = true;
                RotateAroundObject();
            }
            else
            {
                IsRotating = false;
            }
        }*/
        if (Input.GetMouseButton(1))
        {
            RotateAroundObject();
        }

        if (IsRotating && Input.GetMouseButtonUp(1))
        {
            IsRotating = false;
            //GettingLongClick = false;
        }


        //Move camera with Middle Mouse Button
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
        }
    }

    // Si le doigt vient d'être posé
    private void OnStartSwipe(Touch touch)
    {
        // On calcule la position du doigt DANS LE WORLD (touch.position renvoie la position du doigt SUR L'ECRAN en pixels)
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
    }

    // Si le doigt vient de bouger
    private void OnMoveSwipe(Touch touch)
    {
        // On calcule la position du doigt DANS LE WORLD (touch.position renvoie la position du doigt SUR L'ECRAN en pixels)
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        // On calcule la position du doigt à la dernière frame DANS LE WORLD (touch.deltaPosition renvoie la différence entre la position actuelle et la dernière position du doigt sur l'ecran en pixels)
        Vector2 touchPreviousPosition = Camera.main.ScreenToWorldPoint(touch.position - touch.deltaPosition);

        Debug.Log($"deltaPosition: {touch.deltaPosition}");

        mouseHorizontal = touch.deltaPosition.x / 10f;
        mouseVertical = touch.deltaPosition.y / 10f;

        if (!MouseOnUI)
        {
            RotateAroundObject();
        }
    }

    private void OnEndSwipe(Touch touch)
    {
        Vector3 lookDirection = CameraTarget.position - _transform.position;
        lookDirection.Normalize();
        _transform.rotation = SmoothDamp(_transform.rotation, Quaternion.LookRotation(lookDirection), ref nullQuaternion, 0.1f);
    }

    private void OnStartPinch(Touch touch0, Touch touch1)
    {
        Vector2 touchPos0 = touch0.position;
        Vector2 touchPreviousPos0 = touch0.position - touch0.deltaPosition;

        Vector2 touchPos1 = touch1.position;
        Vector2 touchPreviousPos1 = touch1.position - touch1.deltaPosition;
    }

    private void OnMovePinch(Touch touch0, Touch touch1)
    {
        Vector2 touchPos0 = touch0.position;
        Vector2 touchPreviousPos0 = touch0.position - touch0.deltaPosition;

        Vector2 touchPos1 = touch1.position;
        Vector2 touchPreviousPos1 = touch1.position - touch1.deltaPosition;

        Debug.Log($"Previous: {touchPreviousPos0} - {touchPreviousPos1}");
        Debug.Log($"Now: {touchPos0} - {touchPos1}");
        //Debug.Log($"Now: {Vector2.Distance(touchPos0, touchPos1)}");

        if (Vector2.Distance(touchPreviousPos0, touchPreviousPos1) > Vector2.Distance(touchPos0, touchPos1))
        {
            Debug.Log("Zoom");
            _scrollWheelChange = Vector2.Distance(touch0.deltaPosition, touch1.deltaPosition) * -0.1f;
        }
        else if (Vector2.Distance(touchPreviousPos0, touchPreviousPos1) < Vector2.Distance(touchPos0, touchPos1))
        {
            Debug.Log("Pinch");
            _scrollWheelChange = Vector2.Distance(touch0.deltaPosition, touch1.deltaPosition) * 0.1f;
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
        //Move Camera towards target
        Vector3 newPos = Vector3.SmoothDamp(_transform.position, CameraAnchor.position, ref velocity, 1f);
        _transform.position = newPos;

        //While Camera keeps looking at the target
        _transform.LookAt(CameraTarget);

        float targetThreshold = 0.1f;

        switch (componentType)
        {
            case "Planet":
                targetThreshold = CameraTarget.GetComponent<StellarObject>().ObjectSize * 0.5f;
                break;
            
            case "Galaxy":
                targetThreshold = 0.5f;
                break;

            case "Star":
                targetThreshold = CameraTarget.GetComponent<Star>().ObjectSize * 0.5f;
                break;
        }

        if (Vector3.Distance(_transform.position, CameraAnchor.position) <= targetThreshold * 2)
        {
            StartCoroutine(AudioHelper.FadeOut(Controller.TravelSound, Controller.FadeTime * 2));
        }

        if (Vector3.Distance(_transform.position, CameraAnchor.position) <= targetThreshold)
        {
            //Controller.TravelSound.Stop();
            
            IsFocusing = false;
            CameraAnchor = null;
            CameraAnchorObject = null;
        } else
        {
/*            if(Controller.InputType == InputType.TOUCH)
            {
                RotateAroundObject();
            }*/
        }
        

    }

    private void ZoomCamera()
    {

        if(_scrollWheelChange != 0f)
        {
            CameraAnchor = null;

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

    public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
    {
        if (Time.deltaTime < Mathf.Epsilon) return rot;
        // account for double-cover
        var Dot = Quaternion.Dot(rot, target);
        var Multi = Dot > 0f ? 1f : -1f;
        target.x *= Multi;
        target.y *= Multi;
        target.z *= Multi;
        target.w *= Multi;
        // smooth damp (nlerp approx)
        var Result = new Vector4(
            Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
            Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
            Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
            Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
        ).normalized;

        // ensure deriv is tangent
        var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
        deriv.x -= derivError.x;
        deriv.y -= derivError.y;
        deriv.z -= derivError.z;
        deriv.w -= derivError.w;

        return new Quaternion(Result.x, Result.y, Result.z, Result.w);
    }
}
