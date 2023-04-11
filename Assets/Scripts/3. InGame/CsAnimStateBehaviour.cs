using UnityEngine;

public class CsAnimStateBehaviour : StateMachineBehaviour 
{
	[SerializeField]
	int m_nKey;

	public int Key {get{return m_nKey;}}

	public event Delegate<AnimatorStateInfo, int> EventStateEnter;
	public event Delegate<AnimatorStateInfo, int> EventStateExit;
//	public event Delegate<AnimatorStateInfo> EventStateMove;
//	public event Delegate<AnimatorStateInfo> EventStateUpdate;

//	public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
//	{
//		Debug.Log("CsAnimStateBehaviour.OnStateMachineEnter");
//		if (EventStateEnter != null)
//		{
//			EventStateEnter(stateInfo);
//		}
//	}

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (EventStateEnter != null)
		{
			EventStateEnter(stateInfo, m_nKey);
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (EventStateExit != null)
		{
			EventStateExit(stateInfo, m_nKey);
		}
	}

//	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
//	{
//		if (EventStateMove != null)
//		{
//			EventStateMove(stateInfo);
//		}
//	}
//
//	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
//	{
//		if (EventStateUpdate != null)
//		{
//			EventStateUpdate(stateInfo);
//		}
//	}

}

