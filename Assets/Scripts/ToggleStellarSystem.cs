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

        foreach(StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
        {
            stellarObject.UIDetails.gameObject.SetActive(true);
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

        foreach(StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
        {
            stellarObject.Animator.SetBool("ShowName", false);
        }
        Animator.SetBool("IsDeployed", false);
    }
}
