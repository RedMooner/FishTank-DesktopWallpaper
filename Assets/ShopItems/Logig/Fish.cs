using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Fish", menuName = "Fish")]
public class Fish : ScriptableObject
{
    // Start is called before the first frame update
    public string name;
    public string description;
    // public GameObject Prefab;
    public GameObject prefab;
    public GameObject prefabUI;
    public int price;
    public Color colour;
}
