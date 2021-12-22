using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShipCollect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int _platinumScore = 0;

    private bool _flyToStarShip;

    public int PlatinumScore { get => _platinumScore; set => _platinumScore = value; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Platinum")
        {
            other.GetComponentInParent<Asteroid>().FlyToStarShip = true;
        }

        

    }

    public void FlyToStarShip()
    {

    }

    public void CollectPlatinum(int quantity)
    {
        _platinumScore += quantity;
    }
}
