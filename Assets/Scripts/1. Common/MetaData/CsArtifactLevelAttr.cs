using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsArtifactLevelAttr
{
	int m_nArtifactNo;
	int m_nLevel;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int ArtifactNo
	{
		get { return m_nArtifactNo; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifactLevelAttr(WPDArtifactLevelAttr artifactLevelAttr)
	{
		m_nArtifactNo = artifactLevelAttr.artifactNo;
		m_nLevel = artifactLevelAttr.level;
		m_csAttr = CsGameData.Instance.GetAttr(artifactLevelAttr.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(artifactLevelAttr.attrValueId);
	}
}
