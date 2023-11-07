using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManager : MonoBehaviour
{
	//Medkit
	[Header("Medkit")]
	public static int medkitCount = 0;
	float medkitCooldown = 4f;
	float medkitLastUsedTime = 0f;
	public Image medkitImage;
	public TMP_Text medkitText;

	//Dash
	[Header("Dash")]
	public static float dashCooldown = 2f;
	public static float dashCharge = 0f;
	float dashLastUseTime = 0f;
	public Image dashImage;

	//GravBlade
	[Header("GravBlade")]
	public static float gravBladeStunTime = 3f;
	public static float gravBladeTeleportCooldown = 0.5f;
	public static float gravBladeCooldown = 5f;
	public static float gravBladeCharge = 0f;
	float gravBladeLastUseTime = 0f;
	public Image gravBladeImage;
	public GameObject gravBladePrefab;
	public Animator gravBladeAnim;

    private void Update()
    {
		if (PlayerScript.canMove)
		{
            if (Input.GetKeyDown("e") && medkitCount > 0 && Time.time - medkitLastUsedTime >= medkitCooldown)
            {
                medkitCount--;
                PlayerScript.ChangeHealth(5000);
                medkitLastUsedTime = Time.time;
            }
            if (Input.GetKeyDown("q") && Time.time - gravBladeLastUseTime >= gravBladeCooldown)
            {
                ThrowGravBlade();
                gravBladeLastUseTime = Time.time;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - dashLastUseTime >= gravBladeCooldown)
            {
                GetComponent<PlayerMovement>().Dash();
                dashLastUseTime = Time.time;
            }
        }

		UpdateAbilites();
    }

	void UpdateAbilites()
	{
		//Medkit
		medkitImage.fillAmount = 1 - Mathf.Clamp((Time.time - medkitLastUsedTime) / medkitCooldown, 0, 1);
		medkitText.text = medkitCount.ToString();

		//Dash
		dashImage.fillAmount = Mathf.Clamp((Time.time - dashLastUseTime) / dashCooldown, 0, 1);

		//GravBlade
		gravBladeImage.fillAmount = Mathf.Clamp((Time.time - gravBladeLastUseTime) / gravBladeCooldown, 0, 1);
	}

	void ThrowGravBlade()
	{
		gravBladeAnim.SetBool("Throw", true);
		Instantiate(gravBladePrefab, PlayerScript.cam.transform.position - new Vector3(0f,0.5f,0f), Quaternion.LookRotation(PlayerScript.cam.transform.forward));
	}
}