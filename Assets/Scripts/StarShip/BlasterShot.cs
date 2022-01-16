using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterShot : MonoBehaviour
{
    [SerializeField]
    private float _blasterSpeed, _durationBeforeDestroy;
    private float _destroyTimer;

    [SerializeField]
    private int _damage = 25;

    private Transform _transform;
    private Rigidbody _rigidbody;

    [SerializeField]
    private Transform _origin;

    [SerializeField]
    private ParticleSystem _explosion;

    private AudioSource _audioSource;

    public Transform Origin { get => _origin; set => _origin = value; }
    public ParticleSystem Explosion { get => _explosion; set => _explosion = value; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _destroyTimer -= Time.deltaTime;

        if(_destroyTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        Vector3 velocity = _transform.forward * _blasterSpeed;

        var movementStep = velocity * Time.deltaTime;

        var newPos = _transform.position + movementStep;

        _rigidbody.MovePosition(newPos);
    }

    public void Shoot(float speed)
    {
        _blasterSpeed = speed;
        _audioSource.PlayOneShot(_audioSource.clip);
        _destroyTimer = _durationBeforeDestroy;
    }

    public void Explode()
    {
        Explosion.Play();
        _destroyTimer = Explosion.main.duration;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"{transform.name} origin: {Origin}, {collision.transform.IsChildOf(Origin)}");

        if (!collision.transform.IsChildOf(Origin))
        {
            Debug.Log($"{Origin.name} collision: {collision.transform.name}");

            if(collision.transform.name == "Rock")
            {
                Explode();
                //Asteroid asteroid = collision.transform.parent.GetComponent<Asteroid>();
                Asteroid asteroid = collision.transform.GetComponentInParent<Asteroid>();

                asteroid.HealthPoints -= _damage;

                if(asteroid.HealthPoints <= 0)
                {
                    asteroid.Explode();
                }
            }

            
        }
    }
}
