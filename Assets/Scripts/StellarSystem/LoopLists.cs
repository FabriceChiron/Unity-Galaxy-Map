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
    private GameObject _stellarsystemPrefab, _starPrefab, _planetLogicPrefab, _asteroidBeltPrefab;

    [SerializeField]
    private Controller _controller;

    private List<string> _stellarBodiesList;

    private bool _stellarSystemGenerated;

    private int _starCount, _stellarObjectCount, _asteroidCount, _stellarObjectTotal, _asteroidTotal;

    private GameObject _newStellarSystem, _newStar, _newAsteroidBelt;
    public GameObject NewStellarSystem { get => _newStellarSystem; set => _newStellarSystem = value; }
    public GameObject NewStar { get => _newStar; set => _newStar = value; }
    public GameObject NewAsteroidBelt { get => _newAsteroidBelt; set => _newAsteroidBelt = value; }
    public StellarSystemData StellarSystemData { get => _stellarSystemData; set => _stellarSystemData = value; }
    public bool StellarSystemGenerated { get => _stellarSystemGenerated; set => _stellarSystemGenerated = value; }
    public int StarCount { get => _starCount; set => _starCount = value; }
    public int StellarObjectCount { get => _stellarObjectCount; set => _stellarObjectCount = value; }
    public int AsteroidCount { get => _asteroidCount; set => _asteroidCount = value; }
    public int StellarObjectTotal { get => _stellarObjectTotal; set => _stellarObjectTotal = value; }
    public int AsteroidTotal { get => _asteroidTotal; set => _asteroidTotal = value; }

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
        Debug.Log($"StellarSystemGenerated: {StellarSystemGenerated}");

        if (!StellarSystemGenerated)
        {
            _controller.ClearTrails();

            Debug.Log($"StarCount: {StarCount} - StellarSystemData.StarsItem.Length: {StellarSystemData.StarsItem.Length}\n" +
                $"StellarObjectCount: {StellarObjectCount} - StellarObjectTotal: {StellarObjectTotal}\n" +
                $"AsteroidCount: {AsteroidCount} - AsteroidTotal: {AsteroidTotal}");

            if (StarCount == StellarSystemData.StarsItem.Length)
            {
                if (StellarObjectCount == StellarObjectTotal)
                {
                    if (AsteroidCount == AsteroidTotal)
                    {
                        _controller.SetScales();

                        DeployStellarSystem();

                        StellarSystemGenerated = true;

                    }
                }
            }

        }

        foreach (Star star in GameObject.FindObjectsOfType<Star>())
        {
            if (star.ObjectTrail)
            {
                star.ObjectTrail.gameObject.SetActive(StellarSystemGenerated);
            }
        }

        foreach (StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
        {
            if(stellarObject.ObjectTrail)
            {
                stellarObject.ObjectTrail.gameObject.SetActive(StellarSystemGenerated);
            }
        }
        
    }

    private void InitCounts()
    {
        StarCount = 0;
        StellarObjectCount = 0;
        AsteroidCount = 0;
        StellarObjectTotal = 0;
        AsteroidTotal = 0;
    }

    public void GenerateStellarSystem()
    {
        InitCounts();

        StellarSystemGenerated = false;

        _stellarBodiesList = new List<string>();

        NewStellarSystem = Instantiate(_stellarsystemPrefab);

        NewStellarSystem.name = $"{StellarSystemData.Name} - stellar system";

        foreach(StarData starData in StellarSystemData.StarsItem)
        {
            GenerateStar(starData);
        }

        foreach(AsteroidBeltData asteroidBeltData in StellarSystemData.AsteroidBeltItem)
        {
            GenerateAsteroidsBelt(asteroidBeltData);
        }

        //GenerateStar(StellarSystemData);

        StellarObjectTotal = StellarSystemData.ChildrenItem.Length;

        foreach (PlanetData planetData in StellarSystemData.ChildrenItem)
        {
            GameObject newPlanet = Instantiate(_planetLogicPrefab, NewStellarSystem.transform);

            StellarObject newPlanetStellarObject = newPlanet.GetComponentInChildren<StellarObject>();

            newPlanetStellarObject.PlanetData = planetData;
            newPlanetStellarObject.ObjectType = "planet";
            newPlanetStellarObject.LoopLists = this;


            newPlanet.name = $"{planetData.Name} - Planet Orbit Anchor";

            _stellarBodiesList.Add(planetData.Name);

            StellarObjectTotal += planetData.ChildrenItem.Length;

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
        


    }

    private void DeployStellarSystem()
    {
        NewStellarSystem.GetComponent<ToggleStellarSystem>().DeployStellarSystem();
    }

    public void GenerateAsteroidsBelt(AsteroidBeltData asteroidBeltData)
    {
        NewAsteroidBelt = Instantiate(_asteroidBeltPrefab, NewStellarSystem.transform);
        NewAsteroidBelt.GetComponent<AsteroidBelt>().AsteroidBeltData = asteroidBeltData;
        NewAsteroidBelt.GetComponent<AsteroidBelt>().Controller = GetComponent<Controller>();
        NewAsteroidBelt.GetComponent<AsteroidBelt>().LoopLists = GetComponent<LoopLists>();

        AsteroidTotal += asteroidBeltData.Quantity;
    }

    public void GenerateStar(StarData starData)
    {
        NewStar = Instantiate(_starPrefab, NewStellarSystem.transform);
        Star starScript = NewStar.GetComponentInChildren<Star>();

        starScript.StarData = starData;
        NewStar.name = $"{starData.Name}";

        starScript.LoopLists = this;

        starScript.GetComponent<Light>().intensity = starScript.GetComponent<Light>().intensity / StellarSystemData.StarsItem.Length;

        _stellarBodiesList.Add($"<b>{starData.Name}</b>");
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
