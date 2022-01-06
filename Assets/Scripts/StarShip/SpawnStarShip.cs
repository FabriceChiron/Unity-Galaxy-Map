using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStarShip : MonoBehaviour
{

    [SerializeField]
    private float _orbit;

    [SerializeField]
    private string _coords;

    [SerializeField]
    private Vector3 _position;

    [SerializeField]
    private Scales scales;

    [SerializeField]
    private Controller _controller;

    private void Awake()
    {
        _controller = GetComponent<StarShipSetup>().Controller;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaceStarShipOnScene();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlaceStarShipOnScene()
    {
        //transform.parent.localRotation = Quaternion.Euler(0f, _controller.GetOrbitOrientationStart(_coords), 0f);

        SetScales();
    }

    private void SetScales()
    {
        float OrbitSize = _orbit * _controller.LoopLists.dimRet(scales.Orbit, 3.5f, scales.RationalizeValues) * (PlayerPrefs.GetInt("ScaleFactor") != 0 ? _controller.LoopLists.StellarSystemData.ScaleFactor : 1f);

        //Debug.Log($"Starship position z: {OrbitSize}");

        transform.position = new Vector3(_position.x, _position.y, _position.z) * OrbitSize * .5f;

    }
}
