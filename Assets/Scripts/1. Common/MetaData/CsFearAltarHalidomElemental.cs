using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsFearAltarHalidomElemental
{
	int m_nHalidomElementalId;
	string m_strName;
	CsItemReward m_csItemRewardCollection;

	//---------------------------------------------------------------------------------------------------
	public int HalidomElementalId
	{
		get { return m_nHalidomElementalId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsItemReward CollectionItemReward
	{
		get { return m_csItemRewardCollection; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarHalidomElemental(WPDFearAltarHalidomElemental fearAltarHalidomElemental)
	{
		m_nHalidomElementalId = fearAltarHalidomElemental.halidomElementalId;
		m_strName = CsConfiguration.Instance.GetString(fearAltarHalidomElemental.nameKey);
		m_csItemRewardCollection = CsGameData.Instance.GetItemReward(fearAltarHalidomElemental.collectionItemRewardId);
	}
}
