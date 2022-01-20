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

    [SerializeField]
    private MainMenuNav _mainMenuNav;

    private Button _button;
    private Toggle _toggle;

    public GameObject UIElement { get => _UIElement; set => _UIElement = value; }
    private GameObject _UIElementParent;
    public Transform VROverlayUI { get => _VROverlayUI; set => _VROverlayUI = value; }

    [SerializeField]
    private Transform elementCenter;
    private bool _isSquare;

    [SerializeField]
    private bool _doNotMove;

    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private bool _isInteractable;


    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();

        elementCenter = UIElement.transform.Find("Center");
        _isSquare = UIElement.GetComponent<RectTransform>().sizeDelta.x == UIElement.GetComponent<RectTransform>().sizeDelta.y;

        _toggle = UIElement.GetComponent<Toggle>();
        _button = UIElement.GetComponent<Button>();
        _UIElementParent = UIElement.transform.parent.gameObject;

        _mainMenuNav = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenuNav>();
    }

    // Update is called once per frame
    void Update()
    {
        _isInteractable = (_button != null) ? _button.IsInteractable() : (_toggle != null) ? _toggle.IsInteractable() : false;

/*        Debug.Log(UIElement.transform.parent.gameObject.activeSelf);
        Debug.Log(_isInteractable);*/

        if (_isInteractable)
        {
            ActivateOverlay(true);

            if (!_doNotMove)
            {
                transform.position = new Vector3(
                    elementCenter.position.x,
                    elementCenter.position.y,
                    VROverlayUI.transform.position.z + (_isSquare ? -0.02f : -0.01f)
                );
            }
        }
        else
        {
            ActivateOverlay(false);
        }
    }

    private void ActivateOverlay(bool isEnabled)
    {
        _meshCollider.enabled = isEnabled;
    }

    public void TriggerUIElement()
    {

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
