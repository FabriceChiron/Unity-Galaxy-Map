using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchorCollider : MonoBehaviour
{

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger with {other.name}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision with {collision.transform.name}");
    }
}
