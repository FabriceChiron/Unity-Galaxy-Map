using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    private Rigidbody rb;

    private Controller _controller;

    const float G = 66740f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
    }

    private void FixedUpdate()
    {
        Attractor[] attractors = FindObjectsOfType<Attractor>();

        foreach(Attractor attractor in attractors)
        {
            if(attractor != this && !_controller.IsPaused)
            {
                Attract(attractor);
            }
        }
    }

    void Attract (Attractor objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        if (!rbToAttract.isKinematic)
        {
            Vector3 direction = rb.position - rbToAttract.position;
            float distance = direction.magnitude;

            float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);

            Vector3 force = direction.normalized * forceMagnitude;
            //Debug.Log($"{rb.name} attracts {rbToAttract.name} by {forceMagnitude}");
            rbToAttract.AddForce(force);
        }

    }
}
