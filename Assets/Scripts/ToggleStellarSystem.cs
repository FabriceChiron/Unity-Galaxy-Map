using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleStellarSystem : MonoBehaviour
{
    [SerializeField]
    private Scales scales;

    [SerializeField]
    private ToggleSetting _toggleValues;

    [SerializeField]
    private TextMeshProUGUI _infos;

    [SerializeField]
    private float _resetTimeToScale = 1f;
    private float _timeToScale;
    private float _stellarSystemOriginalScale;
    private float _stellarSystemCurrentScale;
    private float _stellarSystemTargetScale;

    private float velocity;

    private Animator _animator;
    private bool _isAnimating;
    private bool _isScaleChanging;


    public Animator Animator { get => _animator; set => _animator = value; }
    public bool IsAnimating { get => _isAnimating; set => _isAnimating = value; }
    public float StellarSystemOriginalScale { get => _stellarSystemOriginalScale; set => _stellarSystemOriginalScale = value; }
    public float StellarSystemCurrentScale { get => _stellarSystemCurrentScale; set => _stellarSystemCurrentScale = value; }
    public float StellarSystemTargetScale { get => _stellarSystemTargetScale; set => _stellarSystemTargetScale = value; }
    public bool IsScaleChanging { get => _isScaleChanging; set => _isScaleChanging = value; }

    private Controller _controller;




    private void Awake()
    {
        _timeToScale = 0;
        _toggleValues = GameObject.FindObjectOfType<ToggleSetting>();

        _infos = GameObject.Find("DeviceInfo").GetComponent<TextMeshProUGUI>();
        
        Debug.Log(_toggleValues.name);

        _controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetTargetScale();

        StellarSystemCurrentScale = 0f;

        _toggleValues.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            IsScaleChanging = true;
            GetTargetScale();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Animator.GetBool("IsAnimating"))
        {
            _controller.ClearTrails();
        }
        
        if (IsScaleChanging)
        {
            _controller.ClearTrails();

            StellarSystemCurrentScale = Mathf.SmoothDamp(StellarSystemCurrentScale, StellarSystemTargetScale, ref velocity, _resetTimeToScale);

            Animator.SetFloat("Scale", StellarSystemCurrentScale);

            if(Mathf.Abs(StellarSystemTargetScale - StellarSystemCurrentScale) <= 0.01f) {
                StellarSystemCurrentScale = StellarSystemTargetScale;
                Animator.SetFloat("Scale", StellarSystemTargetScale);
                IsScaleChanging = false;

                Camera.main.GetComponent<CameraFollow>().ResetCameraTarget(false);
            }

        }

        
    }

    public void toggleIsAnimating()
    {
        Animator.SetBool("IsAnimating", !Animator.GetBool("IsAnimating"));
    }

    public void ActivateUIDetails()
    {
        //Camera.main.GetComponent<CameraFollow>().ResetCameraTarget();

        foreach(StellarObject stellarObject in GameObject.FindObjectsOfType<StellarObject>())
        {
            stellarObject.UIDetails.gameObject.SetActive(true);
        }
    }

    public void ChangeStellarSystemScale(bool rationalizeValues)
    {

    }

    public void DeployStellarSystem()
    {
        Debug.Log("DeployStellarSystem");
        Animator.SetBool("IsDeployed", true);

        IsScaleChanging = true;

        GetTargetScale();
        
    }

    public void GetTargetScale()
    {
        //StellarSystemTargetScale = scales.RationalizeValues ? 1f : 0.1f;
        StellarSystemTargetScale = 1f;
        StellarSystemOriginalScale = StellarSystemTargetScale;
        _timeToScale = _resetTimeToScale;
        Debug.Log($"StellarSystemTargetScale: {StellarSystemTargetScale}");
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

        IsScaleChanging = true;
        StellarSystemTargetScale = 0;
        Animator.SetBool("IsDeployed", false);
    }
}
