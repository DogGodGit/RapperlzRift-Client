using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsWorldLevelExpFactor
{
	int m_nLevelGap;
	float m_flExpFactor;

	//---------------------------------------------------------------------------------------------------
	public int LevelGap
	{
		get { return m_nLevelGap; }
	}

	public float ExpFactor
	{
		get { return m_flExpFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWorldLevelExpFactor(WPDWorldLevelExpFactor worldLevelExpFactor)
	{
		m_nLevelGap = worldLevelExpFactor.levelGap;
		m_flExpFactor = worldLevelExpFactor.expFactor;
	}
}
