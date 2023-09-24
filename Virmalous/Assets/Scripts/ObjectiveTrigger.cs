using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
	public string newObjectiveText;
	public Transform newMarkerPos;
	
	private void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Player")
		{
			if(newObjectiveText != "KEEP")
			{
				ObjectiveManager.SetObjectiveText(newObjectiveText);
			}
			if(newMarkerPos.localPosition == Vector3.zero)
			{
				ObjectiveManager.SetObjectiveMarker(Vector3.zero);
			}else
			{
				ObjectiveManager.SetObjectiveMarker(newMarkerPos.position);
			}
			
			Destroy(gameObject);
		}
	}
}
