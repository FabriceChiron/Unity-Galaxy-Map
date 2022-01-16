using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinkGameObjectToUIElement : MonoBehaviour
{
    [SerializeField]
    private GameObject UIElement;

    private Button _button;
    private Toggle _toggle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerUIElement()
    {
        _toggle = UIElement.GetComponent<Toggle>();
        _button = UIElement.GetComponent<Button>();
        if(_toggle != null)
        {
            _toggle.isOn = !_toggle.isOn;
        }
        if(_button != null)
        {
            _button.onClick.Invoke();
        }
    }
}
