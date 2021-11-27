using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Galaxy : MonoBehaviour
{

    [SerializeField]
    private GalaxyData _galaxyData;

    [SerializeField]
    private GameObject _clusterPrefab;

    [SerializeField]
    private Transform _cameraAnchor, _cameraPivot;

    private CameraFollow _camera;

    private TextMeshProUGUI[] _clustersName;

    private Vector3 _initRotationAngles;

    private bool _isGalaxyFocused, _toLerp;

    public GalaxyData GalaxyData { get => _galaxyData; set => _galaxyData = value; }
    public GameObject ClusterPrefab { get => _clusterPrefab; set => _clusterPrefab = value; }
    public Vector3 InitRotationAngles { get => _initRotationAngles; set => _initRotationAngles = value; }
    public bool IsGalaxyFocused { get => _isGalaxyFocused; set => _isGalaxyFocused = value; }
    public bool ToLerp { get => _toLerp; set => _toLerp = value; }

    private void Awake()
    {
        _camera = Camera.main.GetComponent<CameraFollow>();

        InitRotationAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

        Debug.Log($"Position: {transform.position}\nScale: {transform.localScale}\nRotation: {transform.rotation}");

        if (GalaxyData)
        {
            foreach(ClusterData clusterData in GalaxyData.ClusterItem)
            {
                ExtractClusterData(clusterData);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _clustersName = GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI _clusterName in _clustersName)
        {
            //_clusterName.transform.parent.LookAt(_camera.transform);
            _clusterName.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }

        RotateGalaxy(IsGalaxyFocused, Vector3.zero);
        DetectClick();
    }

    private void RotateGalaxy(bool IsGalaxyFocused, Vector3 toAngles)
    {
        if(!IsGalaxyFocused)
        {
            transform.Rotate(new Vector3(0f, 1f * Time.deltaTime, 0f));
        }
        else if(ToLerp)
        {
            if (Vector3.Distance(transform.eulerAngles, toAngles) > 0.01f)
            {
                transform.rotation = Quaternion.Lerp(Quaternion.Euler((Vector3)transform.position), Quaternion.Euler(toAngles), 100f * Time.deltaTime);
                _camera.transform.LookAt(_cameraPivot);
            }
            else
            {
                transform.eulerAngles = toAngles;
                _camera.transform.LookAt(_cameraPivot);
                ToLerp = false;
            }
                //transform.rotation = Quaternion.Euler(toAngles);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toAngles), 100f * Time.deltaTime);
        }
    }



    private void ExtractClusterData(ClusterData clusterData)
    {
        GameObject _clusterPoint = Instantiate(ClusterPrefab);

        _clusterPoint.transform.parent = transform;

        float clusterLeft = (clusterData.Left - 50f) / 10f;
        float clusterTop = (clusterData.Top - 50f) / -10f;

        _clusterPoint.transform.localPosition = new Vector3(clusterLeft, 0f, clusterTop);
        _clusterPoint.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        _clusterPoint.GetComponent<ClusterPoint>().UIName.GetComponentInChildren<TextMeshProUGUI>().text = clusterData.Name;
    }

    private void DetectClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // On peut visualiser le rayon dans la scène pour debugger (n'influe en rien sur le jeu)
        Debug.DrawRay(ray.origin, ray.direction * 20f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && (hit.transform == transform))
        {
            if(Input.GetMouseButtonDown(0))
            {
                IsGalaxyFocused = true;
                ToLerp = true;
                _camera.CameraAnchor = _cameraAnchor;
                _camera.CameraAnchorObject = gameObject;
                _camera.ChangeTarget(_cameraAnchor);
                ShowClusterNames();
            }
        }
    }

    private void ShowClusterNames()
    {
        foreach (ClusterPoint _clusterPoint in GetComponentsInChildren<ClusterPoint>())
        {
            _clusterPoint.ToggleClusterName(true);
        }
    }

}
