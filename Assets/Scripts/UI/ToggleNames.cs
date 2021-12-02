using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleNames : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle;

    private void Awake()
    {
    
    }

    public void LoadPrefs()
    {
        if(!PlayerPrefs.HasKey("ShowNames"))
        {
            PlayerPrefs.SetInt("ShowNames", 0);
        }

        _toggle.isOn = (PlayerPrefs.GetInt("ShowNames") != 0) ? true : false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToggleNames()
    {
        PlayerPrefs.SetInt("ShowNames", (_toggle.isOn) ? 1 : 0);
    }
}
