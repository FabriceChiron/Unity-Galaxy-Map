using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleSetting : MonoBehaviour
{
    [SerializeField]
    private string _prefName;

    [SerializeField]
    private Scales scales;

    [SerializeField]
    private TextMeshProUGUI _label;

    private Planet[] _planets;

    private Toggle _toggle;

    public Scales Scales { get => scales; set => scales = value; }
    protected string PrefName { get => _prefName; set => _prefName = value; }

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPrefs()
    {
        // On récupère le Toggle
        _toggle = GetComponent<Toggle>();

        _toggle.isOn = (PlayerPrefs.GetInt(PrefName) != 0) ? true : false;


        _toggle.onValueChanged.AddListener(delegate
        {
            PlayerPrefs.SetInt(PrefName, (_toggle.isOn) ? 1 : 0);
            Scales.RationalizeValues = _toggle.isOn;

            Rescale();
        });
    }

    public void Rescale()
    {
        _planets = GameObject.FindGameObjectWithTag("StellarSystem").GetComponentsInChildren<Planet>();

        foreach (Planet planet in _planets)
        {
            //Debug.Log(planet.name);
            if (planet.IsCreated)
            {
                planet.SetScales("toggle");
            }
        }
    }
}
