using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderSetting : MonoBehaviour
{

    //On d?clare la variable qui contient le nom du param?tre PlayerPref ? r?cup?rer/modifier
    [SerializeField]
    protected string _prefName;

    [SerializeField]
    private Scales scales;

    [SerializeField]
    private TextMeshProUGUI _label;

    [SerializeField]
    private TextMeshProUGUI _displayValue;

    [SerializeField]
    private Controller _controller;

    // On d?clare la variable qui contiendra le composant Slider
    protected Slider _slider;

    public Scales Scales { get => scales; set => scales = value; }

    private void Awake()
    {

    }


    public virtual void LoadPrefs()
    {
        _slider = GetComponent<Slider>();

        InitValueFromScales(_prefName);

        _label.text = _prefName;

        //Debug.Log($"Getting PlayerPrefs {_prefName} and assigning its value to {_slider.name}");
        //Debug.Log($"{_prefName}: {PlayerPrefs.GetFloat(_prefName)}");
        // On d?fini la valeur du composant Slider gr?ce aux PlayerPrefs
        _slider.value = PlayerPrefs.GetFloat(_prefName);

    }

    public void InitValueFromScales(string prefName)
    {
        float value;
        switch (prefName)
        {
            case "Orbit":
                value = _controller.HasPlayer ? 200f : Scales.Orbit;
                break;
            case "Planet Radius":
                value = _controller.HasPlayer ? 5f : Scales.Planet;
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

        PlayerPrefs.SetFloat(prefName, Mathf.Round(value * 4) / 4);
    }

    public virtual void SetValue(float value)
    {
        value = Mathf.Round(value * 4) / 4;

        AssignValueToScales(_prefName, value);

        PlayerPrefs.SetFloat(_prefName, value);

        if (_controller.LoopLists.StellarSystemGenerated)
        {
            _controller.TriggerSetScales("SliderSetting");
        }
    }

    public void AssignValueToScales(string prefName, float value)
    {
        value = Mathf.Round(value * 4) / 4;

        switch (prefName)
        {
            case "Orbit":
                Scales.Orbit = value;
                break;
            case "Planet Radius":
                Scales.Planet = value;
                break;
            case "Year":
                Scales.Year = value;
                break;
            case "Day":
                Scales.Day = value;
                break;
        }

        _slider.value = value;
        _displayValue.text = $"{value}";
    }
}
