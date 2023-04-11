using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-06-04)
//---------------------------------------------------------------------------------------------------

public class CsMenuContentOpenPreview
{
	int m_nPreviewNo;
	CsMenuContent m_csMenuContent;

	//---------------------------------------------------------------------------------------------------
	public int PreviewNo
	{
		get { return m_nPreviewNo; }
	}

	public CsMenuContent MenuContent
	{
		get { return m_csMenuContent; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMenuContentOpenPreview(WPDMenuContentOpenPreview menuContentOpenPreview)
	{
		m_nPreviewNo = menuContentOpenPreview.previewNo;
		m_csMenuContent = CsGameData.Instance.GetMenuContent(menuContentOpenPreview.contentId);
	}

}
