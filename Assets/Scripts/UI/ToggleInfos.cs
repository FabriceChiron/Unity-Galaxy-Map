using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleInfos : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle;

    [SerializeField]
    private Text _label;

    [SerializeField]
    private Controller _controller;

    [SerializeField]
    private ToggleFocus _toggleFocus;

    private string _iconIsOn = "s";
    private string _iconIsOff = "Q";
    //private string _iconIsOn = "[";
    //private string _iconIsOff = "F";

    private CameraFollow _camera;

    private Transform _objectTarget;
    private Transform _targetedObject;


    public Toggle Toggle { get => _toggle; set => _toggle = value; }
    public Text Label { get => _label; set => _label = value; }

    private void Awake()
    {
        _camera = _controller.MainCamera.GetComponent<CameraFollow>();
        Toggle = GetComponent<Toggle>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Label.text = (Toggle.isOn ? _iconIsOn : _iconIsOff);
        _objectTarget = _camera.CameraTarget;

        if(_objectTarget == null || !HasInfos(_objectTarget))
        {
            Toggle.isOn = false;
            SetToggleInfos();
            Toggle.interactable = false;
            Label.color = new Vector4(Label.color.r, Label.color.g, Label.color.b, 0.5f);
        }
        else if(HasInfos(_objectTarget))
        {
            Toggle.interactable = true;
            Label.color = new Vector4(Label.color.r, Label.color.g, Label.color.b, 1f);

            if(_objectTarget != _targetedObject && _targetedObject != null)
            {
                SetToggleInfos();
            }
        }
    }

    public bool HasInfos(Transform spaceObject)
    {
        if(spaceObject.GetComponent<StellarObject>() != null || spaceObject.GetComponent<Star>())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetToggleInfos()
    {
        /*Debug.Log($"Toggle.isOn: {Toggle.isOn}");
        Debug.Log($"_targetedObject: {_targetedObject}");
        Debug.Log($"_objectTarget: {_objectTarget}");*/

        if (Toggle.isOn)
        {
            
            //if an object
            if(_targetedObject != _objectTarget && _targetedObject != null)
            {
                //Debug.Log("_targetedObject != _objectTarget");
                //Hide infos of previously opened object
                ShowInfos(_targetedObject, false);
            }

            if (_objectTarget.GetComponent<StellarObject>())
            {
                //Debug.Log("_objectTarget.GetComponent<StellarObject>()");
                _targetedObject = _objectTarget;

                _toggleFocus.GetComponent<Toggle>().isOn = true;

                //Show infos of the current object
                ShowInfos(_objectTarget, true);
            }
            else if (_objectTarget.GetComponent<Star>())
            {
                //Debug.Log("_objectTarget.GetComponent<StellarObject>()");
                _targetedObject = _objectTarget;

                _toggleFocus.GetComponent<Toggle>().isOn = true;

                //Show infos of the current object
                ShowInfos(_objectTarget, true);

            }
        }
        else if(_targetedObject != null)
        {
            //Hide infos of the current object
            ShowInfos(_targetedObject, false);
            _targetedObject = null;
        }
    }

    public void ShowInfos(Transform thisObject, bool show)
    {
        if (thisObject.GetComponent<StellarObject>())
        {
            thisObject.GetComponent<StellarObject>().Animator.SetBool("ShowDetails", show);
        }
        else if (thisObject.GetComponent<Star>())
        {
            thisObject.GetComponent<Star>().Animator.SetBool("ShowDetails", show);
        }
    }
}
