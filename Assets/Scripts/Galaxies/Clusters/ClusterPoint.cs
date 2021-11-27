using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClusterPoint : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _UIName;

    public TextMeshProUGUI UIName { get => _UIName; set => _UIName = value; }

    // Start is called before the first frame update
    void Start()
    {
        ToggleClusterName(false);
    }


    // Update is called once per frame
    void Update()
    {
        UIName.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void ToggleClusterName(bool toggle)
    {
        UIName.gameObject.SetActive(toggle);
    }
}
