using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneratePlanets : MonoBehaviour
{
    [SerializeField]
    private PlanetsList _planetsList;

    [SerializeField]
    private StellarSystemData _stellarSystemData;

    [SerializeField]
    private GameObject PlanetLogicPrefab;

    [SerializeField]
    private string _galaxyName, _clusterName;

    [SerializeField]
    private bool _goDeeper;

    private Transform _transform;

    private TMP_Dropdown PlanetListDropdown;

    public PlanetsList PlanetsList { get => _planetsList; set => _planetsList = value; }
    public bool GoDeeper { get => _goDeeper; set => _goDeeper = value; }
    public string GalaxyName { get => _galaxyName; set => _galaxyName = value; }
    public string ClusterName { get => _clusterName; set => _clusterName = value; }
    public StellarSystemData StellarSystemData { get => _stellarSystemData; set => _stellarSystemData = value; }

    private void Awake()
    {
        if(GalaxyName == "")
        {
            GalaxyName = "Milky Way";
        }
        if(ClusterName == "")
        {
            ClusterName = "Local Cluster";
        }

        PlanetListDropdown = GameObject.FindGameObjectWithTag("PlanetsList").GetComponent<TMP_Dropdown>();
        
        if (StellarSystemData != null && StellarSystemData.ChildrenItem.Length > 0 && GoDeeper)
        {
            PlanetListDropdown.AddOptions(new List<string> { GameObject.FindGameObjectWithTag("Star").name });
            LoopPlanetsList(true, StellarSystemData.ChildrenItem, "", "planet");
        }

/*
        if(StellarSystemData)
        {
            LoopTestList(StellarSystemData.ChildrenItem);
        }*/
    }

    public void LoopTestList(PlanetData[] ChildrenItem)
    {
        foreach (PlanetData planetData in ChildrenItem)
        {
            Debug.Log($"stellarSystemData: {planetData.Name}");
        }
    }

    public void LoopPlanetsList(bool goDeeper, PlanetData[] ChildrenItem, string ParentName, string objectType)
    {
        //Debug.Log($"{name}: goDeeper is set to {goDeeper}, PlanetsList is {PlanetsList}");

        if (goDeeper)
        {
            //GetComponent<Transform>().name = PlanetsList.name.Replace(" - list", "");
        }

        foreach (PlanetData planetData in ChildrenItem)
        {
            //Debug.Log(planetData.name);
            PlanetListDropdown.AddOptions(new List<string> { $"{((objectType == "moon") ? "     " : "") }{planetData.name}" });
            CreatePlanet(planetData, goDeeper, ParentName, objectType);

        }
    }

    private void CreatePlanet(PlanetData planetData, bool goDeeper, string ParentName, string objectType)
    {
        GameObject PlanetLogic = Instantiate(PlanetLogicPrefab);
        Planet planet = PlanetLogic.GetComponentInChildren<Planet>();
        GeneratePlanets moonGenerator = planet.GeneratePlanets;

        ScaleSettings scaleSettings = PlanetLogic.GetComponentInChildren<ScaleSettings>();

        //planet.ScaleSettings = Resources.Load<ScaleSettings>("Data/Scales");

        planet.ParentStellarObject = ParentName;
        planet.ObjectType = objectType;

        PlanetLogic.transform.parent = GetComponent<Transform>();

        if (goDeeper)
        {
            PlanetLogic.transform.parent = GetComponent<Transform>();
        }
        else
        {
            //Debug.Log($"{planetData.Name} -- {moonGenerator.transform.name}");
            PlanetLogic.transform.parent = planet.transform.parent;
        }

        if(objectType == "moon")
        {
            //Destroy(planet.GetComponent<Rigidbody>());
            //planet.GetComponent<Rigidbody>().isKinematic = false;
        }

        PlanetLogic.transform.name = $"{planetData.Name} - OrbitAnchor";

        //Debug.Log($"Name: {PlanetLogic.transform.name}");
        //Debug.Log(GetMoonsList(planet, planetData));

        planet.PlanetData = planetData;
        

        planet.OnCreation();

        if (goDeeper)
        {
        }

        else
        {
            moonGenerator.enabled = false;
            Destroy(moonGenerator.gameObject);
        }

        /*moonGenerator.GalaxyName = GalaxyName;
        moonGenerator.ClusterName = ClusterName;
        
        string MoonsListPath = GetMoonsListPath(planet, planetData);
        moonGenerator.PlanetsList = Resources.Load<PlanetsList>(MoonsListPath);*/

        //Debug.Log(moonGenerator.PlanetsList);
        //if (moonGenerator.PlanetsList != null && moonGenerator.PlanetsList.name.Contains(planetData.name))
        //{
            moonGenerator.LoopPlanetsList(false, planet.PlanetData.ChildrenItem, planet.PlanetData.Name, "moon");
        //}
    }

    private string GetMoonsListPath(Planet planet, PlanetData planetData)
    {
        string StellarSystemName = name;
        string PlanetName = planetData.Name;
        string Path = $"Data/Galaxies/{GalaxyName}/{ClusterName}/{StellarSystemName}/{PlanetName}/{PlanetName} - list";
        //Debug.Log(Path);
        return Path;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
