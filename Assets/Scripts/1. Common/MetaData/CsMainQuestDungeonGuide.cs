using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-04)
//---------------------------------------------------------------------------------------------------

public class CsMainQuestDungeonGuide
{
	int m_nDungeonId;
	int m_nStep;
	int m_nNo;
	string m_strImageName;
	string m_strTitle;
	string m_strContent;

	//---------------------------------------------------------------------------------------------------
	public int DungeonId
	{
		get { return m_nDungeonId; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public int No
	{
		get { return m_nNo; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public string Title
	{
		get { return m_strTitle; }
	}

	public string Content
	{
		get { return m_strContent; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuestDungeonGuide(WPDMainQuestDungeonGuide mainQuestDungeonGuide)
	{
		m_nDungeonId = mainQuestDungeonGuide.dungeonId;
		m_nStep = mainQuestDungeonGuide.step;
		m_nNo = mainQuestDungeonGuide.no;
		m_strImageName = mainQuestDungeonGuide.imageName;
		m_strTitle = CsConfiguration.Instance.GetString(mainQuestDungeonGuide.titleKey);
		m_strContent = CsConfiguration.Instance.GetString(mainQuestDungeonGuide.contentKey);
	}
}
