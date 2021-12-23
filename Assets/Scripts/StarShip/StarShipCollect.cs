using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShipCollect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int _platinumScore = 0;

    [SerializeField]
    private GameObject _lidarPrefab;

    [SerializeField]
    private Transform _lidarAnchor;

    [SerializeField] 
    private float _delayBetweenLidars;

    private float _nextLidarTime;

    private bool _flyToStarShip;
    private bool _isPatinumCollected;
    private bool _isLidarFired;

    public int PlatinumScore { get => _platinumScore; set => _platinumScore = value; }

    void Start()
    {
        _nextLidarTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= _nextLidarTime)
        {
            if (Input.GetButtonDown("Lidar"))
            {
                FireLidar();
                _nextLidarTime = Time.time + _delayBetweenLidars;
            }
        }

        if (_isLidarFired)
        {

        }
    }

    private void FireLidar()
    {
        Debug.Log("LIDAR!");
        _isLidarFired = true;

        GameObject newLidar = Instantiate(_lidarPrefab, _lidarAnchor.position, _lidarAnchor.rotation);

/*        Lidar lidar = newLidar.GetComponent<Lidar>();

        lidar.Fire();*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Platinum")
        {
            Debug.Log($"There's platinum here!");
            other.GetComponentInParent<Asteroid>().FlyToStarShip = true;
        }

    }

    public void FlyToStarShip()
    {

    }

    public void CollectPlatinum(int quantity)
    {
        if (!_isPatinumCollected)
        {
            PlatinumScore += quantity;
            Debug.Log(PlatinumScore);
            _isPatinumCollected = true;
        }
    }
}
