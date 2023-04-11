using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public class CsSystemMessage
{
	CsSystemMessageInfo m_csSystemMessageInfo;
	CsNation m_csNation;
	Guid m_guidHeroId;
	string m_strHeroName;

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessageInfo SystemMessageInfo
	{
		get { return m_csSystemMessageInfo; }
	}

	public CsNation Nation
	{
		get { return m_csNation; }
	}

	public Guid HeroId
	{
		get { return m_guidHeroId; }
	}

	public string HeroName
	{
		get { return m_strHeroName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessage(PDSystemMessage systemMessage)
	{
		m_csSystemMessageInfo = CsGameData.Instance.GetSystemMessageInfo(systemMessage.id);
		m_csNation = CsGameData.Instance.GetNation(systemMessage.nationId);
		m_guidHeroId = systemMessage.heroId;
		m_strHeroName = systemMessage.heroName;
	}

}
