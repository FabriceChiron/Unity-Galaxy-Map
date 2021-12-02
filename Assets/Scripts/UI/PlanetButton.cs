using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetButton : MonoBehaviour
{

    [SerializeField]
    private Planet _planet;

    public Planet Planet { get => _planet; set => _planet = value; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SelectPlanet()
    {
        if (Planet.Camera.CameraTarget == Planet.StellarObject)
        {
            Planet.Camera.CameraAnchor = Planet.CameraAnchor;
            Planet.Camera.CameraAnchorObject = Planet.gameObject;
            Planet.Animator.SetBool("ShowDetails", true);
        }

        Planet.Camera.ChangeTarget(Planet.StellarObject);

    }

    public void ShowName()
    {
        if(Planet.Camera.CameraTarget == Planet.StellarObject)
        {
            Planet.Animator.SetBool("ShowName", false);
        }
        else
        {
            Planet.Animator.SetBool("ShowName", true);
        }
    }

    public void HideName()
    {
        Planet.Animator.SetBool("ShowName", false);
    }
}
