using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-09)
//---------------------------------------------------------------------------------------------------

public class CsWingMemoryPieceType
{
	int m_nType;
	string m_strDescription;
	CsItem m_csItemRequired;

	//---------------------------------------------------------------------------------------------------
	public int Type
	{
		get { return m_nType; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public CsItem RequiredItem
	{
		get { return m_csItemRequired; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingMemoryPieceType(WPDWingMemoryPieceType wingMemoryPieceType)
	{
		m_nType = wingMemoryPieceType.type;
		m_strDescription = CsConfiguration.Instance.GetString(wingMemoryPieceType.descriptionKey);
		m_csItemRequired = CsGameData.Instance.GetItem(wingMemoryPieceType.requiredItemId);
	}
}
