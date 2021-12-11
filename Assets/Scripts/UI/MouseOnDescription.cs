using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseOnDescription : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private void OnMouseEnter()
    {
        Debug.Log("Entering description");
    }

    private void OnMouseExit()
    {
        Debug.Log("Leaving description");
        _animator.SetBool("ShowDetails", false);
    }

}
