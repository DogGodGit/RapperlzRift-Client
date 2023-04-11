using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsCostumeEffect
{
	int m_nCostumeEffectId;
	string m_strName;
	string m_strPrefabName;
	CsItem m_csItemRequired;

	//---------------------------------------------------------------------------------------------------
	public int CostumeEffectId
	{
		get { return m_nCostumeEffectId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public CsItem RequiredItem
	{
		get { return m_csItemRequired; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostumeEffect(WPDCostumeEffect costumeEffect)
	{
		m_nCostumeEffectId = costumeEffect.costumeEffectId;
		m_strName = CsConfiguration.Instance.GetString(costumeEffect.nameKey);
		m_strPrefabName = costumeEffect.prefabName;
		m_csItemRequired = CsGameData.Instance.GetItem(costumeEffect.requiredItemId);
	}
}
