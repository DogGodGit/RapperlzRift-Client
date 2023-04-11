using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardCollectionGrade
{
	int m_nGrade;
	string m_strColorCode;
	int m_nCollectionFamePoint;

	//---------------------------------------------------------------------------------------------------
	public int Grade
	{
		get { return m_nGrade; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	public int CollectionFamePoint
	{
		get { return m_nCollectionFamePoint; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardCollectionGrade(WPDCreatureCardCollectionGrade creatureCardCollectionGrade)
	{
		m_nGrade = creatureCardCollectionGrade.grade;
		m_strColorCode = creatureCardCollectionGrade.colorCode;
		m_nCollectionFamePoint = creatureCardCollectionGrade.collectionFamePoint;
	}
}
