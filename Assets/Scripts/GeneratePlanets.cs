using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneratePlanets : MonoBehaviour
{
    [SerializeField]
    private PlanetsList _planetsList;

    [SerializeField]
    private GameObject PlanetLogicPrefab;

    [SerializeField]
    private bool _goDeeper;

    private Transform _transform;

    private TMP_Dropdown PlanetListDropdown;

    public PlanetsList PlanetsList { get => _planetsList; set => _planetsList = value; }
    public bool GoDeeper { get => _goDeeper; set => _goDeeper = value; }

    private void Awake()
    {


        PlanetListDropdown = GameObject.FindGameObjectWithTag("PlanetsList").GetComponent<TMP_Dropdown>();

        if (PlanetsList != null && GoDeeper)
        {
            LoopPlanetsList(true, PlanetsList, "", "planet");
        }
    }

    public void LoopPlanetsList(bool goDeeper, PlanetsList PlanetsList, string ParentName, string objectType)
    {
        //Debug.Log($"{name}: goDeeper is set to {goDeeper}, PlanetsList is {PlanetsList}");

        if (goDeeper)
        {
            GetComponent<Transform>().name = PlanetsList.name;
        }

        foreach (PlanetData planetData in PlanetsList.PlanetItem)
        {
            //Debug.Log(planetData.name);
            PlanetListDropdown.AddOptions(new List<string> { planetData.name });
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
            planet.GetComponent<Rigidbody>().isKinematic = false;
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
        
        string MoonsListPath = GetMoonsListPath(planet, planetData);

        moonGenerator.PlanetsList = Resources.Load<PlanetsList>(MoonsListPath);

        if (moonGenerator.PlanetsList != null && moonGenerator.PlanetsList.name.Contains(planetData.name))
        {
            moonGenerator.LoopPlanetsList(false, moonGenerator.PlanetsList, planet.PlanetData.Name, "moon");
        }
    }

    private string GetMoonsListPath(Planet planet, PlanetData planetData)
    {
        string StellarSystemName = name;
        string PlanetName = planetData.Name;
        string Path = $"Data/{StellarSystemName}/{PlanetName}/{PlanetName}-Satellites";

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
