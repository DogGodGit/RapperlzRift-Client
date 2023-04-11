using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardCollectionEntry
{
	CsCreatureCardCollection m_csCsCreatureCardCollection;
	CsCreatureCard m_csCreatureCard;

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardCollection CreatureCardCollection
	{
		get { return m_csCsCreatureCardCollection; }
	}

	public CsCreatureCard CreatureCard
	{
		get { return m_csCreatureCard; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardCollectionEntry(WPDCreatureCardCollectionEntry creatureCardCollectionEntry)
	{
		m_csCsCreatureCardCollection = CsGameData.Instance.GetCreatureCardCollection(creatureCardCollectionEntry.collectionId);
		m_csCreatureCard = CsGameData.Instance.GetCreatureCard(creatureCardCollectionEntry.creatureCardId);
	}
}
