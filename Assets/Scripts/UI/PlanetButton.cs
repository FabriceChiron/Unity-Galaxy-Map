using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetButton : MonoBehaviour
{

    [SerializeField]
    private StellarObject _stellarObject;

    [SerializeField]
    private Star _starObject;

    private Controller _controller;

    public StellarObject StellarObject { get => _stellarObject; set => _stellarObject = value; }
    public Star StarObject { get => _starObject; set => _starObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        _controller = GameObject.FindObjectOfType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SelectPlanet()
    {
        if (StellarObject && !_controller.HasPlayer)
        {
            if (StellarObject.Camera.CameraTarget == StellarObject.StellarBody)
            {
                StellarObject.Camera.CameraAnchor = StellarObject.CameraAnchor;
                StellarObject.Camera.CameraAnchorObject = StellarObject.gameObject;
                //StellarObject.Animator.SetBool("ShowDetails", true);
                StellarObject.Animator.SetBool("ShowName", false);
            }

            StellarObject.Camera.ChangeTarget(StellarObject.StellarBody);
        }

    }
    public void SelectStar()
    {
        if (StarObject)
        {
            if (StarObject.Camera.CameraTarget == StarObject.StarBody)
            {
                StarObject.Camera.CameraAnchor = StarObject.CameraAnchor;
                StarObject.Camera.CameraAnchorObject = StarObject.gameObject;
                //StellarObject.Animator.SetBool("ShowDetails", true);
                StarObject.Animator.SetBool("ShowName", false);
            }

            StarObject.Camera.ChangeTarget(StarObject.StarBody);
        }

    }

    public void ShowName()
    {
        if (StellarObject)
        {
            StellarObject.Animator.SetBool("ShowName", true);
        }
        else if (StarObject)
        {
            StarObject.Animator.SetBool("ShowName", true);
        }

        /*if (StellarObject.Camera.CameraTarget == StellarObject.StellarBody)
        {
            StellarObject.Animator.SetBool("ShowName", false);
        }
        else
        {
            StellarObject.Animator.SetBool("ShowName", true);
        }*/
    }

    public void HideName()
    {
        if (StellarObject)
        {
            StellarObject.Animator.SetBool("ShowName", false);
        }
        else if (StarObject)
        {
            StarObject.Animator.SetBool("ShowName", false);
        }
    }
}
