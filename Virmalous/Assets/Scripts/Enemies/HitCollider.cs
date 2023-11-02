using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class hitInfo
{
	public DamageInfo damageInfo;
	public Vector3 point;
}

[System.Serializable]
public class DamageMultiplier
{
    public float damageMultiplier;
    public float stunDamageMultiplier;
    public float fireDamageMultiplier;
    public float freezeDamageMultiplier;
    public float electricityDamageMultiplier;
}

public class HitCollider : MonoBehaviour
{
	public GameObject targetObject;
	public string methodName = "GotHit";
	public DamageMultiplier damageMultiplier;
	
	public hitInfo info;
	
	public void Hit(DamageInfo damageInfo, Vector3 point)
	{
		info.damageInfo.damage = damageInfo.damage * damageMultiplier.damageMultiplier;
        info.damageInfo.stunDamage = damageInfo.stunDamage * damageMultiplier.stunDamageMultiplier;
        info.damageInfo.fireDamage = damageInfo.fireDamage * damageMultiplier.fireDamageMultiplier;
        info.damageInfo.freezeDamage = damageInfo.freezeDamage * damageMultiplier.freezeDamageMultiplier;
        info.damageInfo.electricityDamage = damageInfo.electricityDamage * damageMultiplier.electricityDamageMultiplier;

		info.point = point;
		
		targetObject.SendMessage(methodName, info, SendMessageOptions.RequireReceiver);
	}
}