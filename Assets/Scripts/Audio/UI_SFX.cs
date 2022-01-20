using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIType
{
    BUTTON,
    TOGGLE,
}

public class UI_SFX : MonoBehaviour
{

    [SerializeField]
    private UIType _UIType;

    [SerializeField]
    private AudioClip[] _audioClips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySFX()
    {
        switch (_UIType)
        {
            case UIType.TOGGLE:
                Toggle _toggle = GetComponent<Toggle>();

                GetComponent<AudioSource>().PlayOneShot(_audioClips[(_toggle.isOn ? 1 : 0)]);

                break;

            case UIType.BUTTON:
                GetComponent<AudioSource>().PlayOneShot(_audioClips[0]);
                break;
        }
    }

}
