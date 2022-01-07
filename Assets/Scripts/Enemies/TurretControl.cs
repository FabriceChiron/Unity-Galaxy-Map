using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private int _turretStartHealth;

    [SerializeField]
    private Transform _turretAim;

    [SerializeField]
    private float _aimingSpeed = 5f;

    private int _turretCurrentHealth;
    private Transform _playerBeacon;

    private ShootBlaster _shootBalster;

    private bool _attackMode;

    private Transform Player { get => _player; set => _player = value; }
    public bool AttackMode { get => _attackMode; set => _attackMode = value; }

    // Start is called before the first frame update
    void Start()
    {
        _turretStartHealth = 100;
    }

    private void OnEnable()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerBeacon = Player.GetComponent<SC_SpaceshipController>().TurretAnchor;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_playerBeacon);

        AimAtPlayer();
    }

    private void AimAtPlayer()
    {
        //Quaternion targetRotation = Quaternion.LookRotation(_playerBeacon.position - _turretAim.position);

        //_turretAim.rotation = Quaternion.Slerp(_turretAim.rotation, targetRotation, _aimingSpeed * Time.deltaTime);

        _turretAim.LookAt(_playerBeacon);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Turret CollisionEnter: {collision.transform.name}");
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log($"Turret CollisionExit: {collision.transform.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Turret TriggerEnter: {other.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Turret TriggerExit: {other.name}");
    }

}
