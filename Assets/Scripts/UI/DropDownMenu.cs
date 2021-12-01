using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDownMenu : MonoBehaviour
{

    private TMP_Dropdown PlanetListDropdown;

    private CameraFollow _camera;

    private void Awake()
    {
        _camera = Camera.main.GetComponent<CameraFollow>();

        PlanetListDropdown = GameObject.FindGameObjectWithTag("PlanetsList").GetComponent<TMP_Dropdown>();

        /*PlanetListDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(PlanetListDropdown);
        });*/

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropdownValueChanged(TMP_Dropdown Dropdown)
    {
        _camera.ChangeTarget(Dropdown.options[Dropdown.value].text.Replace("     ", ""));
    }
}
