using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-13)
//---------------------------------------------------------------------------------------------------

public class CsSubGearAttrValue
{
	int m_nSubGearId;					// 보조장비ID
	CsAttr m_csAttr;					// 속성
	int m_nLevel;						// 레벨
	int m_nQuality;						// 품질
	CsAttrValueInfo m_csAttrValueInfo;

	//---------------------------------------------------------------------------------------------------
	public int SubGearId
	{
		get { return m_nSubGearId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int Quality
	{
		get { return m_nQuality; }
	}

	public int Value
	{
		get { return m_csAttrValueInfo.Value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearAttrValue(WPDSubGearAttrValue subGearAttrValue)
	{
		m_nSubGearId = subGearAttrValue.subGearId;
		m_csAttr = CsGameData.Instance.GetAttr(subGearAttrValue.attrId);
		m_nLevel = subGearAttrValue.level;
		m_nQuality = subGearAttrValue.quality;
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(subGearAttrValue.attrValueId);
	}
}
