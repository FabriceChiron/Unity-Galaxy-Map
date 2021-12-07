using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetButton : MonoBehaviour
{

    [SerializeField]
    private StellarObject _stellarObject;

    public StellarObject StellarObject { get => _stellarObject; set => _stellarObject = value; }

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
        if (StellarObject.Camera.CameraTarget == StellarObject.StellarBody)
        {
            StellarObject.Camera.CameraAnchor = StellarObject.CameraAnchor;
            StellarObject.Camera.CameraAnchorObject = StellarObject.gameObject;
            StellarObject.Animator.SetBool("ShowDetails", true);
        }

        StellarObject.Camera.ChangeTarget(StellarObject.StellarBody);

    }

    public void ShowName()
    {
        if (StellarObject.Camera.CameraTarget == StellarObject.StellarBody)
        {
            StellarObject.Animator.SetBool("ShowName", false);
        }
        else
        {
            StellarObject.Animator.SetBool("ShowName", true);
        }
    }

    public void HideName()
    {
        StellarObject.Animator.SetBool("ShowName", false);
    }
}
