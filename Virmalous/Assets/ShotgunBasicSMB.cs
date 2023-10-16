using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBasicSMB : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Shoot", false);
        if (stateInfo.IsTag("reload"))
        {
            animator.SetBool("Reload", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        ShotgunBasic shotgun = GameObject.Find("ShotgunBasic(Clone)").GetComponent<ShotgunBasic>();

        if (animatorStateInfo.IsTag("reload"))
        {
            PlayerScript.isReloading = false; 
            PlayerScript.ammoCounts[shotgun.weaponIndex] = shotgun.maxAmmo;
        }
    }
}
