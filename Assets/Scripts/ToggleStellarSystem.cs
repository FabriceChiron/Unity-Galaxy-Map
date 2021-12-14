using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStellarSystem : MonoBehaviour
{

    private Animator _animator;
    private bool _isAnimating;

    private float _stellarSystemScale;

    public Animator Animator { get => _animator; set => _animator = value; }
    public bool IsAnimating { get => _isAnimating; set => _isAnimating = value; }

    private Controller _controller;

    [SerializeField]
    private float _resetTimeToScale = 1f;
    private float _timeToScale;



    private void Awake()
    {
        _timeToScale = _resetTimeToScale;

        _controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeScale(0f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Animator.GetBool("IsAnimating"))
        {
            _controller.ClearTrails();
            
        }

        
    }

    public void toggleIsAnimating()
    {
        Animator.SetBool("IsAnimating", !Animator.GetBool("IsAnimating"));
    }

    public void ActivateUIDetails()
    {
        Camera.main.GetComponent<CameraFollow>().ResetCameraTarget();

        foreach(StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
        {
            stellarObject.UIDetails.gameObject.SetActive(true);
        }
    }

    public void DeployStellarSystem()
    {
        Debug.Log("DeployStellarSystem");
        Animator.SetBool("IsDeployed", true);

        //ChangeScale(0f, 1f, 1f);
        
    }
    private IEnumerator ChangeScale(float initialScale, float targetScale, float duration)
    {
        float currentScale;
        for (float t= 0f; t < duration; t += Time.deltaTime)
        {
            currentScale = Mathf.Lerp(initialScale, targetScale, t / duration);
            yield return null;
        }
        currentScale = targetScale;
        Debug.Log("ChangeScale");
        Animator.SetFloat("Scale", currentScale);
    }

    public void FoldStellarSystem()
    {
        foreach(GameObject UIDetails in GameObject.FindGameObjectsWithTag("UI - Details"))
        {
            Destroy(UIDetails);
        }

        foreach(StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
        {
            stellarObject.Animator.SetBool("ShowName", false);
        }
        Animator.SetBool("IsDeployed", false);
    }
}
