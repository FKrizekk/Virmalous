using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
	//Medkit
	public static int medkitCount = 0;
	
	//Dash
	public static float dashCooldown = 2f;
	public static float dashCharge = 0f;
	
	//GravBlade
	public static float gravBladeStunTime = 3f;
	public static float gravBladeTeleportCooldown = 0.5f;
	public static float gravBladeCooldown = 5f;
	public static float gravBladeCharge = 0f;

    private void Update()
    {
		if (Input.GetKeyDown("e") && medkitCount > 0)
		{
			medkitCount--;
			PlayerScript.ChangeHealth(5000);
		}
    }
}