using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private float _orbit;
    private float _posY;
    private float _scale;

    public float Orbit { get => _orbit; set => _orbit = value; }
    public float PosY { get => _posY; set => _posY = value; }
    public float Scale { get => _scale; set => _scale = value; }
}
