using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-29)
//---------------------------------------------------------------------------------------------------

public class CsMainGearRefinementRecipe
{
	int m_nProtectionCount;				// 속성보호개수
	CsItem m_csItemProtection;			// 보호아이템

	//---------------------------------------------------------------------------------------------------
	public int ProtectionCount
	{
		get { return m_nProtectionCount; }
	}

	public CsItem ProtectionItem
	{
		get { return m_csItemProtection; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearRefinementRecipe(WPDMainGearRefinementRecipe mainGearRefinementRecipe)
	{
		m_nProtectionCount = mainGearRefinementRecipe.protectionCount;
		m_csItemProtection = CsGameData.Instance.GetItem(mainGearRefinementRecipe.protectionItemId);
	}
}
