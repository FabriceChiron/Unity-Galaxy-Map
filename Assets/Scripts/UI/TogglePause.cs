using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TogglePause : MonoBehaviour
{
    [SerializeField]
    private UITest _uiTest;

    [SerializeField]
    private Text _label;

    [SerializeField]
    private Controller _controller;

    private Toggle _toggle;

    public UITest UITest { get => _uiTest; set => _uiTest = value; }
    public Toggle Toggle { get => _toggle; set => _toggle = value; }
    public Text Label { get => _label; set => _label = value; }

    private void Awake()
    {
        Toggle = GetComponent<Toggle>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //SetTogglePause();
    }

    public void SetTogglePause()
    {
        _controller.IsPaused = !_controller.IsPaused;
        Toggle.isOn = _controller.IsPaused;
        Label.text = (Toggle.isOn ? "Ø" : "Û");
    }
}
