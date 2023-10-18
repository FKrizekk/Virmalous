using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Announcer : MonoBehaviour
{
    public Transform announcerParent;
    public GameObject itemPrefab;
    public Item[] items;

    public void AnnounceItem(string tag)
    {
        foreach (var item in items)
        {
            if (item.tag == tag)
            {
                var itemObj = Instantiate(itemPrefab, announcerParent);
                itemObj.GetComponent<AnnounceItemScript>().Setup(item.itemName, item.image);
            }
        }
    }
}