using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBelt : MonoBehaviour
{

    [SerializeField]
    private AsteroidBeltData _asteroidBeltData;

    [SerializeField]
    private Scales scales;

    [SerializeField]
    private GameObject _asteroidPrefab;

    [SerializeField]
    private Controller _controller;
    
    [SerializeField]
    private LoopLists _loopLists;

    [SerializeField]
    private Texture _icyTexture;

    private List<Transform> _asteroidList;

    private float _orbitSize, _revolutionTime;

    public AsteroidBeltData AsteroidBeltData { get => _asteroidBeltData; set => _asteroidBeltData = value; }
    public GameObject AsteroidPrefab { get => _asteroidPrefab; set => _asteroidPrefab = value; }
    public Controller Controller { get => _controller; set => _controller = value; }
    public LoopLists LoopLists { get => _loopLists; set => _loopLists = value; }
    public float OrbitSize { get => _orbitSize; set => _orbitSize = value; }
    public float RevolutionTime { get => _revolutionTime; set => _revolutionTime = value; }

    // Start is called before the first frame update
    void Start()
    {
        _asteroidList = new List<Transform>();

        for (int i = 0; i < AsteroidBeltData.Quantity; i++)
        {
            GenerateAsteroid(i, AsteroidBeltData.Quantity);
        }

        SetScales();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Controller.IsPaused)
        {
            AsteroidsRevolution();
        }
    }

    private void GenerateAsteroid(int i, int arrayLength)
    {
        GameObject asteroid = Instantiate(AsteroidPrefab, transform.GetChild(0));

        Transform asteroidBody = asteroid.GetComponentInChildren<MeshRenderer>().transform;

        asteroid.transform.rotation = Quaternion.Euler(0f, Controller.GetOrbitOrientationStart(i, arrayLength), 0f);

        asteroidBody.rotation = Random.rotation;


        //SetMaterial();

        _asteroidList.Add(asteroid.transform);
    }

    private void AsteroidsRevolution()
    {
        RevolutionTime = Mathf.Max(AsteroidBeltData.YearLength, 0.5f) * scales.Year;

        Controller.RotateObject(transform.GetChild(0), RevolutionTime, false);
    }

    public void SetScales()
    {
        //Debug.Log(AsteroidBeltData.Orbit * LoopLists.dimRet(scales.Orbit, 3.5f, scales.RationalizeValues) * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? LoopLists.StellarSystemData.ScaleFactor : 1f) + LoopLists.NewStar.transform.localScale.z);

        SetOrbitSize();
        
        foreach(Transform asteroid in _asteroidList)
        {
            asteroid.GetChild(0).localPosition = new Vector3(0f, 0f, OrbitSize);

            Transform asteroidBody = asteroid.GetComponentInChildren<MeshRenderer>().transform;
            asteroidBody.localPosition = new Vector3(0f, 0f, Random.Range(scales.Orbit * -.5f, scales.Orbit * .5f));
        }
    }

    private void SetOrbitSize()
    {
        //Calculate the size of the orbit, based on its real orbit size, the scale factor (if set), and if values are rationalized or not
        OrbitSize = AsteroidBeltData.Orbit * LoopLists.dimRet(scales.Orbit, 3.5f, scales.RationalizeValues) * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? LoopLists.StellarSystemData.ScaleFactor : 1f) + LoopLists.NewStar.transform.localScale.z;

    }

    private void SetMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();

        if(AsteroidBeltData.asteroidType == AsteroidType.Icy)
        {
            renderer.material.mainTexture = _icyTexture;
        }

        //Make the texture emit light (to be visible even in the darkness of space)
        renderer.material.EnableKeyword("_EMISSION");
        renderer.material.SetTexture("_EmissionMap", renderer.material.mainTexture);
        renderer.material.SetColor("_EmissionColor", new Vector4(0.05f, 0.05f, 0.05f));
    }
}
