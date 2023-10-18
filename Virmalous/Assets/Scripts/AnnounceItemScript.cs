using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AnnounceItemScript : MonoBehaviour
{
    public Image _itemImage;
    public TMP_Text _itemName;

    public void Setup(string itemName, Sprite itemImage)
    {
        _itemName.text = itemName;
        _itemImage.sprite = itemImage;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}