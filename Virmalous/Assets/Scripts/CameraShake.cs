using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	float shakeMagn;
	
	bool shaking = false;
	
	void Update()
	{
		if(shaking)
		{
			transform.localPosition = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * shakeMagn;
		}
	}
	
	public void Shake(float time, float magnitude)
	{
		StartCoroutine(shake(time, magnitude));
	}
	
	IEnumerator shake(float time, float magnitude)
	{
		shakeMagn = magnitude;
		shaking = true;
		
		yield return new WaitForSeconds(time);
		
		transform.localPosition = Vector3.zero;
		
		shaking = false;
	}
}