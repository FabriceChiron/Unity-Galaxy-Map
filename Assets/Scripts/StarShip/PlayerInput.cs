using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR.LegacyInputHelpers;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalAxis;
    public float VerticalAxis;
    public float HorizontalDirection;
    public float VerticalDirection;
    public float FireAxis;
    public float BoostAxis;
    public float WarpAxis;
    public bool SwitchCameraButton;

    private void Awake()
    {
        GetInputs();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        GetInputs();
    }

    public void GetInputs()
    {
        HorizontalAxis = Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal") != 0 ? Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal") : Input.GetAxis("Horizontal");
        VerticalAxis = Input.GetAxis("XRI_Left_Primary2DAxis_Vertical") != 0 ? Input.GetAxis("XRI_Left_Primary2DAxis_Vertical") * -1f : Input.GetAxis("Vertical");

        HorizontalDirection = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal") != 0 ? Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal") : Input.GetAxis("Mouse X");
        VerticalDirection = Input.GetAxis("XRI_Right_Primary2DAxis_Vertical") != 0 ? Input.GetAxis("XRI_Right_Primary2DAxis_Vertical") : Input.GetAxis("Mouse Y");
        FireAxis = Input.GetAxis("XRI_Right_Trigger") != 0 ? Input.GetAxis("XRI_Right_Trigger") : Input.GetAxis("Fire1");
        BoostAxis = Input.GetAxis("XRI_Left_Grip") != 0 ? Input.GetAxis("XRI_Left_Grip") : Input.GetAxis("Boost");
        WarpAxis = Input.GetAxis("XRI_Left_Trigger")  !=0 ? Input.GetAxis("XRI_Left_Trigger") : Input.GetAxis("Warp");

        SwitchCameraButton = Input.GetButtonDown("XRI_Right_PrimaryButton") ? 
            Input.GetButtonDown("XRI_Right_PrimaryButton") :
                Input.GetButtonDown("XRI_Left_PrimaryButton") ?
                    Input.GetButtonDown("XRI_Left_PrimaryButton") :
                Input.GetKeyDown(KeyCode.C);

    }
}
