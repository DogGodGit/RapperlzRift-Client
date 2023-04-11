using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsArtifactAttr
{
	int m_nArtifactNo;
	CsAttr m_csAttr;

	//---------------------------------------------------------------------------------------------------
	public int ArtifactNo
	{
		get { return m_nArtifactNo; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifactAttr(WPDArtifactAttr artifactAttr)
	{
		m_nArtifactNo = artifactAttr.artifactNo;
		m_csAttr = CsGameData.Instance.GetAttr(artifactAttr.attrId);
	}
}
