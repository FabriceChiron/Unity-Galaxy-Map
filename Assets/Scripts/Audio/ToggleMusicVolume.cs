using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMusicVolume : MonoBehaviour
{

    [SerializeField]
    private Toggle _toggle;

    [SerializeField]
    private Controller _controller;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPref();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadPref()
    {
        // if MusicOn is already stored in PlayerPrefs
        if (PlayerPrefs.HasKey("MusicOn"))
        {
            // Set Toggle On/Off based on PlayerPref value
            _toggle.isOn = PlayerPrefs.GetInt("MusicOn") != 0;
        }
        // if MusicOn is not yet stored in PlayerPrefs
        else
        {
            //Store it based on Toggle On/Off (On by default, should set MusicOn to 1;
            PlayerPrefs.SetInt("MusicOn", _toggle.isOn ? 1 : 0);
        }

        SetMusicVolume();
    }

    public void SetMusicVolume()
    {
        if(_toggle.isOn)
        {
            _controller.AudioMixer.SetFloat("MusicVolume", 0f);
            _animator.SetBool("MusicOn", true);
        }
        else
        {
            _controller.AudioMixer.SetFloat("MusicVolume", -80f);
            _animator.SetBool("MusicOn", false);
        }
    }
}
