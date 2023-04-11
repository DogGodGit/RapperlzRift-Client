using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-26)
//---------------------------------------------------------------------------------------------------

public class CsStaminaRecoverySchedule
{
	int m_nScheduleId;
	int m_nRecoveryTime;
	int m_nRecoveryStamina;

	//---------------------------------------------------------------------------------------------------
	public int ScheduleId
	{
		get { return m_nScheduleId; }
	}

	public int RecoveryTime
	{
		get { return m_nRecoveryTime; }
	}

	public int RecoveryStamina
	{
		get { return m_nRecoveryStamina; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsStaminaRecoverySchedule(WPDStaminaRecoverySchedule staminaRecoverySchedule)
	{
		m_nScheduleId = staminaRecoverySchedule.scheduleId;
		m_nRecoveryTime = staminaRecoverySchedule.recoveryTime;
		m_nRecoveryStamina = staminaRecoverySchedule.recoveryStamina;
	}
}
