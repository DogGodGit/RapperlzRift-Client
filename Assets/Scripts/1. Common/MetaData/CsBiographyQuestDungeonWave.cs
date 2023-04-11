using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-23)
//---------------------------------------------------------------------------------------------------

public class CsBiographyQuestDungeonWave
{
	int m_nDungeonId;
	int m_nWaveNo;
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nTargetType;
	int m_nTargetArrangeKey;

	//---------------------------------------------------------------------------------------------------
	public int DungeonId
	{
		get { return m_nDungeonId; }
	}

	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public int TargetType
	{
		get { return m_nTargetType; }
	}

	public int TargetArrangeKey
	{
		get { return m_nTargetArrangeKey; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsBiographyQuestDungeonWave(WPDBiographyQuestDungeonWave biographyQuestDungeonWave)
	{
		m_nDungeonId = biographyQuestDungeonWave.dungeonId;
		m_nWaveNo = biographyQuestDungeonWave.waveNo;
		m_strTargetTitle = CsConfiguration.Instance.GetString(biographyQuestDungeonWave.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(biographyQuestDungeonWave.targetContentKey);
		m_nTargetType = biographyQuestDungeonWave.targetType;
		m_nTargetArrangeKey = biographyQuestDungeonWave.targetArrangeKey;
	}
}
