using UnityEngine;

public class CsAnimStateJobChange : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("jobchange", false);
    }
}