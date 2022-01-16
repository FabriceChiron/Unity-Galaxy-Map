using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuNav : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Image _mainPanel, _gamePanel;

    private bool DeployGamePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DebugMessage(string message)
    {
        Debug.Log(message);
    }

    public void toggleGamePanel()
    {
        Debug.Log("toggleGamePanel");
        DeployGamePanel = !DeployGamePanel;
        _animator.SetBool("DeployGamePanel", DeployGamePanel);
    }

    public void NewGame()
    {
        Debug.Log("New Game!");
        SceneManager.LoadScene("Scene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
