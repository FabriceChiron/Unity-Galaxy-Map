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

    public string StarDescription;

    //Texture file for star
    public Texture Texture;

    public Material Material;

    public float Top;

    public float Left;

    public PlanetData[] ChildrenItem;
}
