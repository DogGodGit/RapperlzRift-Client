using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsMonsterArrange
{
	long m_lMonsterArrangeId;
	int m_nMonsterId;

	//---------------------------------------------------------------------------------------------------
	public long MonsterArrangeId
	{
		get { return m_lMonsterArrangeId; }
	}

	public int MonsterId
	{
		get { return m_nMonsterId; }
	}
	//---------------------------------------------------------------------------------------------------
	public CsMonsterArrange(WPDMonsterArrange monsterArrange)
	{
		m_lMonsterArrangeId = monsterArrange.monsterArrangeId;
		m_nMonsterId = monsterArrange.monsterId;
	}
}
