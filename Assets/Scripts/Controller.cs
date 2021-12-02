using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{

    private void Awake()
    {
/*        if (GameObject.FindGameObjectWithTag("Star"))
        {
            PlanetListDropdown = GameObject.FindGameObjectWithTag("PlanetsList").GetComponent<TMP_Dropdown>();
            PlanetListDropdown.AddOptions(new List<string> { GameObject.FindGameObjectWithTag("Star").name });
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEscapePressed())
        {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
    bool IsEscapePressed()
    {
        #if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null ? Keyboard.current.escapeKey.isPressed : false; 
        #else
        return Input.GetKey(KeyCode.Escape);
        #endif
    }
}
