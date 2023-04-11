using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsMountAwakeningLevelMaster
{
	int m_nAwakeningLevel;				// 0부터
	float m_flUnequippedAttrFactor;

	//---------------------------------------------------------------------------------------------------
	public int AwakeningLevel
	{
		get { return m_nAwakeningLevel; }
	}

	public float UnequippedAttrFactor
	{
		get { return m_flUnequippedAttrFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountAwakeningLevelMaster(WPDMountAwakeningLevelMaster mountAwakeningLevelMaster)
	{
		m_nAwakeningLevel = mountAwakeningLevelMaster.awakeningLevel;
		m_flUnequippedAttrFactor = mountAwakeningLevelMaster.unequippedAttrFactor;
	}
}
