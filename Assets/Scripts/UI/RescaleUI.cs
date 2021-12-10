using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RescaleUI : MonoBehaviour
{
    [SerializeField]
    private float _UIScaleFactor;

    [SerializeField]
    private float _resizeTextInside;

    [SerializeField]
    private RectTransform[] _itemsToResize;

    [SerializeField]
    private TextMeshProUGUI _displayScreenRatio;


    private CanvasScaler _canvasScaler;

    private float _baseDPI = 72f;
    private float _screenDPI;
    private float _dpiRatio;
    private void Awake()
    {
        _screenDPI = Screen.dpi;

        _dpiRatio = _screenDPI / _baseDPI;

        _UIScaleFactor = _baseDPI / _screenDPI;

        if(_dpiRatio > 2f)
        {

        }

        _canvasScaler = GetComponent<CanvasScaler>();
/*

        if(_displayScreenRatio != null)
        {
            _displayScreenRatio.text = $"Scale Factor: {_UIScaleFactor} \nText Factor: {_resizeTextInSide}";
        }


        _canvasScaler.scaleFactor = _UIScaleFactor;

        Debug.Log($"{name} - {_displayScreenRatio}");
*/

        if (_resizeTextInside > 0f)
        {
            foreach (RectTransform rectTransform in _itemsToResize)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * _resizeTextInside, rectTransform.sizeDelta.y * _resizeTextInside);
            }

            foreach (TextMeshProUGUI TextComp in _canvasScaler.gameObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                Debug.Log($"TextMeshProUGUI - {TextComp.text}");
                TextComp.fontSize *= _resizeTextInside;
            }

            foreach (Text TextComp in _canvasScaler.gameObject.GetComponentsInChildren<Text>())
            {
                TextComp.fontSize *= Mathf.RoundToInt(_resizeTextInside);
            }
        }


#if UNITY_ANDROID

        /*_canvasScaler = GetComponent<CanvasScaler>();

        if(_resizeTextInSide > 0f)
        {
            foreach(TextMeshProUGUI TextComp in _canvasScaler.gameObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                TextComp.fontSize *= _resizeTextInSide;
            }
        }

        _canvasScaler.scaleFactor = _UIScaleFactor;
        Debug.Log("on Android");*/
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
