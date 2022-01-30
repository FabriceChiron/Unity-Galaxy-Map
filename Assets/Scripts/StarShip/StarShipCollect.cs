using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarShipCollect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int _platinumScore = 0;

    [SerializeField]
    private StarShipSetup _starShipSetup;

    [SerializeField]
    private Memory _memory;

    [SerializeField]
    private GameObject _lidarPrefab;

    [SerializeField]
    private Transform _lidarAnchor;

    [SerializeField] 
    private float _delayBetweenLidars;

    [SerializeField]
    private TextMesh _platinumGauge;

    [SerializeField]
    private TextMeshProUGUI _platinumObjective, _turretsObjective;

    [SerializeField]
    private bool _collectingHydrogen;

    private float _nextLidarTime;

    private bool _flyToStarShip;
    private bool _isPatinumCollected;
    private bool _isLidarFired;

    public int PlatinumScore { get => _platinumScore; set => _platinumScore = value; }
    public StarShipSetup StarShipSetup { get => _starShipSetup; set => _starShipSetup = value; }

    private GameObject _platinum;

    void Start()
    {
        _nextLidarTime = Time.time;

        UpdateObjectives();
    }

    private void UpdateObjectives()
    {
        _platinumObjective.text = $"<color=#f00>{_platinumScore}</color>/{_memory.SavedData.SavedStellarSystem.Item.Platinum} <size=75%>Platinum</size>";
        _turretsObjective.text = $"<color=#f00>{_memory.SavedData.Turrets}</color>/{_memory.SavedData.SavedStellarSystem.Item.Turrets} <size=75%>Turrets</size>";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObjectives();
        /*        if (Time.time >= _nextLidarTime)
                {
                    if (Input.GetButtonDown("Lidar"))
                    {
                        FireLidar();
                        _nextLidarTime = Time.time + _delayBetweenLidars;
                    }
                }*/

        if (_isLidarFired)
        {

        }

        if (_platinum)
        {
            _platinum.transform.position = Vector3.Lerp(_platinum.transform.position, transform.position, Time.deltaTime * 15f);
            _platinum.transform.localScale = Vector3.Lerp(_platinum.transform.localScale, Vector3.zero, Time.deltaTime * 15f);

            if(Vector3.Distance(transform.position, _platinum.transform.position) < 5f)
            {
                _isPatinumCollected = false;
                CollectPlatinum(_platinum.GetComponentInParent<Asteroid>().PlatinumQuantity);
            }
        }
    }

    private void FixedUpdate()
    {
        CollectHydrogen();
    }

    private void FireLidar()
    {
        Debug.Log("LIDAR!");
        _isLidarFired = true;

        GameObject newLidar = Instantiate(_lidarPrefab, _lidarAnchor.position, _lidarAnchor.rotation);

/*        Lidar lidar = newLidar.GetComponent<Lidar>();

        lidar.Fire();*/
    }

    private void CollectHydrogen()
    {
        if (_collectingHydrogen)
        {
            StarShipSetup.Hydrogen += Time.deltaTime * 5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Platinum")
        {
            //Debug.Log($"There's platinum here!");
            //other.GetComponentInParent<Asteroid>().FlyToStarShip = true;
            _platinum = other.GetComponentInParent<Asteroid>().Platinum;
        }

        if(other.tag == "Gas")
        {
            Debug.Log("Entering gas layer!");
            _collectingHydrogen = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gas")
        {
            Debug.Log("Leaving gas layer!");
            _collectingHydrogen = false;
        }
    }

    public void CollectPlatinum(int quantity)
    {
        if (!_isPatinumCollected)
        {
            PlatinumScore += quantity;
            _memory.SavedData.Platinum += PlatinumScore;
            _platinumGauge.text = PlatinumScore.ToString();
            _isPatinumCollected = true;
            Destroy(_platinum);
            //_platinum = null;
        }
    }

}
