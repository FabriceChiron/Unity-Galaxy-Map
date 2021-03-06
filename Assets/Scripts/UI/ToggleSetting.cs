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

    [SerializeField]
    private ToggleStellarSystem _toggleStellarSystem;

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

        // On r?cup?re le Toggle
        Toggle = GetComponent<Toggle>();

        Toggle.isOn = (PlayerPrefs.GetInt(PrefName) != 0) ? true : false;

    }

    public void SetValue()
    {
        switch (PrefName)
        {
            case "RationalizeValues":
                PlayerPrefs.SetInt(PrefName, (Toggle.isOn) ? 1 : 0);
                Scales.RationalizeValues = Toggle.isOn;

                if(_controller != null)
                {
                    _toggleStellarSystem = _controller.LoopLists.NewStellarSystem.GetComponent<ToggleStellarSystem>();

                    _toggleStellarSystem.GetTargetScale();

                    _toggleStellarSystem.IsScaleChanging = true;
                }


                break;

            case "ScaleFactor":
                PlayerPrefs.SetInt(PrefName, (Toggle.isOn) ? 1 : 0);
                break;
        }

        if(_controller && _controller.LoopLists && _controller.LoopLists.StellarSystemGenerated)
        {
            _controller.TriggerSetScales("ToggleSetting");
        }
    }
}
