using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDownMenu : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _label;

    private TMP_Dropdown _planetListDropdown;

    private CameraFollow _camera;

    public TextMeshProUGUI Label { get => _label; set => _label = value; }
    public TMP_Dropdown PlanetListDropdown { get => _planetListDropdown; set => _planetListDropdown = value; }

    private void Awake()
    {
        _camera = Camera.main.GetComponent<CameraFollow>();

        PlanetListDropdown = GetComponent<TMP_Dropdown>();

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
        string StellarObjectName = Dropdown.options[Dropdown.value].text.Replace("    ", "").Replace("<b>", "").Replace("</b>", "");
        Label.text = StellarObjectName;
        Debug.Log(StellarObjectName);
        _camera.ChangeTarget(StellarObjectName);
        //PlanetListDropdown.transform.GetChild(0)text = StellarObjectName;
    }
}
