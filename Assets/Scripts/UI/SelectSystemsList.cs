using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SelectSystemsList : MonoBehaviour
{

    [SerializeField]
    private StellarSystemData[] _stellarSystemsArray;

    private TMP_Dropdown _systemsDropdown;

    private void Awake()
    {
        _systemsDropdown = GetComponent<TMP_Dropdown>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
