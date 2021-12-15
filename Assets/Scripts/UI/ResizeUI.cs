using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeUI : MonoBehaviour
{
    [SerializeField]
    private float _widthThreshold;

    [SerializeField]
    private Canvas _canvasDefault, _canvasOverride;
    
    private void Awake()
    {
        SwitchCanvas();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Controller>().DeviceInfo.text = $"IsMobile: {GameObject.FindObjectOfType<DetectMobile>().isMobile()}\nTouch Supported: {Input.touchSupported}\nScreen Width: {Screen.width} \nScreen Height: {Screen.height} \nScreen DPI: {Screen.dpi}";
        SwitchCanvas();
    }

    private void SwitchCanvas()
    {
        if (Screen.width > Screen.height && Screen.width >= _widthThreshold)
        {
            _canvasDefault.gameObject.SetActive(false);
            _canvasOverride.gameObject.SetActive(true);
        }
        else
        {
            _canvasOverride.gameObject.SetActive(false);
            _canvasDefault.gameObject.SetActive(true);
        }
    }
}
