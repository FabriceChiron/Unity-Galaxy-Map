using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR.LegacyInputHelpers;

public class VRControllers : MonoBehaviour
{
    [SerializeField]
    private GameObject[] laserSources;

    [SerializeField]
    private GraphicRaycaster graphicRaycaster;

    private PointerEventData _pointerEventData;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private RectTransform canvasRect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Set up the new Pointer Event
        _pointerEventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the game object
        _pointerEventData.position = this.transform.localPosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        graphicRaycaster.Raycast(_pointerEventData, results);

        if (results.Count > 0) Debug.Log("Hit " + results[0].gameObject.name);

        /*foreach(GameObject laserSource in laserSources)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit)) { 

                

                if (hit.collider.gameObject.tag == "Tagged") { 
                    Debug.DrawRay(transform.position, transform.forward, Color.green); print("Hit"); 
                } 
            }

        }*/

    }
}
