using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    [SerializeField]
    private StellarSystemData stellarSystemData;
    
    [SerializeField]
    private Scales scales;

    private Material _material;

    private CameraFollow _camera;

    public StellarSystemData StellarSystemData { get => stellarSystemData; set => stellarSystemData = value; }
    public CameraFollow Camera { get => _camera; set => _camera = value; }

    // Start is called before the first frame update
    void Start()
    {
        Camera = UnityEngine.Camera.main.GetComponent<CameraFollow>();
        
        SetScales();

        _material = StellarSystemData.Material;

        SetMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        //DetectClick();
    }


    //Apply material set in StellarSystemData 
    public void SetMaterial()
    {
        Renderer renderer= GetComponent<MeshRenderer>();
        renderer.material = _material;

        if (!renderer.material.name.Contains("sun-texture"))
        {
            GetComponent<Light>().color = Color.Lerp(Color.white, renderer.material.GetColor("_EmissionColor"), 0.1f);
        }
    }

    //Set scales according to "scales" scriptable object and StellarSystemData (for star size)
    public void SetScales()
    {
        //if the scales are not rationalized
        if (!scales.RationalizeValues)
        {
            //star scale is calculated with the star size (in Earth size) and the scales applied to planets
            transform.localScale = new Vector3(StellarSystemData.StarSize * scales.Planet, StellarSystemData.StarSize * scales.Planet, StellarSystemData.StarSize * scales.Planet);
        }
        //else, set a default size for the star (multiplied by the scales applied to planets
        else
        {
            transform.localScale = new Vector3(5f * scales.Planet, 5f * scales.Planet, 5f * scales.Planet);
        }
    }

    private void DetectClick()
    {

        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

        // Visualize Ray on Scene (no impact on Game view)
        Debug.DrawRay(ray.origin, ray.direction * 20f);

        RaycastHit hit;

        //if the mouse is on the sun and and is clicked
        if (Physics.Raycast(ray, out hit) && hit.transform == transform && Input.GetMouseButton(0))
        {
            Camera.ChangeTarget(hit.transform);
        }
    }
}
