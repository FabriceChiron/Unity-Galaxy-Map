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
        DeployStellarSystem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeployStellarSystem()
    {
        Animator.SetBool("IsDeployed", true);
    }

    public void FoldStellarSystem()
    {
        Animator.SetBool("IsDeployed", false);
    }
}
