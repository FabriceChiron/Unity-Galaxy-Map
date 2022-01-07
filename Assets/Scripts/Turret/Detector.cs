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


        if(other.name == "Asteroid")
        {
            Debug.Log("Asteroid detected");
            Asteroid asteroid = other.GetComponentInParent<Asteroid>();

            if (asteroid != null && asteroid.HasTurret)
            {
                Debug.Log($"asteroid should explode and spawn turret");
                asteroid.Explode();
                asteroid.ActivateTurret();
            }
        }

        if(other.name == "SpaceTurret")
        {
            Debug.Log("Turret detected");
            other.GetComponent<TurretControl>().AttackMode = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "SpaceTurret")
        {
            other.GetComponent<TurretControl>().AttackMode = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter: Detected {collision.transform.name}!");
    }
}
