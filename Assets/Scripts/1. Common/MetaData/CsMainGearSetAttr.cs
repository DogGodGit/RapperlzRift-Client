using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public class CsMainGearSetAttr
{
	int m_nTier;
	int m_nGrade;
	int m_nQuality;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValueInfo;

	//---------------------------------------------------------------------------------------------------
	public int Tier
	{
		get { return m_nTier; }
	}

	public int Grade
	{
		get { return m_nGrade; }
	}

	public int Quality
	{
		get { return m_nQuality; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValueInfo
	{
		get { return m_csAttrValueInfo; }
	}

	public int BattlePower
	{
		get { return m_csAttrValueInfo.Value * m_csAttr.BattlePowerFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearSetAttr(WPDMainGearSetAttr mainGearSetAttr)
	{
		m_nTier = mainGearSetAttr.tier;
		m_nGrade = mainGearSetAttr.grade;
		m_nQuality = mainGearSetAttr.quality;
		m_csAttr = CsGameData.Instance.GetAttr(mainGearSetAttr.attrId);
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(mainGearSetAttr.attrValueId);
	}
}
