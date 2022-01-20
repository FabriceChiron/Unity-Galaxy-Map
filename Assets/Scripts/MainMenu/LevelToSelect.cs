using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelToSelect : MonoBehaviour
{
    [SerializeField]
    private StellarSystemData _levelItem;

    [SerializeField]
    private MainMenuNav _mainMenuNav;

    public StellarSystemData LevelItem { get => _levelItem; set => _levelItem = value; }

    // Start is called before the first frame update
    void Start()
    {
        _mainMenuNav = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenuNav>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectGame()
    {
        Debug.Log($"LevelToSelect - SelectGame: {LevelItem}");
        _mainMenuNav.SelectGame(LevelItem);
    }
}
