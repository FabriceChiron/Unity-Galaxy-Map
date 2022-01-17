using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelToSelect : MonoBehaviour
{
    [SerializeField]
    private StellarSystemData _levelItem;

    public StellarSystemData LevelItem { get => _levelItem; set => _levelItem = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
