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

    private bool _changeStellarSystem;

    private float _timeBeforeDeploy = 1f;

    public bool ChangeStellarSystem { get => _changeStellarSystem; set => _changeStellarSystem = value; }

    private GameObject currentStellarSystem;
    private GameObject newStellarSystem;

    private void Awake()
    {
        _systemsDropdown = GetComponent<TMP_Dropdown>();

        currentStellarSystem = GameObject.FindGameObjectWithTag("StellarSystem");

        foreach (StellarSystemData stellarSystemItem in _stellarSystemsArray)
        {
            _systemsDropdown.AddOptions(new List<string> { stellarSystemItem.name });
        }
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
            Debug.Log($"Switching Stellar Systems");

            _timeBeforeDeploy -= Time.deltaTime;

            currentStellarSystem.GetComponent<Animator>().SetBool("IsDeployed", false);

            if (_timeBeforeDeploy <= 0)
            {
                Camera.main.transform.parent = null;
                ChangeStellarSystem = false;
                Destroy(currentStellarSystem);
                newStellarSystem.GetComponent<Animator>().SetBool("IsDeployed", true);
            }
        }
    }

    public void SelectSolarSystem(TMP_Dropdown Dropdown)
    {
        Debug.Log(_stellarSystemsArray[Dropdown.value]);

        newStellarSystem = Instantiate(_stellarSystemPrefab, Vector3.zero, Quaternion.identity);

        GeneratePlanets newGenerator = newStellarSystem.GetComponent<GeneratePlanets>();

        newGenerator.StellarSystemData = _stellarSystemsArray[Dropdown.value];

        newGenerator.GenerateStellarSystem();

        currentStellarSystem = GameObject.FindGameObjectWithTag("StellarSystem");

        ChangeStellarSystem = true;
    }
}
