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

    private Transform _camera;

    private TextMeshPro[] _clustersName;

    private Vector3 _initRotationAngles;

    public GalaxyData GalaxyData { get => _galaxyData; set => _galaxyData = value; }
    public GameObject ClusterPrefab { get => _clusterPrefab; set => _clusterPrefab = value; }
    public Vector3 InitRotationAngles { get => _initRotationAngles; set => _initRotationAngles = value; }

    private void Awake()
    {
        _camera = Camera.main.transform;

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
        _clustersName = GetComponentsInChildren<TextMeshPro>();

        foreach (TextMeshPro _clusterName in _clustersName)
        {
            _clusterName.transform.parent.LookAt(_camera);
        }

        RotateGalaxy();
    }

    private void RotateGalaxy()
    {
        transform.Rotate(new Vector3(0f, 1f * Time.deltaTime, 0f));
    }

    private void ExtractClusterData(ClusterData clusterData)
    {
        GameObject _clusterPoint = Instantiate(ClusterPrefab);

        _clusterPoint.transform.parent = transform;

        float clusterLeft = (clusterData.Left - 50f) / 10f;
        float clusterTop = (clusterData.Top - 50f) / -10f;

        _clusterPoint.transform.localPosition = new Vector3(clusterLeft, 0f, clusterTop);
        _clusterPoint.GetComponentInChildren<TextMeshPro>().text = clusterData.Name;
    }


}
