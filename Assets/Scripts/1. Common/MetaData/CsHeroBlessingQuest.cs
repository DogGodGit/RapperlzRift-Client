using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsHeroBlessingQuest
{
	long m_lId;
	Guid m_guidTargetHeroId;
	string m_strTargetName;
	CsBlessingTargetLevel m_csBlessingTargetLevel;

	//---------------------------------------------------------------------------------------------------
	public long Id
	{
		get { return m_lId; }
	}

	public Guid TargetHeroId
	{
		get { return m_guidTargetHeroId; }
	}

	public string TargetName
	{
		get { return m_strTargetName; }
	}

	public CsBlessingTargetLevel BlessingTargetLevel
	{
		get { return m_csBlessingTargetLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroBlessingQuest(PDHeroBlessingQuest heroBlessingQuest)
	{
		m_lId = heroBlessingQuest.id;
		m_guidTargetHeroId = heroBlessingQuest.targetHeroId;
		m_strTargetName = heroBlessingQuest.targetName;
		m_csBlessingTargetLevel = CsGameData.Instance.GetBlessingTargetLevel(heroBlessingQuest.blessingTargetLevelId);
	}
}
