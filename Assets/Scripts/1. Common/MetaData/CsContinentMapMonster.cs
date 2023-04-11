using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsContinentMapMonster
{
	int m_nContinentId;             // 대륙ID
	CsMonsterInfo m_csMonsterInfo;  // 몬스터ID
	float m_flXPosition;            // x좌표	 
	float m_flYPosition;            // y좌표
	float m_flZPosition;            // z좌표	

	//---------------------------------------------------------------------------------------------------
	public int ContinentId
	{
		get { return m_nContinentId; }
	}

	public CsMonsterInfo MonsterInfo
	{
		get { return m_csMonsterInfo; }
	}

	public float XPosition
	{
		get { return m_flXPosition; }
	}

	public float YPosition
	{
		get { return m_flYPosition; }
	}

	public float ZPosition
	{
		get { return m_flZPosition; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsContinentMapMonster(WPDContinentMapMonster continentMapMonster)
	{
		m_nContinentId = continentMapMonster.continentId;
		m_csMonsterInfo = CsGameData.Instance.GetMonsterInfo(continentMapMonster.monsterId) ;
		m_flXPosition = continentMapMonster.xPosition;
		m_flYPosition = continentMapMonster.yPosition;
		m_flZPosition = continentMapMonster.zPosition;
	}
}
