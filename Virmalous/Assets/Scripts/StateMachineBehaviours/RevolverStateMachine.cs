using UnityEngine;

public class RevolverStateMachine : StateMachineBehaviour {
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool("Shoot", false);
		if(stateInfo.IsTag("reload"))
		{
			animator.SetBool("Reload", false);
		}
	}
	
	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
		Revolver revolver = GameObject.Find("revolverBasic(Clone)").GetComponent<Revolver>();
		
		if(animatorStateInfo.IsTag("reload"))
		{
			PlayerScript.isReloading = false;
		}
	}
}