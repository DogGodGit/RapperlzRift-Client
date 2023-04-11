using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsEliteMonsterKillAttrValue
{
	int m_nEliteMonsterId;
	int m_nKillCount;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int EliteMonsterId
	{
		get { return m_nEliteMonsterId; }
	}

	public int KillCount
	{
		get { return m_nKillCount; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonsterKillAttrValue(WPDEliteMonsterKillAttrValue eliteMonsterKillAttrValue)
	{
		m_nEliteMonsterId = eliteMonsterKillAttrValue.eliteMonsterId;
		m_nKillCount = eliteMonsterKillAttrValue.killCount;
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(eliteMonsterKillAttrValue.attrValueId);
	}
}
