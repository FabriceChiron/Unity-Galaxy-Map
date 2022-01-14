using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void toggleGamePanel()
    {
        DeployGamePanel = !DeployGamePanel;
        _animator.SetBool("DeployGamePanel", DeployGamePanel);
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
