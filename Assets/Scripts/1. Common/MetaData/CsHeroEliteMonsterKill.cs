using ClientCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsHeroEliteMonsterKill
{
	CsEliteMonster m_csEliteMonster;
	int m_nKillCount;

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonster EliteMonster
	{
		get { return m_csEliteMonster; }
	}

	public int KillCount
	{
		get { return m_nKillCount; }
		set { m_nKillCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroEliteMonsterKill(PDHeroEliteMonsterKill heroEliteMonsterKill)
	{
		m_csEliteMonster = CsGameData.Instance.GetEliteMonster(heroEliteMonsterKill.eliteMonsterId);
		m_nKillCount = heroEliteMonsterKill.killCount;
	}

    //---------------------------------------------------------------------------------------------------
    public CsHeroEliteMonsterKill(int nEliteMonsterId, int nKillCount)
    {
        m_csEliteMonster = CsGameData.Instance.GetEliteMonster(nEliteMonsterId);
        m_nKillCount = nKillCount;
    }

	public CsEliteMonsterKillAttrValue GetEliteMonsterKillAttrValue()
	{
		return m_csEliteMonster.GetEliteMonsterKillAttrValue(m_nKillCount);
	}
}
