using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject interactText;
    public GameObject crosshair;

    Interactable interactable;

    void Update()
    {
        InteractionCheck();
    }

    void InteractionCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerScript.cam.transform.position, PlayerScript.cam.transform.forward, out hit, 6, PlayerScript.layerMask))
        {
            if (hit.collider.gameObject.tag == "Interactable")
            {
                PlayerScript.isInteracting = true;
                interactable = hit.collider.GetComponentInParent<Interactable>();
                if (Input.GetKeyDown("f"))
                {
                    interactable.Interact();
                }
            }
            else
            {
                PlayerScript.isInteracting = false;
            }
        }
        else
        {
            PlayerScript.isInteracting = false;
        }

        interactText.SetActive(PlayerScript.isInteracting);
        crosshair.SetActive(!PlayerScript.isInteracting);
    }
}
