using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShipShootBlaster : MonoBehaviour
{
    [SerializeField] private GameObject _blasterPrefab;
    [SerializeField] private Transform[] _blasters;

    [SerializeField] private float _blasterSpeed = 50f;
    [SerializeField] private float _delayBetweenShots;
    //[SerializeField] private float _destroyTime = 3f;

    private int blasterIndex = 0;

    private float _nextShotTime;

    // Start is called before the first frame update
    void Start()
    {
        _nextShotTime = Time.time;
    }

    private void FireBlaster()
    {
        blasterIndex++;

        if(blasterIndex >= _blasters.Length)
        {
            blasterIndex = 0;
        }

        GameObject newBlasterShot = Instantiate(_blasterPrefab, _blasters[blasterIndex].position, _blasters[blasterIndex].rotation);

        BlasterShot blasterShot = newBlasterShot.GetComponent<BlasterShot>();

        blasterShot.Shoot(_blasterSpeed);

        //Destroy(bullet.gameObject, _destroyTime);

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= _nextShotTime)
        {
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("FireBlaster");
                FireBlaster();

                _nextShotTime = Time.time + _delayBetweenShots;

            }
        }
    }
}
