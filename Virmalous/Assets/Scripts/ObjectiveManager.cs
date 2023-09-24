using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
	public static Vector3 currentObjectivePos = Vector3.zero;
	
	public Image objectiveMarker;
	public GameObject arrow;
	public TMP_Text objText;
	public static TMP_Text objectiveText;
	
	void Start()
	{
		objectiveText = objText;
	}
	
	void Update()
	{
		//Update the screen position of the objective marker
		Vector3 screenPos = Vector3.zero;
		if(currentObjectivePos != Vector3.zero)
		{
			screenPos = PlayerScript.cam.transform.GetChild(0).gameObject.GetComponent<Camera>().WorldToScreenPoint(currentObjectivePos);
		}
		objectiveMarker.enabled = screenPos.z > 0 && currentObjectivePos != Vector3.zero;
		arrow.SetActive(currentObjectivePos != Vector3.zero && (screenPos.z < 0 || objectiveMarker.transform.position.x > Screen.width || objectiveMarker.transform.position.x < 0 || objectiveMarker.transform.position.y > Screen.height || objectiveMarker.transform.position.y < 0));
		objectiveMarker.transform.position = screenPos;
		
		//Update objective arrow direction if the marker is off-screen
		if(screenPos.z < 0 || objectiveMarker.transform.position.x > Screen.width || objectiveMarker.transform.position.x < 0 || objectiveMarker.transform.position.y > Screen.height || objectiveMarker.transform.position.y < 0)
		{
			if(screenPos.z < 0)
			{
				Debug.DrawRay(arrow.transform.position, objectiveMarker.transform.position - arrow.transform.position);
				Debug.DrawRay(arrow.transform.position, -Vector2.left*100, Color.blue);
				float angle = Vector2.SignedAngle(objectiveMarker.transform.position - arrow.transform.position, -Vector2.left);
				
				arrow.transform.eulerAngles = new Vector3(0,0,-angle+180);
			}else
			{
				Debug.DrawRay(arrow.transform.position, objectiveMarker.transform.position - arrow.transform.position);
				Debug.DrawRay(arrow.transform.position, -Vector2.left*100, Color.blue);
				float angle = Vector2.SignedAngle(objectiveMarker.transform.position - arrow.transform.position, -Vector2.left);
				
				arrow.transform.eulerAngles = new Vector3(0,0,-angle);
			}
		}else
		{
			
		}
	}
	
	public static void SetObjectiveText(string newText)
	{
		objectiveText.text = newText;	
	}
	
	public static void SetObjectiveMarker(Vector3 pos)
	{
		currentObjectivePos = pos;
	}
}
