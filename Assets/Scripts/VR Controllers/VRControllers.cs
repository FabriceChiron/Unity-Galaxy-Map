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
    private List<LineRenderer> laserLineRenderers;

    [SerializeField]
    private string[] triggerInputs;

    [SerializeField]
    private Transform[] _UIContainers;

    [SerializeField]
    private bool _duplicateUIElementsToVR;

    [SerializeField]
    private List<GameObject> _VROverlaysUI;

    [SerializeField]
    private GameObject _VROverlayUIPrefab;

    [SerializeField]
    private List<GameObject> _UIImages;

    [SerializeField]
    private GameObject[] overlays;

    [SerializeField]
    private GameObject _squareButtonOverlayPrefab, _rectangleButtonOverlayPrefab, _levelBtnPrefab;

    private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        LayerMask layerMask = 1 << 9;

        if (XRSettings.isDeviceActive)
        {

            foreach(GameObject laserSource in laserSources)
            {
                laserLineRenderers.Add(laserSource.GetComponent<LineRenderer>());
            }

         
            foreach(Transform UIContainer in _UIContainers)
            {
                GameObject newVROverlayUI = Instantiate(_VROverlayUIPrefab, UIContainer.transform.parent);
                newVROverlayUI.transform.localPosition = new Vector3(
                    UIContainer.transform.localPosition.x,
                    UIContainer.transform.localPosition.y,
                    UIContainer.transform.localPosition.z - 0.01f
                ); ;

                _VROverlaysUI.Add(newVROverlayUI);
    
                DuplicateUIElementsToGameObjects(UIContainer);
            }


            /*
            foreach(GameObject overlay in overlays)
            {
                overlay.SetActive(false);
            }
            */
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < laserSources.Length; i++)
        {
            
            RaycastHit hit;
            if (Physics.Raycast(laserSources[i].transform.position, laserSources[i].transform.forward, out hit, Mathf.Infinity, ~layerMask))
            {
                Debug.Log($"RaycastHit: {hit.transform.name}");

                laserLineRenderers[i].SetPosition(1, new Vector3(0f ,0f , hit.distance));

                LinkGameObjectToUIElement UIElement = hit.collider.GetComponent<LinkGameObjectToUIElement>();

                if (hit.collider.GetComponent<LinkGameObjectToUIElement>() != null && Input.GetButtonDown($"{triggerInputs[i]}"))
                {
                    //Debug.Log("yo");
                    //Debug.Log(hit.collider.name);
                    UIElement.TriggerUIElement();
                }
            }

        }

    }

    public void DuplicateUIElementsToGameObjects(Transform container)
    {
        foreach (GameObject VROverlayUI in _VROverlaysUI)
        {
            foreach (Image image in container.GetComponentsInChildren<Image>())
            {
                Debug.Log(image.name);
                if (image.transform.childCount > 0 && image.GetComponent<Canvas>() == null)
                {
                    GameObject GODuplicate;
                    if (image.enabled)
                    {
                        if (image.GetComponent<RectTransform>().sizeDelta.x == image.GetComponent<RectTransform>().sizeDelta.y)
                        {
                            GODuplicate = Instantiate(_squareButtonOverlayPrefab, VROverlayUI.transform);
                        }
                        else
                        {
                            GODuplicate = Instantiate(_rectangleButtonOverlayPrefab, VROverlayUI.transform);
                        }

                        GODuplicate.name = image.name.Replace("Button -", "Button Overlay -");
                        GODuplicate.name = image.name.Replace("Toggle -", "Toggle Overlay -");


                        GODuplicate.GetComponent<LinkGameObjectToUIElement>().UIElement = image.gameObject;
                        GODuplicate.GetComponent<LinkGameObjectToUIElement>().VROverlayUI = VROverlayUI.transform;

                        _UIImages.Add(GODuplicate);
                    }
                }
            }

        }    
    }

}
