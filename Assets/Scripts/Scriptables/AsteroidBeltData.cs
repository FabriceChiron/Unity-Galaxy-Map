using UnityEngine;

public enum AsteroidType
{
    Rocky,
    Icy
}

[CreateAssetMenu]
public class AsteroidBeltData : ScriptableObject
{
    public int Quantity;

    public AsteroidType asteroidType;

    public float Orbit;

    public float YearLength = 1f;
}