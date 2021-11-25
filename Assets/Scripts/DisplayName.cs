using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayName : MonoBehaviour
{
    private TextMeshPro _TMPro;
    private Color _startColor;

    private void Awake()
    {
        _TMPro = GetComponent<TextMeshPro>();

        _TMPro.color = new Color(_TMPro.color.r, _TMPro.color.g, _TMPro.color.b, 0f);

        _startColor = _TMPro.color;

        //Debug.Log(_TMPro.color.a);
    }

    public void ToggleName(bool show)
    {
        //Debug.Log(_TMPro.text);
        _TMPro.color = new Color(_TMPro.color.r, _TMPro.color.g, _TMPro.color.b, (show ? 1f : 0f) * 10f / Time.deltaTime);
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
