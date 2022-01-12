using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]
    private SphereCollider _detector;

    private ToggleStellarSystem _toggleStellarSystem;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _toggleStellarSystem = GameObject.FindObjectOfType<ToggleStellarSystem>();
        _toggleStellarSystem.Detector = this; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        DetectTurrets(transform.position, _detector.radius);
    }

    private void OnTriggerEnter(Collider other)
    {

        _toggleStellarSystem = GameObject.FindObjectOfType<ToggleStellarSystem>();

        if (_toggleStellarSystem != null && !_toggleStellarSystem.IsScaleChanging)
        {
            if(other.name == "Asteroid")
            {
                //Debug.Log("Asteroid detected");
                Asteroid asteroid = other.GetComponentInParent<Asteroid>();

                if (asteroid != null && asteroid.HasTurret)
                {
                    //Debug.Log($"asteroid should explode and spawn turret");
                    asteroid.Explode();
                    asteroid.ActivateTurret();
                }
            }

            /*if(other.name == "SpaceTurret")
            {
                Debug.Log("Turret detected");
                other.GetComponent<TurretControl>().AttackMode = true;
            }*/
        }


    }

    private void DetectTurrets(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.name == "Asteroid" || hitCollider.name == "SpaceTurret")
            {
                Debug.Log(hitCollider.name);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name == "SpaceTurret")
        {
            other.GetComponent<TurretControl>().AttackMode = false;
            Debug.Log("SpaceTurret is leaving attack mode");
        }
    }
}
