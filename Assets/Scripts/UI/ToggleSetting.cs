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
    private string PrefName { get => _prefName; set => _prefName = value; }

    public Scales Scales { get => scales; set => scales = value; }
    public Toggle Toggle { get => _toggle; set => _toggle = value; }

    private void Awake()
    {
        Toggle = GetComponent<Toggle>();
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
            PlayerPrefs.SetInt(PrefName, Scales.RationalizeValues ? 1 : 0);
        }

        // On récupère le Toggle
        _toggle = GetComponent<Toggle>();

        _toggle.isOn = (PlayerPrefs.GetInt(PrefName) != 0) ? true : false;

    }

    public void SetValue()
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

        if(_controller.LoopLists && _controller.LoopLists.StellarSystemGenerated)
        {
            _controller.SetScales();
        }
    }

    public void Rescale()
    {
    }
}
