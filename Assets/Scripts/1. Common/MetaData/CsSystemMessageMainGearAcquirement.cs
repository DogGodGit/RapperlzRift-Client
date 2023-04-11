using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public class CsSystemMessageMainGearAcquirement : CsSystemMessage
{
	CsMainGear m_csMainGear;

	//---------------------------------------------------------------------------------------------------
	public CsMainGear MainGear
	{
		get { return m_csMainGear; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessageMainGearAcquirement(PDMainGearAcquirementSystemMessage mainGearAcquirementSystemMessage) 
		: base(mainGearAcquirementSystemMessage)
	{
		m_csMainGear = CsGameData.Instance.GetMainGear(mainGearAcquirementSystemMessage.mainGearId);
	}
}
