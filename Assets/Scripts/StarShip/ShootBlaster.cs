using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBlaster : MonoBehaviour
{
    [SerializeField] private GameObject _blasterPrefab;
    [SerializeField] private Transform[] _blasters;

    [SerializeField] private float _blasterSpeed = 50f;
    [SerializeField] private float _delayBetweenShots;

    [SerializeField]
    private Controller _controller;

    [Header("Enemies")]
    [SerializeField] private bool _isEnemy;
    [SerializeField]
    private TurretControl _turretControl;
    [SerializeField] private float _delayBetweenSalvoes;
    [SerializeField] private float _salvoDuration = 1f;
    private float _currentSalvoDuration;

    //[SerializeField] private float _destroyTime = 3f;

    [Header("Starship")]
    [SerializeField]
    private SC_SpaceshipController _starShipController;
    [SerializeField]
    private PlayerInput _playerInput;


    private int blasterIndex = 0;

    private float _nextShotTime;
    private float _nextSalvoTime;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();

        _nextShotTime = Time.time;

        if (_isEnemy)
        {
            _nextSalvoTime = Time.time;
            _currentSalvoDuration = _salvoDuration;
        }
    }

    private void FireBlaster()
    {
        if (!_controller.IsPaused)
        {
            if(blasterIndex >= _blasters.Length)
            {
                blasterIndex = 0;
            }
            //Debug.Log($"blasterIndex: {blasterIndex}, _blasters[{blasterIndex}].position: {_blasters[blasterIndex].position}");

            if(_blasters[blasterIndex] != null)
            {
                GameObject newBlasterShot = Instantiate(_blasterPrefab, _blasters[blasterIndex].position, _blasters[blasterIndex].rotation);

                BlasterShot blasterShot = newBlasterShot.GetComponent<BlasterShot>();
                blasterShot.Origin = transform;

                blasterShot.Shoot(_blasterSpeed);

                blasterIndex++;
                //Destroy(bullet.gameObject, _destroyTime);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        //For the turrets
        //If turret is in attack mode
        if (_isEnemy && _turretControl.AttackMode)
        {
            if(Time.time >= _nextSalvoTime && _turretControl.AttackMode)
            {

                _currentSalvoDuration -= Time.deltaTime;

                if (Time.time >= _nextShotTime && _currentSalvoDuration >= 0f)
                {
                    FireBlaster();
                    _nextShotTime = Time.time + _delayBetweenShots;
                    
                }

                if(_currentSalvoDuration <= 0f)
                {
                    _nextSalvoTime = Time.time + _delayBetweenSalvoes;
                    _currentSalvoDuration = _salvoDuration;
                }

            }
        }

        //For the Starship
        else
        {
            if (Time.time >= _nextShotTime)
            {
                if (_playerInput.FireAxis != 0 && _starShipController != null && !_starShipController.IsWarping)
                {
                    FireBlaster();
                    _nextShotTime = Time.time + _delayBetweenShots;
                }

            }
        }
    }

    private void FixedUpdate()
    {
        
    }
}
