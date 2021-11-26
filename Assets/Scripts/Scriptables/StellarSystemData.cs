using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StellarSystemData : ScriptableObject
{
    public string Name;

    //Descirption of the planet
    public string StarName;

    public float StarSize;

    public float Top;

    public float Left;

    public PlanetData[] ChildrenItem;
}
