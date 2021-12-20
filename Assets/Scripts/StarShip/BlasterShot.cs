using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterShot : MonoBehaviour
{
    [SerializeField]
    private float _blasterSpeed;
    private Transform _transform;
    private Rigidbody _rigidbody;

    private AudioSource _audioSource;

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
    }
}
