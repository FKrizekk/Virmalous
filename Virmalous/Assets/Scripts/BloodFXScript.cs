using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFXScript : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		Invoke("End",5);
	}
	
	void End()
	{
		Destroy(gameObject);
	}
}
