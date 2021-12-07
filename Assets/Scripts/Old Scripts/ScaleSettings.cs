using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSettings : MonoBehaviour
{

    public Scales stellarScales;

    [SerializeField]
    private float _orbit;
    [SerializeField]
    private float _scaleFactor;
    [SerializeField]
    private float _planet;
    [SerializeField]
    private float _year;
    [SerializeField]
    private float _day;


    [SerializeField]
    private bool _rationalizeValues;

    public float Orbit { get => _orbit; set => _orbit = value; }
    public float ScaleFactor { get => _scaleFactor; set => _scaleFactor = value; }
    public float Planet { get => _planet; set => _planet = value; }
    public float Year { get => _year; set => _year = value; }
    public float Day { get => _day; set => _day = value; }
    public bool RationalizeValues { get => _rationalizeValues; set => _rationalizeValues = value; }

    private void Awake()
    {
        stellarScales = Resources.Load<Scales>("Data/Scales");

        Orbit = stellarScales.Orbit;
        Planet = stellarScales.Planet;
        Year = stellarScales.Year;
        Day = stellarScales.Day;
        RationalizeValues = stellarScales.RationalizeValues;
    }

    public float dimRet(float val, float scale, bool rationalizeValues)
    {
        if (val < 0)
        {
            return -dimRet(-val, scale, rationalizeValues);
        }

        float mult = val / scale;
        float trinum = (Mathf.Sqrt(4.0f * mult + 1.0f) - 1.0f) / 2.0f;

        //Debug.Log($"val : {val} - trinum * scale {trinum * scale}");

        if (!rationalizeValues)
        {
            //Debug.Log($"RationalizeValues is {rationalizeValues}, returning 'val': {val}");
            return val;
        }
        else
        {
            //Debug.Log($"RationalizeValues is {rationalizeValues}, returning 'trinum * scale': {trinum * scale}");
            return trinum * scale;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Orbit = stellarScales.Orbit;
        Planet = stellarScales.Planet;
        Year = stellarScales.Year;
        Day = stellarScales.Day;
        RationalizeValues = stellarScales.RationalizeValues;
    }
}
