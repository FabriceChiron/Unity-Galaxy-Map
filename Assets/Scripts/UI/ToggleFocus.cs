using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleFocus : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle;

    [SerializeField]
    private Text _label;

    [SerializeField]
    private Controller _controller;

    private float _labelFontSize;

    private CameraFollow _camera;

    private Transform _objectTarget;
    private Transform _targetedObject;
    public Toggle Toggle { get => _toggle; set => _toggle = value; }
    public Text Label { get => _label; set => _label = value; }

    private void Awake()
    {
        _camera = Camera.main.GetComponent<CameraFollow>();
        Toggle = GetComponent<Toggle>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _labelFontSize = Label.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        //Label.color = new Vector4(Label.color.r, Label.color.g, Label.color.b, Toggle.isOn ? 1f : 0.5f);

        _objectTarget = _camera.CameraTarget;

        if (_objectTarget == null)
        {
            Toggle.isOn = false;
            SetFocus();
            Toggle.interactable = false;
            //Label.color = new Vector4(Label.color.r, Label.color.g, Label.color.b, 0.5f);
        }
        else
        {
            Toggle.interactable = true;
            //Label.color = new Vector4(Label.color.r, Label.color.g, Label.color.b, 1f);

            if (_objectTarget != _targetedObject && _targetedObject != null)
            {
                SetFocus();
            }
        }

        if (Toggle.isOn)
        {
            Label.fontSize = Mathf.RoundToInt(_labelFontSize + (_labelFontSize / 3));
            Label.color = new Vector4(Label.color.r, Label.color.g, Label.color.b, 1f);
        }
        else if(Toggle.interactable)
        {
            Label.fontSize = (int)_labelFontSize;
            Label.color = new Vector4(Label.color.r, Label.color.g, Label.color.b, 1f);
        }
        else
        {
            Label.fontSize = (int)_labelFontSize;
            Label.color = new Vector4(Label.color.r, Label.color.g, Label.color.b, 0.5f);
        }
    }

    public void SetFocus()
    {
        if (Toggle.isOn)
        {

            //if an object
            if (_targetedObject != _objectTarget && _targetedObject != null)
            {

            }

            if (_objectTarget.GetComponent<StellarObject>())
            {
                //Debug.Log("_objectTarget.GetComponent<StellarObject>()");
                _targetedObject = _objectTarget;

                _camera.CameraAnchor = _objectTarget.GetComponent<StellarObject>().CameraAnchor;
                _camera.IsFocusing = true;

                //Show infos of the current object
            }
            else if (_objectTarget.GetComponent<Star>())
            {
                //Debug.Log("_objectTarget.GetComponent<StellarObject>()");
                _targetedObject = _objectTarget;

                _camera.CameraAnchor = _objectTarget.GetComponent<Star>().CameraAnchor;
                _camera.IsFocusing = true;
            }
        }
        else if (_targetedObject != null)
        {
            _targetedObject = null;
        }
    }
}
