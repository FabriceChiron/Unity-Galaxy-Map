using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStellarSystem : MonoBehaviour
{

    private Animator _animator;

    public Animator Animator { get => _animator; set => _animator = value; }

    private void Awake()
    {
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
