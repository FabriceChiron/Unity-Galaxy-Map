using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinkGameObjectToUIElement : MonoBehaviour
{
    [SerializeField]
    private GameObject _UIElement;

    [SerializeField]
    private Transform _VROverlayUI;

    private Button _button;
    private Toggle _toggle;

    public GameObject UIElement { get => _UIElement; set => _UIElement = value; }
    public Transform VROverlayUI { get => _VROverlayUI; set => _VROverlayUI = value; }

    private Transform elementCenter;
    private bool _isSquare;

    // Start is called before the first frame update
    void Start()
    {
        elementCenter = UIElement.transform.Find("Center");
        _isSquare = UIElement.GetComponent<RectTransform>().sizeDelta.x == UIElement.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(
            elementCenter.position.x,
            elementCenter.position.y - VROverlayUI.transform.position.y,
            _isSquare ? -0.02f : -0.01f
        ); ;
    }

    public void TriggerUIElement()
    {
        _toggle = UIElement.GetComponent<Toggle>();
        _button = UIElement.GetComponent<Button>();
        if(_toggle != null)
        {
            _toggle.isOn = !_toggle.isOn;
        }
        if(_button != null && _button.GetComponent<Image>().color.a <= 0.2f)
        {
            _button.onClick.Invoke();
        }
    }
}
