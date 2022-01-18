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

    [SerializeField]
    private Memory _memory;

    [SerializeField]
    private VRControllers _VRControllers;

    [SerializeField]
    private Transform _UIContainer;

    [SerializeField]
    private Transform _VROverlayUI;

    [SerializeField]
    private List<GameObject> _UIImages;

    [SerializeField]
    private GameObject _squareButtonOverlayPrefab, _rectangleButtonOverlayPrefab, _levelBtnPrefab;

    [SerializeField]
    private Button _btnResumeGame;
    
    [SerializeField]
    private Toggle _btnSelectLevel;

    [SerializeField]
    private Transform _levelsList;

    private bool DeployGamePanel, DeploySelectLevel;

    // Start is called before the first frame update
    void Start()
    {
        _btnResumeGame.interactable = _memory.SavedStellarSystem.Item != null;

        //FillLevelsList();

        //DuplicateUIElementsToGameObjects(_UIContainer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FillLevelsList()
    {
        _levelsList.GetComponent<RectTransform>().sizeDelta = new Vector2(
            _levelBtnPrefab.GetComponent<RectTransform>().sizeDelta.x,
            (_levelBtnPrefab.GetComponent<RectTransform>().sizeDelta.y * _memory.StellarSystemsArray.stellarSystemsArray.Length) + ((_memory.StellarSystemsArray.stellarSystemsArray.Length) * 10f)
        );

        foreach(StellarSystemData stellarSystemData in _memory.StellarSystemsArray.stellarSystemsArray)
        {
            Debug.Log(stellarSystemData);
            GameObject newBtnLevel = Instantiate(_levelBtnPrefab, _levelsList);

            newBtnLevel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            newBtnLevel.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 0);
            newBtnLevel.GetComponentInChildren<TextMeshProUGUI>().text = stellarSystemData.Name;

            newBtnLevel.GetComponent<LevelToSelect>().LevelItem = stellarSystemData;

            newBtnLevel.GetComponent<Button>().onClick.AddListener(
                delegate { 
                    SelectGame(stellarSystemData);
                }
            );

            newBtnLevel.name = $"Button - Select {stellarSystemData.Name}";
        }

        _VRControllers.DuplicateUIElementsToGameObjects(_levelsList);
    }

    public void DuplicateUIElementsToGameObjects(Transform container)
    {
        foreach(Image image in container.GetComponentsInChildren<Image>())
        {
            Debug.Log(image.name);
            if(image.transform.childCount > 0)
            {
                GameObject GODuplicate;
                if (image.enabled)
                {
                    if(image.GetComponent<RectTransform>().sizeDelta.x == image.GetComponent<RectTransform>().sizeDelta.y)
                    {
                        GODuplicate = Instantiate(_squareButtonOverlayPrefab, _VROverlayUI.transform);
                    }
                    else
                    {
                        GODuplicate = Instantiate(_rectangleButtonOverlayPrefab, _VROverlayUI.transform);
                    }

                    GODuplicate.name = image.name.Replace("-", "Overlay -");


                    GODuplicate.GetComponent<LinkGameObjectToUIElement>().UIElement = image.gameObject;
                    GODuplicate.GetComponent<LinkGameObjectToUIElement>().VROverlayUI = _VROverlayUI;

                    _UIImages.Add(GODuplicate);
                }
            }
        }
    }

    public void DebugMessage(string message)
    {
        Debug.Log(message);
    }

    public void ToggleGamePanel()
    {
        Debug.Log("toggleGamePanel");
        DeployGamePanel = !DeployGamePanel;

        if (!DeployGamePanel)
        {
            _btnSelectLevel.isOn = false;

            foreach (Animator selectLevelAnimator in _levelsList.GetComponentsInChildren<Animator>())
            {
                Destroy(selectLevelAnimator.gameObject);
            }
        }
        else
        {
            FillLevelsList();
        }

        /*DeploySelectLevel = false;
        foreach (Animator selectLevelAnimator in _levelsList.GetComponentsInChildren<Animator>())
        {
            selectLevelAnimator.SetBool("DeploySelectButton", DeploySelectLevel);
        }*/

        _animator.SetBool("DeployGamePanel", DeployGamePanel);

    }

    public void ToggleLevelSelect()
    {
        DeploySelectLevel = !DeploySelectLevel;

        _animator.SetBool("DeployLevelSelection", DeploySelectLevel);

        foreach (Animator selectLevelAnimator in _levelsList.GetComponentsInChildren<Animator>())
        {
            selectLevelAnimator.SetBool("DeploySelectButton", DeploySelectLevel);
        }
    }


    public void SelectLevel(StellarSystemData stellarSystemData)
    {
        _memory.SelectedSystem = stellarSystemData;
    }

    public void NewGame()
    {
        Debug.Log("New Game!");
        SceneManager.LoadScene("Scene", LoadSceneMode.Single);
    }

    public void SelectGame(StellarSystemData stellarSystemData)
    {
        _memory.SelectedSystem = stellarSystemData;
        Debug.Log("Select level and new Game!");
        SceneManager.LoadScene("Scene", LoadSceneMode.Single);
    }

    public void ResumeGame()
    {
        _memory.SelectedSystem = _memory.SavedStellarSystem.Item;
        Debug.Log("Resume Game!");
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
