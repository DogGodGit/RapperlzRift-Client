using UnityEngine;

public class CsMonsterDungeonOsiris : CsMonster
{
	//---------------------------------------------------------------------------------------------------
	protected override void Update()
	{
		if (m_csMonsterInfo == null) return;

		OsirisMonsterUpdate();

		m_vtPrevPos = m_trMyTransform.position;
	}

	//---------------------------------------------------------------------------------------------------
	void OsirisMonsterUpdate()
	{
		if (m_bAppear) return;  // 등장 연출중.		

		if (m_timer.CheckSetTimer())
		{
			MoveSendToServer();
		}

		if (m_enState == EnState.Idle)
		{
			if (m_bDungeonClear)
			{
				if (Dead == false)
				{
					StartCoroutine(DelayDead(0));
				}
				return;
			}
			else
			{
				Vector3 vtTargetPos = new Vector3(CsDungeonManager.Instance.OsirisRoom.TargetXPosition, CsDungeonManager.Instance.OsirisRoom.TargetYPosition, CsDungeonManager.Instance.OsirisRoom.TargetZPosition);
				if (!IsTargetInDistance(vtTargetPos, 1f))
				{
					SetDestination(vtTargetPos, EnState.Walk);
				}
			}
		}
		else if (m_enState == EnState.Walk)
		{
			if (Dead == false)
			{
				if (m_bDungeonClear)
				{
					ChangeState(EnState.Idle);
				}
				else
				{
					WalkState();
				}
			}
		}
		else if (m_enState == EnState.Attack || m_enState == EnState.Chase || m_enState == EnState.Run)
		{
			ChangeState(EnState.Idle);
		}
		else if (m_enState == EnState.Dead)
		{
			if (GetAnimStatus() != EnAnimStatus.Dead)
			{
				ChangeState(EnState.Dead);
			}
		}
	}
}
