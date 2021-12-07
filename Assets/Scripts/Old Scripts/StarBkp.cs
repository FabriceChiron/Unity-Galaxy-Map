using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarBkp : MonoBehaviour
{
    [SerializeField]
    private Scales scales;

    [SerializeField]
    private Texture _texture;

    [SerializeField]
    private Material _material;

    [SerializeField]
    private StellarSystemData stellarSystemData;

    [SerializeField]
    private ScaleSettings _scaleSettings;

    private Controller _controller;

    public Texture Texture { get => _texture; set => _texture = value; }
    public StellarSystemData StellarSystemData { get => stellarSystemData; set => stellarSystemData = value; }

    public TMP_Dropdown PlanetListDropdown;

    private void Awake()
    {
        _controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();

        PlanetListDropdown = GameObject.FindGameObjectWithTag("PlanetsList").GetComponent<TMP_Dropdown>();

        PlanetListDropdown = transform.parent.GetComponent<GeneratePlanets>().PlanetListDropdown;

    }

    // Start is called before the first frame update
    void Start()
    {
        StellarSystemData = transform.parent.GetComponent<GeneratePlanets>().StellarSystemData;
        _material = StellarSystemData.Material;

        //AddToDropdown();

        CustomiseStar();

        SetScales();
    }

    // Update is called once per frame
    void Update()
    {
        //SetScales();
    }

    public void AddToDropdown()
    {
        PlanetListDropdown.AddOptions(new List<string> { StellarSystemData.StarName });
    }

    public void CustomiseStar()
    {
        transform.name = StellarSystemData.StarName;
        GetComponent<MeshRenderer>().material = _material;
        Debug.Log(GetComponent<MeshRenderer>().material.name);

        if(!GetComponent<MeshRenderer>().material.name.Contains("sun-texture"))
        {
            GetComponent<Light>().color = Color.Lerp(Color.white, GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), 0.1f);
        }

    }

    public void SetScales()
    {
        _controller.ClearTrails();

        if (!scales.RationalizeValues)
        {
            transform.localScale = new Vector3(StellarSystemData.StarSize * scales.Planet, StellarSystemData.StarSize * scales.Planet, StellarSystemData.StarSize * scales.Planet);
        }
        else
        {
            transform.localScale = new Vector3(5f, 5f, 5f);
        }
    }
}
