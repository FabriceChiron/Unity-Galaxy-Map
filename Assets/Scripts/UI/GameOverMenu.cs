using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    private Memory _memory;

    public Memory Memory { get => _memory; set => _memory = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("SavedData") != null)
        {
            Memory = GameObject.FindGameObjectWithTag("SavedData").GetComponent<Memory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("SavedData") != null)
        {
            Memory = GameObject.FindGameObjectWithTag("SavedData").GetComponent<Memory>();
        }
    }

    public void Restart()
    {
        Memory.SelectedSystem = Memory.SavedStellarSystem.Item;
        Debug.Log("Resume Game!");
        SceneManager.LoadScene("Scene", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }
}
