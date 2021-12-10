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

    private Controller _controller;

    public Scales Scales { get => scales; set => scales = value; }
    private string PrefName { get => _prefName; set => _prefName = value; }

    private void Awake()
    {
        _controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
    }

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
        if (!PlayerPrefs.HasKey(PrefName))
        {
            PlayerPrefs.SetInt(PrefName, 0);
        }

        // On récupère le Toggle
        _toggle = GetComponent<Toggle>();

        _toggle.isOn = (PlayerPrefs.GetInt(PrefName) != 0) ? true : false;

        _toggle.onValueChanged.AddListener(delegate
        {
            switch (PrefName)
            {
                case "RationalizeValues":
                    PlayerPrefs.SetInt(PrefName, (_toggle.isOn) ? 1 : 0);
                    Scales.RationalizeValues = _toggle.isOn;
                    break;

                case "ScaleFactor":
                    PlayerPrefs.SetInt(PrefName, (_toggle.isOn) ? 1 : 0);
                    break;
            }

            Rescale();
        });
    }

    public void Rescale()
    {
        _controller.SetScales();
    }
}
