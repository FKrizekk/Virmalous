using UnityEngine;

public class RevolverStateMachine : StateMachineBehaviour {
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
		if(stateInfo.IsTag("reload"))
		{
			animator.SetBool("Reload", false);
			PlayerScript.isReloading = true;
		}
	}
	
	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
		Revolver revolver = GameObject.Find("revolverBasic(Clone)").GetComponent<Revolver>();
        animator.SetBool("Shoot", false);
        if (animatorStateInfo.IsTag("reload"))
		{
			PlayerScript.isReloading = false;
		}
	}
}