using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    [SerializeField]
    private StellarSystemData stellarSystemData;
    
    [SerializeField]
    private Scales scales;

    public StellarSystemData StellarSystemData { get => stellarSystemData; set => stellarSystemData = value; }

    // Start is called before the first frame update
    void Start()
    {
        SetScales();

        SetMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Apply material set in StellarSystemData 
    public void SetMaterial()
    {
        Debug.Log($"{StellarSystemData.Name} - {StellarSystemData.Material}");

        Material starMaterial = GetComponent<MeshRenderer>().material;
        starMaterial = StellarSystemData.Material;

        //If starMaterial is not the sun, adapt the color of the light to the material emission color
        if (!starMaterial.name.Contains("sun-texture"))
        {
            GetComponent<Light>().color = Color.Lerp(Color.white, starMaterial.GetColor("_EmissionColor"), 0.1f);
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
}
