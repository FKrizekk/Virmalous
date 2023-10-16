using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class hitInfo
{
	public int amount;
	public Vector3 point;
}

public class HitCollider : MonoBehaviour
{
	public GameObject targetObject;
	public string methodName = "GotHit";
	public int damageMultiplier = 1;
	
	public hitInfo info;
	
	public void Hit(int amount, Vector3 point)
	{
		info.amount = amount*damageMultiplier;
		info.point = point;
		
		targetObject.SendMessage(methodName, info, SendMessageOptions.RequireReceiver);
	}
}