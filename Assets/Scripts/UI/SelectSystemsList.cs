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
    private GameObject _stellarSystemPrefab;

    private TMP_Dropdown _systemsDropdown;

    private bool _changeStellarSystem = false;

    private float _timeBeforeDeploy = 1.5f;
    private float _resetTimeBeforeDeploy;

    private float _timeBeforeResetCam = 1f;
    private float _resetTimeBeforeResetCam;

    public bool ChangeStellarSystem { get => _changeStellarSystem; set => _changeStellarSystem = value; }

    private GameObject currentStellarSystem;
    private GameObject newStellarSystem;

    private void Awake()
    {
        _systemsDropdown = GetComponent<TMP_Dropdown>();

        _resetTimeBeforeDeploy = _timeBeforeDeploy;
        _resetTimeBeforeResetCam = _timeBeforeResetCam;

        currentStellarSystem = GameObject.FindGameObjectWithTag("StellarSystem");

        foreach (StellarSystemData stellarSystemItem in _stellarSystemsArray)
        {
            _systemsDropdown.AddOptions(new List<string> { stellarSystemItem.name });
        }

        SelectSolarSystem(_systemsDropdown);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SwitchStellarSystems();
    }

    private void SwitchStellarSystems()
    {
        if (ChangeStellarSystem)
        {

            _timeBeforeDeploy -= Time.deltaTime;

            //Debug.Log($"Switching Stellar Systems in {_timeBeforeDeploy}");

            currentStellarSystem.GetComponent<ToggleStellarSystem>().FoldStellarSystem();

            

            if (_timeBeforeDeploy <= 0)
            {
                Camera.main.transform.parent = null;
                ChangeStellarSystem = false;
                Destroy(currentStellarSystem);

                newStellarSystem = Instantiate(_stellarSystemPrefab, Vector3.zero, Quaternion.identity);
                GeneratePlanets newGenerator = newStellarSystem.GetComponent<GeneratePlanets>();

                newGenerator.StellarSystemData = _stellarSystemsArray[_systemsDropdown.value];

                newGenerator.GenerateStellarSystem();

                newStellarSystem.GetComponent<ToggleStellarSystem>().DeployStellarSystem();


                _timeBeforeDeploy = _resetTimeBeforeDeploy;

                _timeBeforeResetCam -= Time.deltaTime;

                if(_timeBeforeResetCam <= 0)
                {
                    _timeBeforeResetCam = _resetTimeBeforeResetCam;
                }

            }
        }
    }

    public void SelectSolarSystem(TMP_Dropdown Dropdown)
    {
        currentStellarSystem = GameObject.FindGameObjectWithTag("StellarSystem");

        ChangeStellarSystem = true;
    }
}
