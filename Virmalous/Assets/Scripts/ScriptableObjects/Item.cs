using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite image;
    public string description;
    public string tag;
}