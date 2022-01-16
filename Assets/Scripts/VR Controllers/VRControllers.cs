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
    private string[] triggerInputs;

    [SerializeField]
    private GameObject[] overlays;

    // Start is called before the first frame update
    void Start()
    {
        if (!XRSettings.isDeviceActive)
        {
            this.gameObject.SetActive(false);

            foreach(GameObject overlay in overlays)
            {
                overlay.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < laserSources.Length; i++)
        {
            
            RaycastHit hit;
            if (Physics.Raycast(laserSources[i].transform.position, laserSources[i].transform.forward, out hit))
            {
                LinkGameObjectToUIElement UIElement = hit.collider.GetComponent<LinkGameObjectToUIElement>();

                if(hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                }

                if (hit.collider.GetComponent<LinkGameObjectToUIElement>() != null && Input.GetButtonDown($"{triggerInputs[i]}"))
                {
                    UIElement.TriggerUIElement();
                }
            }

        }

    }
}
