using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RescaleUI : MonoBehaviour
{
    [SerializeField]
    private float _scaleFactor;

    [SerializeField]
    private float _resizeTextInSide;

    private CanvasScaler _canvasScaler;

    private void Awake()
    {
#if UNITY_ANDROID

        _canvasScaler = GetComponent<CanvasScaler>();

        if(_resizeTextInSide > 0f)
        {
            foreach(TextMeshProUGUI TextComp in _canvasScaler.gameObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                TextComp.fontSize *= _resizeTextInSide;
            }
        }

        _canvasScaler.scaleFactor = _scaleFactor;
        Debug.Log("on Android");
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
