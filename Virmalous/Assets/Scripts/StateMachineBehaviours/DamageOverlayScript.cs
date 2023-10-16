using UnityEngine;

public class DamageOverlayScript : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("GotHit", false);
    }
}
