using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyShield : MonoBehaviour
{
    [SerializeField]
    private int _shield = 100;


    [SerializeField]
    private Image energyShieldDisplay;

    private float _delayBetweenHits = 1f;

    private float _nextHitTime;

    public int Shield { get => _shield; set => _shield = value; }

    // Start is called before the first frame update
    void Start()
    {
        _nextHitTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnergyDisplay();
    }

    private void UpdateEnergyDisplay()
    {
        energyShieldDisplay.rectTransform.sizeDelta = new Vector2(
            Shield,
            energyShieldDisplay.rectTransform.sizeDelta.y
            );
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"EnergyShield OnTriggerEnter: {other.name}");


        if (Time.time >= _nextHitTime)
        {
            if (other.GetComponent<StellarObject>() != null)
            {
                ToggleShowShield("on");
                HitOnce();
            }

            if (other.name == "Rock")
            {
                ToggleShowShield("on");
                HitOnce();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"EnergyShield OnTriggerExit: {other.name}");

        ToggleShowShield("off");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"EnergyShield OnCollisionEnter: {collision.transform.name}");


        if (Time.time >= _nextHitTime)
        {
            if (collision.transform.GetComponent<StellarObject>() != null)
            {
                ToggleShowShield("on");
                HitOnce();
            }

            if (collision.transform.name == "Rock")
            {
                ToggleShowShield("on");
                HitOnce();
            }

        }

    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log($"EnergyShield OnCollisionExit: {collision.transform.name}");

        ToggleShowShield("off");
    }

    private void ToggleShowShield(string action)
    {
        switch (action)
        {
            case "on":
                foreach(MeshRenderer shield in FindObjectsOfType<MeshRenderer>()) {
                    shield.enabled = true;
                }
                break;

            case "off":
                foreach (MeshRenderer shield in FindObjectsOfType<MeshRenderer>())
                {
                    shield.enabled = false;
                }
                break;
        }
    }

    public void HitOnce()
    {

        Shield -= 10;
        _nextHitTime = Time.time + _delayBetweenHits;

    }
}
