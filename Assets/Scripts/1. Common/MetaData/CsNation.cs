using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-05-24)
//---------------------------------------------------------------------------------------------------

public class CsNation
{
	int m_nNationId;            // 국가ID
	string m_strName;			// 이름
	string m_strDescription;	// 설명

	//---------------------------------------------------------------------------------------------------
	public int NationId
	{
		get { return m_nNationId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNation(WPDNation nation)
	{
		m_nNationId = nation.nationId;
		m_strName = CsConfiguration.Instance.GetString(nation.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(nation.descriptionKey);
	}
}
