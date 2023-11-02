using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravBladeSMB : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //StopThrow(animator);
        animator.SetBool("Throw", false);
    }

    IEnumerator StopThrow(Animator anim)
    {
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("Throw", false);
    }
}
