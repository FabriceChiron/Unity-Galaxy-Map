using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraFollow : MonoBehaviour
{
    private Transform _transform;
    private Transform _cameraTarget;
    private Transform _previousCameraTarget;

    private UITest _UITest;

    [SerializeField]
    private Camera MainCamera;

    [SerializeField]
    private float _sensitivity = 3f;

    [SerializeField] 
    private string _startingPoint;

    [SerializeField]
    private Controller _controller;

    [SerializeField]
    private Transform _cameraFollower;

    private float _scrollWheelChange;

    [SerializeField]
    private TMP_Dropdown PlanetListDropdown;

    private bool _mouseOnUI;
    private bool _isRotating;
    private bool _gettingLongClick;
    private bool _isFocusing;
    private bool _soundFadeOut;
    private bool _canSnap;
    private int _fadeOutCount;
    private int _fadeInCount;
    private int _countDistance;

    private float mouseHorizontal;
    private float mouseVertical;
    private float _targetThreshold = 0.1f;

    private float _initialDistance;
    private float _distanceRatio;
    private float _remainingDistance;
    private float _turnSpeed;
    private float _focusSpeed;
    //private float _timeBeforeRotate = 0.2f;
    //private float _totalClickTime;

    private SphereCollider _cameraCollider;

    private Rigidbody _targetRigidBody;

    private float _timeToSnap = 5f;
    private float _resetTimeToSnap;

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
    public bool GettingLongClick { get => _gettingLongClick; set => _gettingLongClick = value; }
    public bool IsRotating { get => _isRotating; set => _isRotating = value; }
    public bool IsFocusing { get => _isFocusing; set => _isFocusing = value; }
    public bool SoundFadeOut { get => _soundFadeOut; set => _soundFadeOut = value; }

    public Transform CameraAnchor { get => _cameraAnchor; set => _cameraAnchor = value; }
    public GameObject CameraAnchorObject { get => _cameraAnchorObject; set => _cameraAnchorObject = value; }
    public Vector3 InitPosition { get => _initPosition; set => _initPosition = value; }
    public Quaternion InitRotation { get => _initRotation; set => _initRotation = value; }
    public Controller Controller { get => _controller; set => _controller = value; }
    public int FadeInCount { get => _fadeInCount; set => _fadeInCount = value; }
    public int FadeOutCount { get => _fadeOutCount; set => _fadeOutCount = value; }
    public float InitialDistance { get => _initialDistance; set => _initialDistance = value; }
    public float TargetThreshold { get => _targetThreshold; set => _targetThreshold = value; }
    public float DistanceRatio { get => _distanceRatio; set => _distanceRatio = value; }
    public float RemainingDistance { get => _remainingDistance; set => _remainingDistance = value; }
    public int CountDistance { get => _countDistance; set => _countDistance = value; }
    public Transform PreviousCameraTarget { get => _previousCameraTarget; set => _previousCameraTarget = value; }
    public bool CanSnap { get => _canSnap; set => _canSnap = value; }
    public Rigidbody TargetRigidBody { get => _targetRigidBody; set => _targetRigidBody = value; }
    public SphereCollider CameraCollider { get => _cameraCollider; set => _cameraCollider = value; }
    public float TurnSpeed { get => _turnSpeed; set => _turnSpeed = value; }
    public float FocusSpeed { get => _focusSpeed; set => _focusSpeed = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _transform = GetComponent<Transform>();

        InitPosition = _transform.position;
        InitRotation = _transform.rotation;

        _resetTimeToSnap = _timeToSnap;

        CameraCollider = GetComponent<SphereCollider>();

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

        Debug.Log($"Distance to star target: {Vector3.Distance(CameraTarget.position, transform.position)}");
        
        if (init)
        {
            InitCamera();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Controller.HasPlayer)
        {
            _cameraFollower.position = transform.position;

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
        

            //Set cursor visibility and lockstate depending on camera rotation by mouse/touch inputs
            Cursor.visible = !IsRotating;
            Cursor.lockState = IsRotating ? CursorLockMode.Locked : CursorLockMode.None;
        }

    }

    private void TurnTowardsTarget(float speed)
    {
        //Debug.Log(speed);
        if (CameraTarget)
        {
            Vector3 lookDirection = CameraTarget.position - _transform.position;
            lookDirection.Normalize();

            _transform.rotation = Controller.SmoothDamp(_transform.rotation, Quaternion.LookRotation(lookDirection), ref nullQuaternion, speed);
        }
    }

    private void FixedUpdate()
    {
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");

        _scrollWheelChange = Input.GetAxis("Mouse ScrollWheel");

        if (!Controller.HasPlayer)
        {
            //Separate Mouse/Touch input functions to avoid conflicts (hopefully)
            if (Controller.InputType == InputType.MOUSE || Controller.InputType == InputType.BOTH)
            {
                MouseInteractions();
            }

            if (Controller.InputType == InputType.TOUCH || Controller.InputType == InputType.BOTH)
            {
                if (Input.touchCount > 0)
                {
                    TouchInteractions();
                }
            }


            if (CameraTarget)
            {

                if (IsFocusing)
                {

                    //Debug.Log($"InitialDistance: {InitialDistance}");

                    FadeInCount++;
                    TriggerFadeSound(FadeInCount, "in");
                    float currentDistance = GetDistanceToTarget();

                    float distanceBetweenTargets = Vector3.Distance(PreviousCameraTarget.position, CameraTarget.position);

                    DistanceRatio = currentDistance / InitialDistance;
                    RemainingDistance = InitialDistance - currentDistance;



                    //Debug.Log($"distanceRatio: {DistanceRatio} - remainingDistance: {RemainingDistance} - InitialDistance: {InitialDistance}");
                    //float turnSpeed = Controller.FadeTime * distanceRatio * (currentDistance > TargetThreshold ? 1f : 3f);


                    //Debug.Log($"distanceBetweenTargets: {distanceBetweenTargets}\ncurrentDistance: {currentDistance}");

                    /*if (CameraTarget.GetComponent<StellarObject>() != null && PreviousCameraTarget.GetComponent<StellarObject>() != null)
                    {
                        //Debug.Log($"Current: {CameraTarget.name} parent: {CameraTarget.GetComponent<StellarObject>().ParentStellarObject}");

                        //Debug.Log($"Prev: {PreviousCameraTarget.name} parent: {PreviousCameraTarget.GetComponent<StellarObject>().ParentStellarObject}");

                        if (CameraTarget.GetComponent<StellarObject>().ParentStellarObject == PreviousCameraTarget.name
                            || PreviousCameraTarget.GetComponent<StellarObject>().ParentStellarObject == CameraTarget.name)
                        {
                            //Debug.Log($"{CameraTarget.name} and {PreviousCameraTarget.name} are related");
                            turnSpeed = Controller.FadeTime * distanceRatio / 25f;
                        }

                        else
                        {
                            turnSpeed = Controller.FadeTime * distanceRatio;
                        }
                    }
                    else
                    {
                        turnSpeed = Controller.FadeTime * distanceRatio;
                    }*/

                    //turnSpeed = Controller.FadeTime * distanceRatio * (CanSnap ? Mathf.Lerp(1f, 0.5f, 2f * Time.deltaTime) : 1f);
                    TurnSpeed = Controller.FadeTime * DistanceRatio * (CanSnap ? 0.25f : 1f);

                    /*Debug.Log($"turnSpeed: {turnSpeed}");

                    Debug.Log($"currentDistance: {currentDistance}");
                    Debug.Log($"currentDistance / TargetThreshold: {currentDistance / TargetThreshold}");*/

                    Controller.DeviceInfo.text = $"currentDistance: {currentDistance}\n" +
                        $"distanceRatio: {DistanceRatio}\n" +
                        $"remainingDistance: {RemainingDistance}\n" +
                        $"InitialDistance: {InitialDistance}\n" +
                        $"TurnSpeed: {TurnSpeed}\n" +
                        $"FocusSpeed: {FocusSpeed}";

                    TurnTowardsTarget(TurnSpeed);

                    //Debug.Log($"distanceRatio: {distanceRatio}");
                    //Debug.Log($"currentDistance / (TargetThreshold): {currentDistance / (TargetThreshold)}");

                    //Debug.Log($"currentDistance: {currentDistance}\nTargetThreshold: {TargetThreshold}");

                    if (DistanceRatio <= .25f)
                    {
                        FadeOutCount++;
                        //Debug.Log(FadeOutCount);
                        SoundFadeOut = true;

                        TriggerFadeSound(FadeOutCount, "out");

                        //Debug.Log($"turnSpeed * distanceRatio: {turnSpeed * distanceRatio}");
                        //TurnTowardsTarget(turnSpeed * distanceRatio);

                        DetectCollidersOverlap(CameraTarget.position, CameraCollider.radius);

                        if (TurnSpeed * DistanceRatio > 0.002)
                        {

                        }


                    }

                    if (CanSnap)
                    {
                        _timeToSnap -= Time.deltaTime;

                        if (_timeToSnap <= 0f)
                        {
                            Debug.Log("Snap to target");
                            CancelFocus();
                        }

                    }
                    /*if (distanceRatio <= 0.025f)
                    {
                        //Debug.Log("By distanceRatio:");
                        CancelFocus();
                    }*/


                }
                else
                {
                    TurnTowardsTarget(0.1f);
                    InitialDistance = GetDistanceToTarget();
                }
            }


            //If "Focus" is toggled ON, fire FocusOnTarget function
            if (IsFocusing && CameraTarget != null)
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

        
    }

    void DetectCollidersOverlap(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log($"DetectCollidersOverlap: {hitCollider.transform.name}");
            if(hitCollider.GetComponent<GetMainBody>() != null && hitCollider.GetComponent<GetMainBody>().MainBody == CameraTarget)
            {
                CanSnap = true;
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
        Vector2 touchPos = MainCamera.ScreenToWorldPoint(touch.position);
    }

    // Si le doigt vient de bouger
    private void OnMoveSwipe(Touch touch)
    {
        // On calcule la position du doigt DANS LE WORLD (touch.position renvoie la position du doigt SUR L'ECRAN en pixels)
        Vector2 touchPos = MainCamera.ScreenToWorldPoint(touch.position);
        // On calcule la position du doigt à la dernière frame DANS LE WORLD (touch.deltaPosition renvoie la différence entre la position actuelle et la dernière position du doigt sur l'ecran en pixels)
        Vector2 touchPreviousPosition = MainCamera.ScreenToWorldPoint(touch.position - touch.deltaPosition);

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
        _transform.rotation = Controller.SmoothDamp(_transform.rotation, Quaternion.LookRotation(lookDirection), ref nullQuaternion, 0.1f);
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
        PreviousCameraTarget = CameraTarget;

        CameraTarget = newCameraTarget;

        TargetRigidBody = CameraTarget.GetComponent<Rigidbody>();

        InitialDistance = GetDistanceToTarget();

        ChangeSelectionInDropdown(newCameraTarget.name);
    }

    public void ChangeTarget(string PlanetName)
    {
        string StrippedPlanetName = PlanetName.Replace("    ", "").Replace("<b>", "").Replace("</b>", "");

        PreviousCameraTarget = CameraTarget;

        CameraTarget = GameObject.Find($"{StrippedPlanetName}").transform;

        TargetRigidBody = CameraTarget.GetComponent<Rigidbody>();

        InitialDistance = GetDistanceToTarget();

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
        CountDistance++;
        if(CountDistance == 1)
        {
            InitialDistance = GetDistanceToTarget();
        }

        FocusSpeed = CanSnap ? Controller.FadeTime * 0.25f : Controller.FadeTime;

        //Move Camera towards target
        Vector3 newPos = Vector3.SmoothDamp(_transform.position, CameraAnchor.position, ref velocity, FocusSpeed);
        _transform.position = newPos;

        //While Camera keeps looking at the target




        switch (componentType)
        {
            case "Planet":
                TargetThreshold = CameraTarget.GetComponent<StellarObject>().ObjectSize * 0.5f;
                break;

            case "Galaxy":
                TargetThreshold = 0.5f;
                break;

            case "Star":
                TargetThreshold = CameraTarget.GetComponent<Star>().ObjectSize * 0.5f;
                break;
        }

        /*
                if (Vector3.Distance(_transform.position, CameraAnchor.position) <= InitialDistance * 0.25f)
                {
                    FadeOutCount++;
                    //Debug.Log(FadeOutCount);
                    SoundFadeOut = true;

                    TriggerFadeOut(FadeOutCount);


                    if(InitialDistance * 0.25f < TargetThreshold * 2)
                    {
                        CancelFocus();
                    }
                }
                if (Vector3.Distance(_transform.position, CameraAnchor.position) <= InitialDistance * 0.1f) {
                    TurnTowardsTarget(0.1f);
                }*/
    }

    public void CancelFocus()
    {
        if(!Controller.HasPlayer)
        {
            Debug.Log("CancelFocus");
            CountDistance = 0;
            IsFocusing = false;
            CanSnap = false;
            FadeOutCount = 0;
            FadeInCount = 0;
            _timeToSnap = _resetTimeToSnap;
            CameraAnchor = null;
            CameraAnchorObject = null;
            TurnTowardsTarget(0.1f);
            StartCoroutine(AudioHelper.FadeOut(Controller.TravelSound, Controller.FadeTime * 2));
        }
    }

    public float GetDistanceToTarget()
    {

            if (CameraTarget.GetComponent<StellarObject>())
            {
                CameraAnchor = CameraTarget.GetComponent<StellarObject>().CameraAnchor;
            }
            else if (CameraTarget.GetComponent<Star>())
            {
                CameraAnchor = CameraTarget.GetComponent<Star>().CameraAnchor;
            }

            float distanceToTarget = Vector3.Distance(_transform.position, CameraAnchor.position);

            //Debug.Log($"DistanceToTarget: {distanceToTarget}");

            return distanceToTarget;
    }

    private void TriggerFadeSound(int count, string OutIn)
    {
        if(count == 1)
        {

            switch(OutIn)
            {
                case "in":
                    Debug.Log("FadeIn Sound");
                    StartCoroutine(AudioHelper.FadeIn(Controller.TravelSound, Controller.FadeTime));
                    break;
                case "out":
                    Debug.Log("FadeOut Sound");
                    StartCoroutine(AudioHelper.FadeOut(Controller.TravelSound, Controller.FadeTime));
                    break;
            }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "CameraAnchor")
        {
            if(other.GetComponent<GetMainBody>().MainBody == CameraTarget)
            {
                Debug.Log("Entering CameraAnchor");
                CancelFocus();
            }
        }

        if (other.transform.name == "Bubble")
        {
            if (other.GetComponent<GetMainBody>().MainBody == CameraTarget)
            {
                Debug.Log("Entering Bubble");
                CanSnap = true;
                TriggerFadeSound(FadeOutCount, "out");
            }
        }

        /*if ((other.transform.parent.tag == "Planet" || other.transform.parent.tag == "Star") && other.transform.parent == CameraTarget)
        {
            Debug.Log($"Entering Stellar Object: {other.transform.parent.name}");
            CanSnap = true;
            TriggerFadeSound(FadeOutCount, "out");
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter: {collision.transform.parent.name}");
    }

    
}
