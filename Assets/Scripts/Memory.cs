using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{

    [SerializeField]
    private StellarSystemData _selectedSystem;

    [SerializeField]
    private SavedStellarSystem _savedStellarSystem;

    [SerializeField]
    private StellarSystemsArray _stellarSystemsArray;

    public StellarSystemData SelectedSystem { get => _selectedSystem; set => _selectedSystem = value; }
    public SavedStellarSystem SavedStellarSystem { get => _savedStellarSystem; set => _savedStellarSystem = value; }
    public StellarSystemsArray StellarSystemsArray { get => _stellarSystemsArray; set => _stellarSystemsArray = value; }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SelectedSystem != null)
        {
            SavedStellarSystem.Item = SelectedSystem;
        }
    }
}
