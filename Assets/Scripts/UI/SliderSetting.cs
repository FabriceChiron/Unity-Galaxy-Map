using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderSetting : MonoBehaviour
{

    //On déclare la variable qui contient le nom du paramètre PlayerPref à récupérer/modifier
    [SerializeField]
    protected string _prefName;

    [SerializeField]
    private Scales scales;

    [SerializeField]
    private TextMeshProUGUI _label;

    [SerializeField]
    private TextMeshProUGUI _displayValue;

    private Planet[] _planets;

    // On déclare la variable qui contiendra le composant Slider
    protected Slider _slider;

    private float scaleItem;

    public float ScaleItem { get => scaleItem; set => scaleItem = value; }
    public Scales Scales { get => scales; set => scales = value; }

    private void Awake()
    {
    }


    public virtual void LoadPrefs()
    {
        // On récupère le Slider
        _slider = GetComponent<Slider>();

        /*if(_slider.value == 0)
        {
            Debug.Log($"{_prefName}: {ScaleItem}");
            SetValue(ScaleItem);
        }*/

        InitValueFromScales(_prefName);

        _label.text = _prefName;

        //Debug.Log($"Getting PlayerPrefs {_prefName} and assigning its value to {_slider.name}");
        //Debug.Log($"{_prefName}: {PlayerPrefs.GetFloat(_prefName)}");
        // On défini la valeur du composant Slider grâce aux PlayerPrefs
        _slider.value = PlayerPrefs.GetFloat(_prefName);

    }

    public float InitValueFromScales(string prefName)
    {
        float value;
        switch (prefName)
        {
            case "Orbit":
                value = Scales.Orbit;
                break;
            case "Planet Radius":
                value = Scales.Planet;
                break;
            case "Year":
                value = Scales.Year;
                break;
            case "Day":
                value = Scales.Day;
                break;
            default:
                Debug.LogError($"{_prefName} does not exist");
                value = 1f;
                break;
        }

        PlayerPrefs.SetFloat(prefName, value);

        return value;
    }

    public virtual void SetValue(float value)
    {
        AssignValueToScales(_prefName, value);

        //Debug.Log($"Setting {_prefName} value to {value}");

        // On sauvegarde la nouvelle valeur dans les PlayerPrefs
        PlayerPrefs.SetFloat(_prefName, value);

        _planets = GameObject.FindGameObjectWithTag("StellarSystem").GetComponentsInChildren<Planet>();

        for(int i = 0; i < _planets.Length; i++)
        {
            Debug.Log($"{_planets.Length} - {i} - {_planets[i].name}");
        }

        foreach (Planet planet in _planets)
        {
            Debug.Log(planet.name);
            if(planet.IsCreated)
            {
                planet.SetScales("slider");
            }
        }
        
    }

    public void AssignValueToScales(string prefName, float value)
    {
        switch (prefName)
        {
            case "Orbit":
                if(value == 0)
                {
                    value = Scales.Orbit;
                }
                Scales.Orbit = value;
                break;
            case "Planet Radius":
                if (value == 0)
                {
                    value = Scales.Planet;
                }
                Scales.Planet = value;
                break;
            case "Year":
                if (value == 0)
                {
                    value = Scales.Year;
                }
                Scales.Year = value;
                break;
            case "Day":
                if (value == 0)
                {
                    value = Scales.Day;
                }
                Scales.Day = value;
                break;
        }

        _slider.value = value;
        _displayValue.text = $"{value}";
    }

/*    public float GetValueFromScales(string prefName)
    {
        switch (_prefName)
        {
            case "Orbit":
                return scales.Orbit;
                break;
            case "Planet Radius":
                return scales.Planet;
                break;
            case "Year":
                return scales.Year;
                break;
            case "Day":
                scales.Day = value;
                break;
        }
    }*/
}
