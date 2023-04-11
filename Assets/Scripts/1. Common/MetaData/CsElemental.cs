using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-29)
//---------------------------------------------------------------------------------------------------

/*
1 : 화염
2 : 전기
3 : 빛
4 : 어둠
*/
public enum EnElemental
{
	Fire = 1,
	Electric = 2,
	Light = 3,
	Dark = 4,
}


public class CsElemental
{
	int m_nElementalId;
	string m_strName;

	//---------------------------------------------------------------------------------------------------
	public int ElementalId
	{
		get { return m_nElementalId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsElemental(WPDElemental elemental)
	{
		m_nElementalId = elemental.elementalId;
		m_strName = CsConfiguration.Instance.GetString(elemental.nameKey);
	}
}
