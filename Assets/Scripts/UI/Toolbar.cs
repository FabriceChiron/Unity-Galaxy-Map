using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Toolbar : MonoBehaviour
{
    private Animator _animator;
    private bool _showSettings;

    public Animator Animator { get => _animator; set => _animator = value; }
    public bool ShowSettings { get => _showSettings; set => _showSettings = value; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animator.SetBool("ShowSettings", ShowSettings);
    }
}
