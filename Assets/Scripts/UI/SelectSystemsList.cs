using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SelectSystemsList : MonoBehaviour
{

    [SerializeField]
    private StellarSystemData[] _stellarSystemsArray;

    [SerializeField]
    private LoopLists _loopLists;

    [SerializeField]
    private GameObject _stellarSystemPrefab;

    private TMP_Dropdown _systemsDropdown;

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

    private GameObject currentStellarSystem;

    private void Awake()
    {
        //_controller = LoopLists.GetComponent<Controller>();
        //StartCoroutine(AudioHelper.FadeIn(_controller.TravelSound, _controller.FadeTime));
        _systemsDropdown = GetComponent<TMP_Dropdown>();

        _resetTimeBeforeDeploy = _timeBeforeDeploy;
        _resetTimeBeforeResetCam = _timeBeforeResetCam;

        foreach (StellarSystemData stellarSystemItem in _stellarSystemsArray)
        {
            _systemsDropdown.AddOptions(new List<string> { stellarSystemItem.name });
        }

        LoopLists.StellarSystemData = _stellarSystemsArray[0];


        LoopLists.GenerateStellarSystem();



        //SelectSolarSystem(_systemsDropdown);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SwitchStellarSystems();
        //FireResetCamera();
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
            Camera.main.GetComponent<CameraFollow>().ResetCameraTarget(true);

            if(LoopLists.NewStellarSystem.GetComponent<Animator>().GetFloat("Scale") == 0) {


                LoopLists.NewStellarSystem.SetActive(false);
                Destroy(LoopLists.NewStellarSystem);
                ChangeStellarSystem = false;

                LoopLists.StellarSystemData = _stellarSystemsArray[_systemsDropdown.value];

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
        LoopLists.NewStellarSystem.GetComponent<ToggleStellarSystem>().FoldStellarSystem();

        ChangeStellarSystem = true;
    }
}
