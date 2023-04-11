using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsStoryDungeonGuide
{
	int m_nDungeonNo;
	int m_nDifficulty;
	int m_nStep;
	int m_nNo;
	string m_strImageName;
	string m_strTitle;
	string m_strContent;

	//---------------------------------------------------------------------------------------------------
	public int DungeonNo
	{
		get { return m_nDungeonNo; }
	}

	public int Difficulty
	{
		get { return m_nDifficulty; }
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
	public CsStoryDungeonGuide(WPDStoryDungeonGuide storyDungeonGuide)
	{
		m_nDungeonNo = storyDungeonGuide.dungeonNo;
		m_nDifficulty = storyDungeonGuide.difficulty;
		m_nStep = storyDungeonGuide.step;
		m_nNo = storyDungeonGuide.no;
		m_strImageName = storyDungeonGuide.imageName;
		m_strTitle = CsConfiguration.Instance.GetString(storyDungeonGuide.titleKey);
		m_strContent = CsConfiguration.Instance.GetString(storyDungeonGuide.contentKey);
	}
}
