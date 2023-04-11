using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-13)
//---------------------------------------------------------------------------------------------------

public class CsSubGearSoulstoneSocket
{
	int m_nSubGearId;										// 보조장비ID
	int m_nSocketIndex;										// 소켓인덱스
	int m_nItemType;										// 소울스톤아이템타입
	CsSubGearGrade m_csSubGearGradeRequiredSubGearGrade;    // 개방필요보조장비등급

	//---------------------------------------------------------------------------------------------------
	public int SubGearId
	{
		get { return m_nSubGearId; }	
	}

	public int SocketIndex
	{
		get { return m_nSocketIndex; }
	}

	public int ItemType
	{
		get { return m_nItemType; }
	}

	public CsSubGearGrade RequiredSubGearGrade
	{
		get { return m_csSubGearGradeRequiredSubGearGrade; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearSoulstoneSocket(WPDSubGearSoulstoneSocket subGearGemSocket)
	{
		m_nSubGearId = subGearGemSocket.subGearId;
		m_nSocketIndex = subGearGemSocket.socketIndex;
		m_nItemType = subGearGemSocket.itemType;
		m_csSubGearGradeRequiredSubGearGrade = CsGameData.Instance.GetSubGearGrade(subGearGemSocket.requiredSubGearGrade);
	}

}
