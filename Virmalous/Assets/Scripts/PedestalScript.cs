using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalScript : MonoBehaviour
{
	public void GetCrystal()
	{
		Destroy(transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
	}
}