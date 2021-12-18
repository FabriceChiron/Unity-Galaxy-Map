using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMainBody : MonoBehaviour
{
    [SerializeField]
    private Transform _mainBody;

    public Transform MainBody { get => _mainBody; set => _mainBody = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
