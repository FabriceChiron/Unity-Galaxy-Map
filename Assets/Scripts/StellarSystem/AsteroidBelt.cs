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
    private Scales scalesStarship;

    private Scales _currentScales;

    [SerializeField]
    private GameObject _asteroidPrefab;

    [SerializeField]
    private GameObject _icyAsteroidPrefab;

    [SerializeField]
    private Controller _controller;
    
    [SerializeField]
    private LoopLists _loopLists;

    [SerializeField]
    private Texture _icyTexture;

    [SerializeField]
    private Material[] _materials;

    private List<Transform> _asteroidList;

    private float _orbitSize, _revolutionTime, _posY;

    private int _asteroidCount;

    [SerializeField]
    private int _asteroidsWithPlatinum, _asteroidsWithTurrets;

    public AsteroidBeltData AsteroidBeltData { get => _asteroidBeltData; set => _asteroidBeltData = value; }
    public GameObject AsteroidPrefab { get => _asteroidPrefab; set => _asteroidPrefab = value; }
    public GameObject IcyAsteroidPrefab { get => _icyAsteroidPrefab; set => _icyAsteroidPrefab = value; }
    public Controller Controller { get => _controller; set => _controller = value; }
    public LoopLists LoopLists { get => _loopLists; set => _loopLists = value; }
    public float OrbitSize { get => _orbitSize; set => _orbitSize = value; }
    public float RevolutionTime { get => _revolutionTime; set => _revolutionTime = value; }
    public float PosY { get => _posY; set => _posY = value; }
    public Scales CurrentScales { get => _currentScales; set => _currentScales = value; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentScales = Controller.HasPlayer ? scalesStarship : scales;

        _asteroidList = new List<Transform>();

        for (int i = 0; i < AsteroidBeltData.Quantity; i++)
        {
            GenerateAsteroid(i, AsteroidBeltData.Quantity);
        }

        if (Controller.HasPlayer)
        {
            InsertPlatinum();
            InsertTurrets();
        }

/*        if(_asteroidCount == _asteroidList.Count)
        {
            SetScales();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (!Controller.IsPaused)
        {
            AsteroidsRevolution();
        }
    }

    private void InsertPlatinum()
    {
        _asteroidsWithPlatinum = AsteroidBeltData.AsteroidsWithPlatinum;

        //Debug.Log($"_asteroidList: {_asteroidList}");

        for(int i = 0; i < _asteroidsWithPlatinum; i++)
        {
            int targetAsteroidIndex = Random.Range(0, _asteroidList.Count - 1);

            //Debug.Log($"targetAsteroidIndex: {targetAsteroidIndex}");

            Asteroid targetAsteroid = _asteroidList[targetAsteroidIndex].GetComponent<Asteroid>();
            targetAsteroid.HasPlatinum = true;
            targetAsteroid.AddPlatinum();
        }
    }

    private void InsertTurrets()
    {
        _asteroidsWithTurrets = AsteroidBeltData.AsteroidsWithTurrets;
            
        for(int i = 0; i < _asteroidsWithTurrets; i++)
        {
            int targetAsteroidIndex = Random.Range(0, _asteroidList.Count - 1);

            Asteroid targetAsteroid = _asteroidList[targetAsteroidIndex].GetComponent<Asteroid>();

            targetAsteroid.HasTurret = true;
        }
    }

    private void GenerateAsteroid(int i, int arrayLength)
    {
        GameObject asteroid = Instantiate((AsteroidBeltData.asteroidType == AsteroidType.Icy ? IcyAsteroidPrefab : AsteroidPrefab), transform.GetChild(0));

        Transform asteroidBody = asteroid.GetComponentInChildren<MeshRenderer>().transform;

        Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
        asteroidScript.Orbit = Random.Range(AsteroidBeltData.OrbitMin, AsteroidBeltData.OrbitMax);
        asteroidScript.PosY = Random.Range(AsteroidBeltData.HeightMin, AsteroidBeltData.HeightMax);
        asteroidScript.Scale = Random.Range(0.1f, 0.6f);

        asteroid.transform.rotation = Quaternion.Euler(0f, Controller.GetOrbitOrientationStart(i, arrayLength), 0f);

        asteroidBody.rotation = Random.rotation;


        _asteroidList.Add(asteroid.transform);

        _asteroidCount++;
        LoopLists.AsteroidCount ++;
    }

    private void AsteroidsRevolution()
    {
        RevolutionTime = Mathf.Max(AsteroidBeltData.YearLength, 0.5f) * CurrentScales.Year;

        Controller.RotateObject(transform.GetChild(0), RevolutionTime, false);
    }

    public void SetScales()
    {
        //Debug.Log(AsteroidBeltData.Orbit * LoopLists.dimRet(CurrentScales.Orbit, 3.5f, CurrentScales.RationalizeValues) * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? LoopLists.StellarSystemData.ScaleFactor : 1f) + LoopLists.NewStar.transform.localScale.z);

        foreach(Transform asteroid in _asteroidList)
        {
            SetOrbitSize(asteroid);
    
            asteroid.GetChild(0).localPosition = new Vector3(0f, PosY, OrbitSize);

            Transform asteroidBody = asteroid.GetComponentInChildren<MeshRenderer>().transform;

            Transform asteroidExplosion = asteroid.GetComponentInChildren<ParticleSystem>().transform;

            Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();

            float asteroidBaseScale = asteroidScript.Scale;
            
            asteroidBody.localScale = new Vector3(asteroidBaseScale * CurrentScales.Planet, asteroidBaseScale * CurrentScales.Planet, asteroidBaseScale * CurrentScales.Planet);

            asteroidExplosion.localScale = asteroidBody.localScale;

            asteroidBody.localPosition = new Vector3(asteroidBody.localScale.x * -.5f, asteroidBody.localScale.y * -.5f, asteroidBody.localScale.z * -.5f);

            asteroidScript.Explosion.gameObject.transform.localScale = asteroidBody.localScale;
            if (asteroidScript.HasPlatinum)
            {
                asteroidScript.Platinum.transform.localScale = asteroidBody.localScale * 50f;
                asteroidScript.PlatinumQuantity = Mathf.Max(Mathf.RoundToInt(asteroidBaseScale * 10), 1);
            }
            //asteroidBody.localPosition = new Vector3(0f, 0f, Random.Range(CurrentScales.Orbit * -.5f, CurrentScales.Orbit * .5f));
        }
    }

    private void SetOrbitSize(Transform asteroid)
    {
        //Calculate the size of the orbit, based on its real orbit size, the scale factor (if set), and if values are rationalized or not
        OrbitSize = asteroid.GetComponent<Asteroid>().Orbit * LoopLists.dimRet(CurrentScales.Orbit, 3.5f, CurrentScales.RationalizeValues) * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? LoopLists.StellarSystemData.ScaleFactor : 1f) + LoopLists.NewStar.transform.localScale.z;

        PosY = asteroid.GetComponent<Asteroid>().PosY * CurrentScales.Planet;

        

        //OrbitSize = AsteroidBeltData.Orbit * LoopLists.dimRet(CurrentScales.Orbit, 3.5f, CurrentScales.RationalizeValues) * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? LoopLists.StellarSystemData.ScaleFactor : 1f) + LoopLists.NewStar.transform.localScale.z;

    }

}
