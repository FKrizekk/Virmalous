using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    public TMP_Text valueText;

    public void ValueChanged()
    {
        valueText.text = GetComponent<Slider>().value.ToString();
    }
}
