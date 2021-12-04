using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStellarSystem : MonoBehaviour
{

    private Animator _animator;
    private bool _isAnimating;

    public Animator Animator { get => _animator; set => _animator = value; }
    public bool IsAnimating { get => _isAnimating; set => _isAnimating = value; }

    private Controller _controller;



    private void Awake()
    {
        _controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //DeployStellarSystem();
    }

    // Update is called once per frame
    void Update()
    {
        if (Animator.GetBool("IsAnimating"))
        {
            _controller.ClearTrails();
        }
    }

    public void toggleIsAnimating()
    {
        Animator.SetBool("IsAnimating", !Animator.GetBool("IsAnimating"));
    }

    public void ActivateUIDetails()
    {
        Camera.main.GetComponent<CameraFollow>().ResetCameraTarget();
        foreach (GameObject UIDetails in GameObject.FindGameObjectsWithTag("UI - Details"))
        {
            UIDetails.SetActive(true);
        }
    }

    public void DeployStellarSystem()
    {
        Animator.SetBool("IsDeployed", true);

    }

    public void FoldStellarSystem()
    {
        foreach(GameObject UIDetails in GameObject.FindGameObjectsWithTag("UI - Details"))
        {
            Destroy(UIDetails);
        }
        Animator.SetBool("IsDeployed", false);
    }
}
