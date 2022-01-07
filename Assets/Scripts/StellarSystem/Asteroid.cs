using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private float _orbit, _posY, _scale;

    [SerializeField]
    private int _healthPoints;

    [SerializeField]
    private bool _hasPlatinum, _hasTurret;

    [SerializeField]
    private MeshRenderer _rock;

    [SerializeField]
    private ParticleSystem _explosion;

    [SerializeField]
    private GameObject _platinum, _platinumMiniMap, _turret, _turretPrefab;

    [SerializeField]
    private int _platinumQuantity;

    [SerializeField]
    private bool _flyToStarShip;

    private Transform _starship;
    public float Orbit { get => _orbit; set => _orbit = value; }
    public float PosY { get => _posY; set => _posY = value; }
    public float Scale { get => _scale; set => _scale = value; }
    public bool HasPlatinum { get => _hasPlatinum; set => _hasPlatinum = value; }
    public int HealthPoints { get => _healthPoints; set => _healthPoints = value; }
    public MeshRenderer Rock { get => _rock; set => _rock = value; }
    public ParticleSystem Explosion { get => _explosion; set => _explosion = value; }
    public GameObject Platinum { get => _platinum; set => _platinum = value; }
    public GameObject PlatinumMiniMap { get => _platinumMiniMap; set => _platinumMiniMap = value; }
    public GameObject Turret { get => _turret; set => _turret = value; }
    public int PlatinumQuantity { get => _platinumQuantity; set => _platinumQuantity = value; }
    public bool FlyToStarShip { get => _flyToStarShip; set => _flyToStarShip = value; }
    public Transform Starship { get => _starship; set => _starship = value; }
    public bool HasTurret { get => _hasTurret; set => _hasTurret = value; }

    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
/*        if (FlyToStarShip && HasPlatinum)
        {
            Starship = GameObject.FindGameObjectWithTag("Player").transform;

            Platinum.transform.position = Vector3.Lerp(Platinum.transform.position, Starship.position, Time.deltaTime * 15f);
            Platinum.transform.localScale = Vector3.Lerp(Platinum.transform.localScale, Vector3.zero, Time.deltaTime * 15f);

            if(Vector3.Distance(Platinum.transform.position, Starship.position) <= 3f)
            {
                Starship.GetComponent<StarShipCollect>().CollectPlatinum(PlatinumQuantity);

                //Destroy(Platinum);
                FlyToStarShip = false;

            }
        }*/
    }

    public void AddPlatinum()
    {
        Rock.material.EnableKeyword("_EMISSION");
        //PlatinumMiniMap.SetActive(true);
    }

    

    public void ActivateTurret()
    {
        Debug.Log($"Activating turret!");

        GameObject newTurret = Instantiate(_turretPrefab, Turret.transform);

    }

    public void Explode()
    {
        if(Rock != null)
        {
            Destroy(Rock.gameObject);
            Explosion.Play();
        }

        if(HasPlatinum && Platinum != null)
        {
            Platinum.SetActive(true);
        }

        if(HasTurret && !Turret.activeSelf)
        {
            ActivateTurret();
        }

    }
}
