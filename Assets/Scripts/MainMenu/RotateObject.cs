using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    [SerializeField]
    private float _speedX, _speedY, _speedZ;


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
        transform.Rotate(new Vector3(
                _speedX * Time.deltaTime,
                _speedY * Time.deltaTime,
                _speedZ * Time.deltaTime
            )
        );
    }
}
