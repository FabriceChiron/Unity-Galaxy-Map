using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    private Transform _playerBeacon;

    public Transform Player { get => _player; set => _player = value; }

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
