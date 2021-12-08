using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoopLists : MonoBehaviour
{

    [SerializeField]
    private Scales scales;


    [SerializeField]
    private TMP_Dropdown _planetsListDropDown;

    [SerializeField]
    private StellarSystemData _stellarSystemData;

    [SerializeField]
    private GameObject _spherePrefab;

    [SerializeField]
    private GameObject _stellarsystemPrefab;

    [SerializeField]
    private GameObject _starPrefab;

    [SerializeField]
    private GameObject _planetLogicPrefab;

    [SerializeField]
    private Controller _controller;

    private List<string> _stellarBodiesList;

    private bool _stellarSystemGenerated;

    private GameObject _newStellarSystem, _newStar;
    public GameObject NewStellarSystem { get => _newStellarSystem; set => _newStellarSystem = value; }
    public GameObject NewStar { get => _newStar; set => _newStar = value; }
    public StellarSystemData StellarSystemData { get => _stellarSystemData; set => _stellarSystemData = value; }
    public bool StellarSystemGenerated { get => _stellarSystemGenerated; set => _stellarSystemGenerated = value; }

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!StellarSystemGenerated)
        {
            _controller.ClearTrails();

        }
 
        foreach(StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
        {
            if(stellarObject.ObjectTrail)
            {
                stellarObject.ObjectTrail.gameObject.SetActive(StellarSystemGenerated);
            }
        }
        
    }

    public void GenerateStellarSystem()
    {
        StellarSystemGenerated = false;

        _stellarBodiesList = new List<string>();

        NewStellarSystem = Instantiate(_stellarsystemPrefab);

        NewStellarSystem.name = $"{StellarSystemData.Name} - stellar system";

        GenerateStar(StellarSystemData);

        foreach (PlanetData planetData in StellarSystemData.ChildrenItem)
        {
            GameObject newPlanet = Instantiate(_planetLogicPrefab, NewStellarSystem.transform);

            StellarObject newPlanetStellarObject = newPlanet.GetComponentInChildren<StellarObject>();

            newPlanetStellarObject.PlanetData = planetData;
            newPlanetStellarObject.ObjectType = "planet";
            newPlanetStellarObject.LoopLists = this;


            newPlanet.name = $"{planetData.Name} - Planet Orbit Anchor";

            _stellarBodiesList.Add(planetData.Name);

            foreach (PlanetData moonData in planetData.ChildrenItem)
            {
                GameObject newMoon = Instantiate(_planetLogicPrefab, newPlanet.transform.GetChild(0).GetChild(0).Find("SatellitesHolder").transform);

                StellarObject newMoonStellarObject = newMoon.GetComponentInChildren<StellarObject>();

                newMoonStellarObject.PlanetData = moonData;
                newMoonStellarObject.ObjectType = "moon";
                newMoonStellarObject.LoopLists = this;
                newMoonStellarObject.ParentStellarObject = planetData.Name;

                newMoon.name = $"{moonData.Name} - Moon Orbit Anchor";

                _stellarBodiesList.Add($"    {moonData.Name}");
            }
        }

        FillPlanetsDropDownList(_stellarBodiesList, _planetsListDropDown);
        
        StellarSystemGenerated = true;

        DeployStellarSystem();
    }

    private void DeployStellarSystem()
    {
        NewStellarSystem.GetComponent<ToggleStellarSystem>().DeployStellarSystem();
    }

    public void GenerateStar(StellarSystemData starInfos)
    {
        NewStar = Instantiate(_starPrefab, NewStellarSystem.transform);
        NewStar.GetComponent<Star>().StellarSystemData = StellarSystemData;
        NewStar.name = $"{starInfos.StarName} - Star";

        _stellarBodiesList.Add($"<b>{starInfos.StarName}</b>");
    }

    public void FillPlanetsDropDownList(List<string> stellarBodiesList, TMP_Dropdown planetsListDropDown)
    {
        planetsListDropDown.ClearOptions();

        foreach (string StellarBodyName in stellarBodiesList)
        {
            planetsListDropDown.AddOptions(new List<string> { StellarBodyName });
        }
    }

    public float dimRet(float val, float scale, bool rationalizeValues)
    {
        if (val < 0)
        {
            return -dimRet(-val, scale, rationalizeValues);
        }

        float mult = val / scale;
        float trinum = (Mathf.Sqrt(4.0f * mult + 1.0f) - 1.0f) / 2.0f;

        if (!rationalizeValues)
        {
            return val;
        }
        else
        {
            return trinum * scale;
        }
    }

}
