using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretControl : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private int _turretStartHealth, _turretCurrentHealth;

    [SerializeField]
    private Transform _turretAim;

    [SerializeField]
    private ParticleSystem _explosion;

    [SerializeField]
    private Canvas _UI;
    [SerializeField]
    private Image healthDisplay;

    private Transform _playerBeacon;

    private ShootBlaster _shootBalster;

    private bool _attackMode;

    private bool _isDead;
    private float _timeToDestroy;

    [SerializeField]
    private AudioClip _hitSound, _explosionSound;

    private Transform Player { get => _player; set => _player = value; }
    public bool AttackMode { get => _attackMode; set => _attackMode = value; }
    
    [SerializeField]
    private ParticleSystem Explosion { get => _explosion; set => _explosion = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _turretStartHealth = 100;
        _turretCurrentHealth = _turretStartHealth;

        _audioSource = GetComponent<AudioSource>();

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

        if(AttackMode == !AttackMode)
        {
            Debug.Log("Attack mode change!");
        }

        AimAtPlayer();

        UpdateHealthDisplay();

        if (IsDead)
        {
            _timeToDestroy -= Time.deltaTime;

            //Debug.Log($"_timeToDestroy: {_timeToDestroy}");

            if (_timeToDestroy <= 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void UpdateHealthDisplay()
    {
        _UI.worldCamera = Camera.main;

        healthDisplay.rectTransform.sizeDelta = new Vector2(
            _turretCurrentHealth,
            healthDisplay.rectTransform.sizeDelta.y
            );
    }


    private void AimAtPlayer()
    {
        //Quaternion targetRotation = Quaternion.LookRotation(_playerBeacon.position - _turretAim.position);

        //_turretAim.rotation = Quaternion.Slerp(_turretAim.rotation, targetRotation, _aimingSpeed * Time.deltaTime);

        _turretAim.LookAt(_playerBeacon);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Turret CollisionEnter: {collision.transform.name}");
    }
    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log($"Turret CollisionExit: {collision.transform.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Turret TriggerEnter: {other.name}");

        BlasterShot blasterShot = other.GetComponent<BlasterShot>();
        
        if(blasterShot != null && blasterShot.Origin != transform)
        {
            Debug.Log("Blaster!");
            blasterShot.Explode();
            HitOnce();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Turret TriggerExit: {other.name}");
    }

    public void Explode()
    {
        Explosion.Play();
        IsDead = true;
        _timeToDestroy = 2.5f;

        foreach (MeshRenderer childMeshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            Destroy(childMeshRenderer.gameObject);
        }
    }

    public void HitOnce()
    {
        _turretCurrentHealth -= 10;

        if(_turretCurrentHealth <= 0)
        {
            _audioSource.PlayOneShot(_explosionSound);
            Explode();
        }
        else
        {
            _audioSource.PlayOneShot(_hitSound);
        }

    }
}
