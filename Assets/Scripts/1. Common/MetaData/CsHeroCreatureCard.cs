using ClientCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsHeroCreatureCard
{
	CsCreatureCard m_csCreatureCard;
	int m_nCount;

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCard CreatureCard
	{
		get { return m_csCreatureCard; }
	}

	public int Count
	{
		get { return m_nCount; }
		set { m_nCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroCreatureCard(PDHeroCreatureCard heroCreatureCard)
	{
		m_csCreatureCard = CsGameData.Instance.GetCreatureCard(heroCreatureCard.creatureCardId);
		m_nCount = heroCreatureCard.count;
	}
}
