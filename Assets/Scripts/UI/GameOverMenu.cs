using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private Memory _memory;

    public Memory Memory { get => _memory; set => _memory = value; }

    // Start is called before the first frame update

    public void Restart()
    {
        Memory.SavedData.SelectedSystem = Memory.SavedData.SavedStellarSystem.Item;
        Debug.Log("Resume Game!");
        SceneManager.LoadScene("Scene", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }
}
