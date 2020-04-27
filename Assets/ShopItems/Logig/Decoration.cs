using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "new Decoration", menuName = "Decoration")]
public class Decoration : ScriptableObject
{
    public string name;
    public string description;
    public Image sprite;
    public GameObject prefab;
    public GameObject prefabUI;
    public int price;
}
