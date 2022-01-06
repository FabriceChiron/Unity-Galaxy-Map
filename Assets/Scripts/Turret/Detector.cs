using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]
    private SphereCollider _detector;

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

        Asteroid asteroid = other.GetComponent<Asteroid>();
        Debug.Log($"OnTriggerEnter: Detected armed {other.name}! Asteroid is {!!asteroid}");

        if(asteroid != null && asteroid.HasTurret)
        {
            asteroid.Explode();
            asteroid.ActivateTurret();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter: Detected {collision.transform.name}!");
    }
}
