using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SelectSystemsList : MonoBehaviour
{

    [SerializeField]
    private StellarSystemsArray _stellarSystemsArray;

    [SerializeField]
    private Memory _memory;

    [SerializeField]
    private LoopLists _loopLists;

    [SerializeField]
    private GameObject _stellarSystemPrefab;

    private TMP_Dropdown _systemsDropdown;

    [SerializeField]
    private Controller _controller;
    
    private bool _changeStellarSystem = false;
    private bool _resetCamera = false;

    private float _timeBeforeDeploy = 1;
    private float _resetTimeBeforeDeploy;

    private float _timeBeforeResetCam = 1f;
    private float _resetTimeBeforeResetCam;

    public bool ChangeStellarSystem { get => _changeStellarSystem; set => _changeStellarSystem = value; }
    public bool ResetCamera { get => _resetCamera; set => _resetCamera = value; }
    public LoopLists LoopLists { get => _loopLists; set => _loopLists = value; }
    public Memory Memory { get => _memory; set => _memory = value; }
    public StellarSystemsArray StellarSystemsList { get => _stellarSystemsArray; set => _stellarSystemsArray = value; }

    private GameObject currentStellarSystem;

    private SC_SpaceshipController _spaceshipController;

    private void Awake()
    {
        //_controller = LoopLists.GetComponent<Controller>();
        //StartCoroutine(AudioHelper.FadeIn(_controller.TravelSound, _controller.FadeTime));
        _systemsDropdown = GetComponent<TMP_Dropdown>();

        _resetTimeBeforeDeploy = _timeBeforeDeploy;
        _resetTimeBeforeResetCam = _timeBeforeResetCam;

        foreach (StellarSystemData stellarSystemItem in _stellarSystemsArray.stellarSystemsArray)
        {
            _systemsDropdown.AddOptions(new List<string> { stellarSystemItem.name });
        }

        if(Memory != null && Memory.SavedData.SelectedSystem != null)
        {
            
            LoopLists.StellarSystemData = Memory.SavedData.SelectedSystem;

            int i = 0;
            foreach (StellarSystemData stellarSystemItem in _stellarSystemsArray.stellarSystemsArray)
            {
                if(stellarSystemItem == Memory.SavedData.SelectedSystem)
                {
                    _systemsDropdown.SetValueWithoutNotify(i);
                }
                i++;
            }
        }
        else
        {
            LoopLists.StellarSystemData = _stellarSystemsArray.stellarSystemsArray[0];
        }

        LoopLists.GenerateStellarSystem();



        //SelectSolarSystem(_systemsDropdown);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_controller.HasPlayer)
        {
            _spaceshipController = _controller.Player.GetComponent<SC_SpaceshipController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SwitchStellarSystems();
        //FireResetCamera();

        if(Memory != null)
        {
            Memory.SavedData.SelectedSystem = _stellarSystemsArray.stellarSystemsArray[_systemsDropdown.value];
        }

        if (!_controller.IsPaused)
        {
            _systemsDropdown.Hide();
        }
    }

/*    private void FireResetCamera()
    {
        if(ResetCamera)
        {
            _timeBeforeResetCam -= Time.deltaTime;

            if (_timeBeforeResetCam <= 0)
            {
                Debug.Log("Camera Reset");
                Camera.main.GetComponent<CameraFollow>().ResetCameraTarget();
                _timeBeforeResetCam = _resetTimeBeforeResetCam;

                ResetCamera = false;
            }
        }
    }*/

    private void SwitchStellarSystems()
    {
        if (ChangeStellarSystem)
        {
            if (!_controller.HasPlayer)
            {
                _controller.MainCamera.GetComponent<CameraFollow>().ResetCameraTarget(true);
            }
            /*
            else
            {
                _spaceshipController.TurnTowardsTarget(LoopLists.NewStellarSystem.transform.position, 0.5f);
            }
            */

            if (LoopLists.NewStellarSystem.GetComponent<Animator>().GetFloat("Scale") == 0) {


                LoopLists.NewStellarSystem.SetActive(false);
                Destroy(LoopLists.NewStellarSystem);
                ChangeStellarSystem = false;

                //LoopLists.StellarSystemData = _stellarSystemsArray[_systemsDropdown.value];
                LoopLists.StellarSystemData = _stellarSystemsArray.stellarSystemsArray[_systemsDropdown.value];

                LoopLists.GenerateStellarSystem();

            }

            

            /*if (_timeBeforeDeploy <= 0)
            {
                Camera.main.transform.parent = null;
                ChangeStellarSystem = false;
                if (currentStellarSystem != null)
                {
                    currentStellarSystem.SetActive(false);
                    Destroy(currentStellarSystem);
                }

                LoopLists.StellarSystemData = _stellarSystemsArray[_systemsDropdown.value];

                LoopLists.GenerateStellarSystem();

                _timeBeforeDeploy = _resetTimeBeforeDeploy;

                _timeBeforeResetCam = _resetTimeBeforeResetCam;

                ResetCamera = true;

            }*/
        }
    }

    public void SelectSolarSystem(TMP_Dropdown Dropdown)
    {
        Debug.Log("yo");

        

        LoopLists.NewStellarSystem.GetComponent<ToggleStellarSystem>().FoldStellarSystem();

        ChangeStellarSystem = true;

        _controller.IsPaused = false;
    }
}
