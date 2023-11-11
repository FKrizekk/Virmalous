using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
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
	[SerializeField] private float gravBladeThrowStrength;
    [SerializeField] private LineRenderer lineRenderer;
	[SerializeField] private Transform releasePosition;
	[Header("DisplayControls")]
	[SerializeField]
	[Range(10, 100)]
	private int linePoints = 25;
	[SerializeField]
	[Range(0.01f, 0.25f)]
	private float timeBetweenPoints = 0.1f;

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
				ReadyGravBlade();
				gravBladeLastUseTime = Time.time;
			}
			if (Input.GetKeyUp("q") && gravBladeAnim.GetBool("Ready"))
			{
				ThrowGravBlade();
			}
			if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - dashLastUseTime >= dashCooldown)
			{
				GetComponent<PlayerMovement>().Dash();
				dashLastUseTime = Time.time;
			}
		}

		if (gravBladeAnim.GetBool("Ready"))
		{
			DrawProjection();
		}
		else
		{
			lineRenderer.enabled = false;
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

	void ReadyGravBlade()
	{
		gravBladeAnim.SetBool("Ready", true);
	}

	void ThrowGravBlade()
	{
		gravBladeAnim.SetBool("Ready", false);
		gravBladeAnim.SetBool("Throw", true);
		Rigidbody gravBlade = Instantiate(gravBladePrefab, releasePosition.position, Quaternion.LookRotation(PlayerScript.cam.transform.forward)).GetComponent<Rigidbody>();

        gravBlade.velocity = Vector3.zero;
        gravBlade.angularVelocity = Vector3.zero;
        gravBlade.isKinematic = false;
        gravBlade.freezeRotation = false;
        gravBlade.transform.SetParent(null, true);
        gravBlade.AddForce(PlayerScript.cam.transform.forward * gravBladeThrowStrength, ForceMode.Impulse);
    }

	private void DrawProjection()
	{
		lineRenderer.enabled = true;
		lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
		Vector3 startPosition = releasePosition.position;
		Vector3 startVelocity = gravBladeThrowStrength * PlayerScript.cam.transform.forward / 0.3f;
		int i = 0;
		lineRenderer.SetPosition(i, startPosition);
		for (float time = 0; time < linePoints; time += timeBetweenPoints)
		{
			i++;
			Vector3 point = startPosition + time * startVelocity;
			point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

			lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = lineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition,
                (point - lastPosition).normalized,
                out RaycastHit hit,
                (point - lastPosition).magnitude,
                PlayerScript.layerMask))
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;
                return;
            }
        }
	}
}