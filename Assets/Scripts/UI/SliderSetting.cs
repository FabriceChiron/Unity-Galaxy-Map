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

    // On déclare la variable qui contiendra le composant Slider
    protected Slider _slider;

    private void Awake()
    {
        
    }

    public virtual void LoadPrefs()
    {
        // On récupère le Slider
        _slider = GetComponent<Slider>();

        _label.text = _prefName;



        Debug.Log($"Getting PlayerPrefs {_prefName} and assigning its value to {_slider.name}");
        //Debug.Log($"{_prefName}: {PlayerPrefs.GetFloat(_prefName)}");
        // On défini la valeur du composant Slider grâce aux PlayerPrefs
        _slider.value = PlayerPrefs.GetFloat(_prefName);

        _displayValue.text = $"{_slider.value}";
    }

    public virtual void SetValue(float value)
    {
        Debug.Log($"Setting {_prefName} value to {value}");

        // On sauvegarde la nouvelle valeur dans les PlayerPrefs
        PlayerPrefs.SetFloat(_prefName, value);
    }
}
